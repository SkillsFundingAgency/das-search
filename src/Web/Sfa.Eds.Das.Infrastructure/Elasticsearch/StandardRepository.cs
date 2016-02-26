namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System;
    using System.Linq;

    using Core.Domain.Model;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Services;
    using Sfa.Eds.Das.Core.Logging;

    public sealed class StandardRepository : IStandardRepository
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILog _applicationLogger;

        public StandardRepository(IElasticsearchClientFactory elasticsearchClientFactory, ILog applicationLogger)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationLogger = applicationLogger;
        }

        public Standard GetById(string id)
        {
            var client = this._elasticsearchClientFactory.Create();
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
                _applicationLogger.Error($"Trying to get standard with id {id}");

                throw new ApplicationException($"Failed query standard with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            if (document != null)
            {
                return new Standard
                           {
                               StandardId = document.StandardId,
                               Title = document.Title,
                               NotionalEndLevel = document.NotionalEndLevel,
                               JobRoles = document.JobRoles,
                               Keywords = document.Keywords,
                               TypicalLength = document.TypicalLength
                           };
            }

            return null;
        }
    }
}
