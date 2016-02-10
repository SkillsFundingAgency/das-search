using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Eds.Indexer.Indexer.Infrastructure.Helpers
{
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