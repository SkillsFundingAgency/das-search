using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch.Models
{
    public sealed class ElasticsearchResponse<T> : IElasticsearchResponse<T>
    {
        public long Total { get; set; }

        public IEnumerable<T> Documents { get; set; }

        public int? HttpStatusCode { get; set; }
    }
}