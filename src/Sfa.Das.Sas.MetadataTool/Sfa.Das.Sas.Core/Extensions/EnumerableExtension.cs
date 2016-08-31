namespace Sfa.Das.Sas.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtension
    {
        public static IEnumerable<T> WhereSafe<T>(this IEnumerable<T> self, Func<T, bool> func)
        {
            return self?.Where(func) ?? new List<T>();
        }
    }
}