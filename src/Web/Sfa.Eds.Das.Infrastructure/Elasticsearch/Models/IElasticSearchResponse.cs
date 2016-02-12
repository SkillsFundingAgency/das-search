namespace Sfa.Eds.Das.Infrastructure.ElasticSearch.Models
{
    using System.Collections.Generic;

    public interface IElasticsearchResponse<T>
    {
        long Total { get; set; }
        IEnumerable<T> Documents { get; set; }
        int? HttpStatusCode { get; set; }
    }
}