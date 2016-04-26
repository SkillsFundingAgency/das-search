using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch.Models
{
    public interface IElasticsearchResponse<T>
    {
        long Total { get; set; }
        IEnumerable<T> Documents { get; set; }
        int? HttpStatusCode { get; set; }
    }
}