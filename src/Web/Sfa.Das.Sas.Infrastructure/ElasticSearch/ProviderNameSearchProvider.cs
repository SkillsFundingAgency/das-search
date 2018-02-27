using Nest;
using Sfa.Das.Sas.ApplicationServices.Services;
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
        private readonly IPaginationOrientationService _paginationOrientationService;

        public ProviderNameSearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings, IProviderNameSearchMapping nameSearchMapping, IPaginationOrientationService paginationOrientationService)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
            _nameSearchMapping = nameSearchMapping;
            _paginationOrientationService = paginationOrientationService;
        }

        public async Task<ProviderNameSearchResultsAndPagination> SearchByTerm(string searchTerm, int page, int take)
        {
            var formattedSearchTerm = QueryHelper.FormatQueryReturningEmptyStringIfEmptyOrNull(searchTerm).Trim();

            _logger.Info(
                $"Provider Name Search provider formatting query: SearchTerm: [{searchTerm}] formatted to: [{formattedSearchTerm}]");

            if (formattedSearchTerm.Length < 3)
            {
                _logger.Info(
                    $"Fromatted search term causing SearchTermTooShort: [{formattedSearchTerm}]");

                return new ProviderNameSearchResultsAndPagination
                {
                    ActualPage = 1,
                    LastPage = 1,
                    HasError = false,
                    SearchTerm = formattedSearchTerm,
                    ResponseCode = ProviderNameSearchResponseCodes.SearchTermTooShort
                };
            }

            var term = $"*{formattedSearchTerm}*";
            _logger.Info($"Provider Name Search provider getting total hits");

            var totalHits = GetTotalMatches(term);
            _logger.Info($"Provider Name Search provider total hits retrieved: [{totalHits}]");
            var paginationDetails = _paginationOrientationService.GeneratePaginationDetails(page, take, totalHits);

            _logger.Debug($"Provider Name Search provider getting results based on pagination details: take: [{take}] skip:[{paginationDetails.Skip}], current page [{paginationDetails.CurrentPage}], last page [{paginationDetails.LastPage}] ");

            var returnedResults = GetResults(term, take, paginationDetails);

            _logger.Debug($"Provider Name Search provider retrieved results, mapping to returned format");

            var results = new ProviderNameSearchResultsAndPagination
            {
                ActualPage = paginationDetails.CurrentPage,
                HasError = false,
                SearchTerm = formattedSearchTerm,
                Results = _nameSearchMapping.FilterNonMatchingAliases(formattedSearchTerm, returnedResults.Documents),
                LastPage = paginationDetails.LastPage,
                TotalResults = totalHits,
                ResultsToTake = returnedResults.Documents.Count,
                ResponseCode = returnedResults.Documents.Count > 0 ? ProviderNameSearchResponseCodes.Success : ProviderNameSearchResponseCodes.NoSearchResultsFound
            };

            _logger.Debug($"Provider Name Search provider retrieved results mapped to returned format");

            return results;
        }

        private ISearchResponse<ProviderNameSearchResult> GetResults(string term, int take, PaginationOrientationDetails paginationDetails)
        {
            return _elasticsearchCustomClient.Search<ProviderNameSearchResult>(s => s
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
    }
}