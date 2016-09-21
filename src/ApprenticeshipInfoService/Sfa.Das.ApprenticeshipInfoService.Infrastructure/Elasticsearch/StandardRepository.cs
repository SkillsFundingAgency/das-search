namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Elasticsearch
{
    using System.Linq;
    using Core.Configuration;
    using Core.Models;
    using Core.Services;
    using Mapping;
    using Nest;

    public sealed class StandardRepository : IGetStandards
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IStandardMapping _standardMapping;

        public StandardRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            IConfigurationSettings applicationSettings,
            IStandardMapping standardMapping)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationSettings = applicationSettings;
            _standardMapping = standardMapping;
        }

        public Standard GetStandardById(int id)
        {
            var results = _elasticsearchCustomClient.Search<StandardSearchResultsItem>(
                s =>
                s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                .Type(Types.Parse("standarddocument"))
                .From(0)
                .Size(1)
                .Query(q => q
                    .Term(t => t
                        .Field(fi => fi.StandardId).Value(id))));

            var document = results.Documents.Any() ? results.Documents.First() : null;

            return document != null ? _standardMapping.MapToStandard(document) : null;
        }
    }
}
