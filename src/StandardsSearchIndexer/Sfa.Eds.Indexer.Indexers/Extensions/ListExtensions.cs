namespace Sfa.Eds.Das.Indexer.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ListExtensions
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> action)
        {
            foreach (var item in list)
            {
                await action(item).ConfigureAwait(false);
            }
        }
    }
}