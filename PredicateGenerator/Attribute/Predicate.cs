using System;
using System.Collections.Generic;
using System.Text;
using PredicateGenerator.Enums;

namespace PredicateGenerator.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class Predicate : System.Attribute
    {
        public Predicate(PredicateExpressionType expressionType, string expressionProperty,
            PredicateConnectorType predicateConnectorType = PredicateConnectorType.And)
        {
            ExpressionType = expressionType;
            ExpressionProperty = expressionProperty;
            PredicateConnectorType = predicateConnectorType;
        }
        public PredicateExpressionType ExpressionType { get; set; }
        public string ExpressionProperty { get; set; }
        public PredicateConnectorType PredicateConnectorType { get; set; }
    }
}
