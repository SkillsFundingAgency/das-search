using System.Collections.Generic;

namespace Sfa.Infrastructure.Elasticsearch.Helpers
{
    public static class ElasticLevelHelper
    {
        private static Dictionary<int, int> Dictionary => new Dictionary<int, int>
            {
                { 2, 3 },
                { 3, 2 },
                { 4, 20 },
                { 5, 21 },
                { 6, 22 },
                { 7, 23 },
                { 20, 4 },
                { 21, 5 },
                { 22, 6 },
                { 23, 7 }
            };

        public static int MapLevel(int level)
        {
            return Dictionary.ContainsKey(level) ? Dictionary[level] : 0;
        }
    }
}
