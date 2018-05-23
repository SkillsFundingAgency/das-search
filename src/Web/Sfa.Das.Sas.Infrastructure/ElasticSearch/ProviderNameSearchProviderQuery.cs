using System;
using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public class ProviderNameSearchProviderQuery : IProviderNameSearchProviderQuery
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IConfigurationSettings _applicationSettings;

        public ProviderNameSearchProviderQuery(IElasticsearchCustomClient elasticsearchCustomClient, IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationSettings = applicationSettings;
        }

        public ISearchResponse<ProviderNameSearchResult> GetResults(string term, int take, PaginationOrientationDetails paginationDetails)
        {
            return _elasticsearchCustomClient.Search<ProviderNameSearchResult>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .Type("providerapidocument")
                .Skip(paginationDetails.Skip)
                .Take(take)
                .Query(q => q
                    .Bool(b => b
                        .Filter(IsNotEmployerProvider())
                        .Must(mu => mu
                            .QueryString(qs => qs
                                .Fields(fs => fs
                                    .Field(f => f.ProviderName)
                                    .Field(f => f.Aliases))
                                .Query(term)))
            )));
        }

        public long GetTotalMatches(string term)
        {
            var initialDetails = _elasticsearchCustomClient.Search<ProviderNameSearchResult>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .Type("providerapidocument")
                .Query(q => q
                    .Bool(b => b
                        .Filter(IsNotEmployerProvider())
                        .Must(mu => mu
                            .QueryString(qs => qs
                                .Fields(fs => fs
                                    .Field(f => f.ProviderName)
                                    .Field(f => f.Aliases))
                                .Query(term)))
            )));

            return initialDetails.HitsMetaData.Total;
        }

        private static Func<QueryContainerDescriptor<ProviderNameSearchResult>, QueryContainer> IsNotEmployerProvider()
        {
            return f => f
                .Term(t => t
                    .Field(fi => fi.IsEmployerProvider).Value(false));
        }
    }
}