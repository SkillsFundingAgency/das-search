namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    using ApplicationServices.Models;
    using Core.Configuration;
    using Core.Domain.Services;
    using Elasticsearch;
    using Nest;

    public sealed class ProviderElasticRepository : IGetProviders
    {
        private const string ProviderApiDocumentType = "providerapidocument";
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IConfigurationSettings _applicationSettings;
  
        public ProviderElasticRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationSettings = applicationSettings;
        }

        public long GetProvidersAmount()
        {
            var results =
                _elasticsearchCustomClient.Search<ProviderSearchResultItem>(
                    s =>
                        s.Index(_applicationSettings.ProviderIndexAlias)
                            .Type(Types.Parse(ProviderApiDocumentType))
                            .From(0)
                            .MatchAll());
            return results.HitsMetaData.Total;
        }
    }
}
