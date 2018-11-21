using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PredicateGenerator.Extensions
{
    public static class PredicateExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var expr2Body = new ParameterVisitor(expr2.Parameters, expr1.Parameters).Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, expr2Body), expr1.Parameters);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var expr2Body = new ParameterVisitor(expr2.Parameters, expr1.Parameters).Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, expr2Body), expr1.Parameters);
        }
        private class ParameterVisitor : ExpressionVisitor
        {
            private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements
            {
                get;
                set;
            }
            public ParameterVisitor(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
            {
                ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
                for (var i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                {
                    ParameterReplacements.Add(fromParameters[i], toParameters[i]);
                }
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (ParameterReplacements.TryGetValue(node, out var replacement))
                {
                    node = replacement;
                }
                return base.VisitParameter(node);
            }
        }
    }
}
