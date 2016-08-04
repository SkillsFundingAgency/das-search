using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public sealed class SearchResult<T>
        where T : class
    {
        public long Total { get; set; }

        public IEnumerable<T> Hits { get; set; }

        public Dictionary<string, long?> TrainingOptionsAggregation { get; set; }

        public Dictionary<string, long?> NationalProvidersAggregation { get; set; }
    }
}
