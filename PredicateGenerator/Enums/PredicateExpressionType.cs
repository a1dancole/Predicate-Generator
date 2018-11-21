using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PredicateGenerator.Enums
{
    public enum PredicateExpressionType
    {
        Contains,
        StartsWith,
        EndsWith,
        Equals = ExpressionType.Equal,
        GreaterThan = ExpressionType.GreaterThan,
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        LessThan = ExpressionType.LessThan,
        LessThanOrEqual = ExpressionType.LessThanOrEqual,
        NotEqual = ExpressionType.NotEqual
    }
}
