using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    public sealed class ProviderElasticRepository : IGetProviders
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;

        public ProviderElasticRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
        }

        public long GetProvidersAmount()
        {
            var results =
                   _elasticsearchCustomClient.Search<ProviderSearchResultItem>(
                       s =>
                       s.Index(_applicationSettings.ProviderIndexAlias)
                           .Type(Types.Parse("providerapidocument"))
                           .From(0)
                           .MatchAll());
            return results.HitsMetaData.Total;
        }

        public ProviderSearchResult GetProvidersFromSearch(int page, int take, string search)
        {
            // MFCMFC still working this out
            var results = _elasticsearchCustomClient.Search<ProviderSearchItem>(
                s =>
                s.Index( _applicationSettings.ProviderIndexAlias)
                .Type(Types.Parse("providerapidocument"))
                .From(0)
                .Query(q => q
                .Term(p => p.ProviderName, search)
                ));

            return new ProviderSearchResult();
        }
    }
}
