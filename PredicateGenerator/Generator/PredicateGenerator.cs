using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using PredicateGenerator.Attribute;
using PredicateGenerator.Enums;
using PredicateGenerator.Extensions;

namespace PredicateGenerator.Generator
{
    public class PredicateGenerator<T1> : IPredicateGenerator<T1>
    {
        private Expression<Func<T1, bool>> _where = o => true;

        public PredicateGenerator<T1> GeneratePredicate<T2>(T2 searchParametersDto)
        {
            foreach (var prop in searchParametersDto.GetType().GetProperties())
            {
                var attributes = prop.CustomAttributes.Where(o => o.AttributeType == typeof(Predicate)).ToList();
                if (attributes.Any())
                {
                    var value = prop.GetValue(searchParametersDto, null);
                    if (ObjectHasValue(value))
                    {
                        foreach (var attribute in attributes)
                        {
                            var collection = ConvertToNullableCollection(value);
                            switch ((PredicateConnectorType) attribute.ConstructorArguments[2].Value)
                            {
                                case PredicateConnectorType.And:
                                    _where = _where.And(BuildPredicate(
                                        collection.Any() ? collection : value,
                                        (PredicateExpressionType) attribute.ConstructorArguments[0].Value,
                                        (string) attribute.ConstructorArguments[1].Value));
                                    continue;
                                case PredicateConnectorType.Or:
                                    _where = _where.Or(BuildPredicate(
                                        collection.Any() ? collection : value,
                                        (PredicateExpressionType) attribute.ConstructorArguments[0].Value,
                                        (string) attribute.ConstructorArguments[1].Value));
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }

            return this;

        }

        public PredicateGenerator<T1> WithAdvancedPredicate(Expression<Func<T1, bool>> predicate)
        {
            _where = _where.And(predicate);
            return this;
        }

        public Func<T1, bool> Compile()
        {
            return _where.Compile();
        }
        private static Expression<Func<T1, bool>> BuildPredicate(object value, PredicateExpressionType comparer, string property)
        {
            var parameterExpression = Expression.Parameter(typeof(T1), typeof(T1).Name);
            return (Expression<Func<T1, bool>>)BuildCondition(parameterExpression, comparer, value, property.Split('.'));
        }
        private static Expression BuildCondition(Expression parameter, PredicateExpressionType comparer, object value, params string[] properties)
        {
            Expression parameterExpression;
            Type childType = null;
            if (properties.Length > 1)
            {
                parameter = Expression.Property(parameter, properties[0]);
                var isCollection = typeof(IEnumerable).IsAssignableFrom(parameter.Type);
                Expression childParameter;
                if (isCollection)
                {
                    childType = parameter.Type.GetGenericArguments()[0];
                    childParameter = Expression.Parameter(childType, childType.Name);
                }
                else
                {
                    childParameter = parameter;
                }
                var innerProperties = properties.Skip(1).ToArray();
                var navigationPropertyPredicate = BuildCondition(childParameter, comparer, value, innerProperties);
                if (isCollection)
                {
                    var anyMethod = typeof(Enumerable).GetMethods().Single(m => m.Name == "Any" && m.GetParameters().Length == 2);
                    anyMethod = anyMethod.MakeGenericMethod(childType);
                    navigationPropertyPredicate = Expression.Call(anyMethod, parameter, navigationPropertyPredicate);
                    parameterExpression = MakeLambda(parameter, navigationPropertyPredicate);
                }
                else
                {
                    parameterExpression = navigationPropertyPredicate;
                }
            }
            else
            {
                parameterExpression = Expression.Property(parameter, properties[0]);
                var valueExpression = Expression.Constant(value);
                var expression = BuildExpression(parameterExpression, comparer, valueExpression);
                parameterExpression = MakeLambda(parameter, expression);
            }
            return parameterExpression;
        }
        private static Expression BuildExpression(Expression parameterExpression, PredicateExpressionType comparer,
           Expression valueExpression)
        {
            if ((comparer == PredicateExpressionType.StartsWith || comparer == PredicateExpressionType.EndsWith) &&
                parameterExpression.Type != typeof(string))
            {
                comparer = PredicateExpressionType.Equals;
            }
            switch (comparer)
            {
                case PredicateExpressionType.Contains
                   when valueExpression.Type == typeof(List<int>) || valueExpression.Type == typeof(List<int?>):
                    return BuildIntContainsCondition(parameterExpression, comparer, valueExpression);
                case PredicateExpressionType.Contains
                   when valueExpression.Type == typeof(string):
                    return BuildStringContainsCondition(parameterExpression, comparer, valueExpression);
                case PredicateExpressionType.StartsWith when valueExpression.Type == typeof(string):
                    return BuildStringCondition(parameterExpression, comparer, valueExpression);
                case PredicateExpressionType.EndsWith when valueExpression.Type == typeof(string):
                    return BuildStringCondition(parameterExpression, comparer, valueExpression);
                case PredicateExpressionType.Equals:
                    break;
                case PredicateExpressionType.GreaterThan:
                    break;
                case PredicateExpressionType.GreaterThanOrEqual:
                    break;
                case PredicateExpressionType.LessThan:
                    break;
                case PredicateExpressionType.LessThanOrEqual:
                    break;
                case PredicateExpressionType.NotEqual:
                    break;
                default:
                    return Expression.MakeBinary((ExpressionType)comparer, parameterExpression, Expression.Convert(valueExpression, parameterExpression.Type));
            }
            return Expression.MakeBinary((ExpressionType)comparer, parameterExpression, Expression.Convert(valueExpression, parameterExpression.Type));
        }
        private static Expression BuildStringCondition(Expression parameterExpression, PredicateExpressionType comparer, Expression valueExpression)
        {
            var compare = typeof(string).GetMethods().Single(m =>
                m.Name.Equals(Enum.GetName(typeof(PredicateExpressionType), comparer)) &&
                m.GetParameters().Length == 1);
            return Expression.Call(parameterExpression, compare, valueExpression);
        }
        private static Expression BuildIntContainsCondition(Expression parameterExpression,
           PredicateExpressionType comparer, Expression valueExpression)
        {
            var contains = valueExpression.Type.GetMethod(Enum.GetName(typeof(PredicateExpressionType), comparer));
            if (contains == null)
            {
                throw new NullReferenceException();
            }
            //Backwards left/right for int contains
            return Expression.Call(valueExpression, contains, parameterExpression);
        }
        private static Expression BuildStringContainsCondition(Expression parameterExpression, PredicateExpressionType comparer, Expression valueExpression)
        {
            var contains = valueExpression.Type.GetMethod(Enum.GetName(typeof(PredicateExpressionType), comparer), new [] {typeof(string)});
            return Expression.Call(parameterExpression, contains, valueExpression, Expression.Constant(StringComparison.CurrentCultureIgnoreCase));
        }
        private static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter;
            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }
        private static bool ObjectHasValue(object value)
        {
            switch (value)
            {
                case List<int?> listNullableIntValue:
                    return listNullableIntValue.Any();
                case List<int> listIntValue:
                    return listIntValue.Any();
                case string stringValue:
                    return !string.IsNullOrEmpty(stringValue);
                case int intValue:
                    return intValue >= 0;
                case double doubleValue:
                    return doubleValue >= 0;
                case decimal decimalValue:
                    return decimalValue >= 0m;
                case bool booleanvalue:
                    return booleanvalue;
                case DateTime datetimeValue:
                    return true;
                case null:
                    return false;
                default:
                    return false;
            }
        }
        private static IEnumerable<int?> ConvertToNullableCollection(object value)
        {
            return value is IEnumerable<int> response ? response.ToNullable() : new List<int?>();
        }
        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }
    }
}
