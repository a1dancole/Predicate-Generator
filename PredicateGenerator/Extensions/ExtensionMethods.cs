using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PredicateGenerator.Extensions
{
    public static class ExtensionMethods
    {
        public static IEnumerable<int?> ToNullable(this IEnumerable<int> collection)
        {
            return collection.Select(o => (int?)o);
        }
    }
}
