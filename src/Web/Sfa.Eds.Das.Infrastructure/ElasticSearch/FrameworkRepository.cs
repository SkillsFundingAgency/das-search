using Sfa.Eds.Das.Core.Configuration;
using Sfa.Eds.Das.Infrastructure.Mapping;

namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System;
    using System.Linq;

    using Core.Domain.Model;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Services;
    using Sfa.Eds.Das.Core.Logging;

    public sealed class FrameworkRepository : IGetFrameworks
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IFrameworkMapping _frameworkMapping;

        public FrameworkRepository(
            IElasticsearchClientFactory elasticsearchClientFactory,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IFrameworkMapping frameworkMapping)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _frameworkMapping = frameworkMapping;
        }

        public Framework GetFrameworkById(int id)
        {
            var client = this._elasticsearchClientFactory.Create();
            var results =
                client.Search<FrameworkSearchResultsItem>(s => s
                    .Index(_applicationSettings.ApprenticeshipIndexAlias)
                    .Types("frameworkdocument")
                    .From(0)
                    .Size(1)
                    .Query(q =>
                        q.QueryString(qs => qs
                        .OnFields(e => e.FrameworkId)
                            .Query(id.ToString()))));

            if (results.ConnectionStatus.HttpStatusCode != 200)
            {
                _applicationLogger.Error($"Trying to get provider with id {id}");

                throw new ApplicationException($"Failed query provider with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            return document != null ? _frameworkMapping.MapToFramework(document) : null;
        }
    }
}
