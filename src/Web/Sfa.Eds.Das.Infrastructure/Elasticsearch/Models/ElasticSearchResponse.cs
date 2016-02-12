namespace Sfa.Eds.Das.Infrastructure.ElasticSearch.Models
{
    using System.Collections.Generic;

    public class ElasticsearchResponse<T> : IElasticsearchResponse<T>
    {
        public long Total { get; set; }

        public IEnumerable<T> Documents { get; set; }

        public int? HttpStatusCode { get; set; }
    }
}