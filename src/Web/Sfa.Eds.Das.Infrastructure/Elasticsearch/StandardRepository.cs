namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System;
    using System.Linq;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Interfaces.Search;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Core.Models;

    public class StandardRepository : IStandardRepository
    {
        private readonly IElasticsearchClientFactory elasticsearchClientFactory ;

        private readonly IApplicationLogger applicationLogger;

        public StandardRepository(IElasticsearchClientFactory elasticsearchClientFactory, IApplicationLogger applicationLogger)
        {
            this.elasticsearchClientFactory = elasticsearchClientFactory;
            this.applicationLogger = applicationLogger;
        }

        public Standard GetById(string id)
        {

            var client = this.elasticsearchClientFactory.Create();
            var results =
                client.Search<StandardSearchResultsItem>(
                    s => s
                    .From(0)
                    .Size(1)
                    .Query(q =>
                        q.QueryString(qs =>
                            qs.OnFields(e => e.StandardId)
                            .Query(id))));

            if (results.ConnectionStatus.HttpStatusCode != 200)
            {
                applicationLogger.Error($"Trying to get standard with id {id}");
                throw new ApplicationException($"Failed query standard with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            if (document != null)
            {
                return new Standard { StandardId = document.StandardId, Title = document.Title, NotionalEndLevel = document.NotionalEndLevel };
            }
            return null;
        }
    }
}
