using Sfa.Eds.Das.Core.Configuration;
using Sfa.Eds.Das.Infrastructure.Mapping;

namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System;
    using System.Linq;

    using Core.Domain.Model;
    using Nest;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Services;
    using Sfa.Eds.Das.Core.Logging;

    public sealed class StandardRepository : IGetStandards
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IStandardMapping _standardMapping;

        public StandardRepository(
            IElasticsearchClientFactory elasticsearchClientFactory,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IStandardMapping standardMapping)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _standardMapping = standardMapping;
        }

        public Standard GetStandardById(int id)
        {
            var client = this._elasticsearchClientFactory.Create();
            var results =
                client.Search<StandardSearchResultsItem>(s => s
                    .Index(_applicationSettings.ApprenticeshipIndexAlias)
                    .Type(Types.Parse("standarddocument"))
                    .From(0)
                    .Size(1)
                    .Query(q =>
                        q.QueryString(qs => qs
                        .Fields(fs => fs
                            .Field(e => e.StandardId))
                        .Query(id.ToString()))));

            if (results.ApiCall.HttpStatusCode != 200)
            {
                _applicationLogger.Error($"Trying to get standard with id {id}");

                throw new ApplicationException($"Failed query standard with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            return document != null ? _standardMapping.MapToStandard(document) : null;
        }
    }
}
