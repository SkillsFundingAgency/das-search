using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using System.Threading.Tasks;
    using ApplicationServices.Interfaces;
    using ApplicationServices.Models;
    using ApplicationServices.Responses;
    using Core.Configuration;
    using Core.Domain.Model;
    using SFA.DAS.NLog.Logger;

    public class ProviderNameSearchProvider : IProviderNameSearchProvider
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IProviderNameSearchMapping _nameSearchMapping;

        public ProviderNameSearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings, IProviderNameSearchMapping nameSearchMapping)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
            _nameSearchMapping = nameSearchMapping;
        }

        public async Task<ProviderNameSearchResults> SearchByTerm(string searchTerm, int page, int take)
        {

            var formattedSearchTerm = QueryHelper.FormatQueryReturningEmptyStringIfEmptyOrNull(searchTerm).Trim();

            if (formattedSearchTerm.Length < 3)
            {
                return new ProviderNameSearchResults
                {
                    ActualPage = 1,
                    LastPage = 1,
                    HasError = false,
                    SearchTerm = formattedSearchTerm,
                    ResponseCode = ProviderNameSearchResponseCodes.SearchTermTooShort
                };
            }

            var term = $"*{formattedSearchTerm}*";
            var totalHits = GetTotalMatches(term);
            var paginationDetails = GeneratePaginationDetails(page, take, totalHits);

            var returnedResults = _elasticsearchCustomClient.Search<ProviderNameSearchResult>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .Type("providerapidocument")
                .Skip(paginationDetails.Skip)
                .Take(take)
                .Query(q => q
                    .QueryString(qs => qs
                        .Fields(fs => fs
                            .Field(f => f.ProviderName)
                            .Field(f => f.Aliases))
                        .Query(term)))
            );

            var results = new ProviderNameSearchResults
            {
                ActualPage = paginationDetails.CurrentPage,
                HasError = false,
                SearchTerm = formattedSearchTerm,
                Results = _nameSearchMapping.FilterNonMatchingAliases(formattedSearchTerm,returnedResults.Documents),
                LastPage = paginationDetails.LastPage,
                TotalResults = totalHits,
                ResultsToTake = returnedResults.Documents.Count,
                ResponseCode = returnedResults.Documents.Count > 0 ? ProviderNameSearchResponseCodes.Success : ProviderNameSearchResponseCodes.NoSearchResultsFound
            };

            return results;
        }

        private long GetTotalMatches(string term)
        {
            var initialDetails = _elasticsearchCustomClient.Search<ProviderNameSearchResult>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .Type("providerapidocument")
                .Query(q => q
                    .QueryString(qs => qs
                        .Fields(fs => fs
                            .Field(f => f.ProviderName)
                            .Field(f => f.Aliases))
                        .Query(term)))
            );

            return initialDetails.HitsMetaData.Total;
        }

        private LocalPaginationDetails GeneratePaginationDetails(int page, int take,  long totalHits)
        {
            var res = default(LocalPaginationDetails);

            var skip = (page - 1) * take;

            while (skip >= totalHits)
            {
                page = page - 1;
                skip = skip - take;
            }

            var lastPage = take > 0 ? (int)System.Math.Ceiling((double)totalHits / take) : 1;

            res.LastPage = lastPage;
            res.CurrentPage = page;
            res.Skip = skip;

            return res;
        }
    }
}