using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Elasticsearch
{
    public class ElasticMapper
    {
        private readonly Dictionary<int, int> _dictionary;
        public ElasticMapper()
        {
            _dictionary = new Dictionary<int, int>
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
        }

        public int MapLevel(int level)
        {
            return _dictionary.ContainsKey(level) ? _dictionary[level] : 0;
        }
    }
}
