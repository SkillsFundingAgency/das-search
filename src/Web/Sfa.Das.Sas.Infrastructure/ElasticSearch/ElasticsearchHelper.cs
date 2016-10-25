using System.Collections.Generic;
using System.Linq;
using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public class ElasticsearchHelper : IElasticsearchHelper
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;

        public ElasticsearchHelper(IElasticsearchCustomClient elasticsearchCustomClient)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
        }

        public List<T> GetAllDocumentsFromIndex<T>(string indexAlias, string type)
            where T : class
        {
            var firstScan = _elasticsearchCustomClient.Search<T>(
                s => s
                    .Index(indexAlias)
                           .Type(Types.Parse(type))
                           .From(0)
                           .Size(2000)
                           .MatchAll()
                           .Scroll("5m")
            );

            var documents = firstScan.Documents.ToList();

            var scrollResults = _elasticsearchCustomClient.Scroll<T>("10m", firstScan.ScrollId);
            while (scrollResults.Documents.Any())
            {
                documents.AddRange(scrollResults.Documents);

                scrollResults = _elasticsearchCustomClient.Scroll<T>("10m", scrollResults.ScrollId);
            }

            return documents;
        }
    }
}
