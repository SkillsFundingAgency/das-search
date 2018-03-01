using Nest;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using System.Threading.Tasks;
    using ApplicationServices.Interfaces;
    using ApplicationServices.Models;
    using ApplicationServices.Responses;
    using SFA.DAS.NLog.Logger;


    public class ProviderNameSearchProvider : IProviderNameSearchProvider
    {
         private readonly ILog _logger;
         private readonly IProviderNameSearchMapping _nameSearchMapping;
        private readonly IPaginationOrientationService _paginationOrientationService;
        private readonly IProviderNameSearchProviderQuery _providerNameSearchProviderQuery;

        public ProviderNameSearchProvider(ILog logger, IProviderNameSearchMapping nameSearchMapping, IPaginationOrientationService paginationOrientationService, IProviderNameSearchProviderQuery providerNameSearchProviderQuery)
        {

            _logger = logger;
            _nameSearchMapping = nameSearchMapping;
            _paginationOrientationService = paginationOrientationService;
            _providerNameSearchProviderQuery = providerNameSearchProviderQuery;
        }

        public async Task<ProviderNameSearchResultsAndPagination> SearchByTerm(string searchTerm, int page, int take)
        {
            var formattedSearchTerm = QueryHelper.FormatQueryReturningEmptyStringIfEmptyOrNull(searchTerm).Trim();

            _logger.Info(
                $"Provider Name Search provider formatting query: SearchTerm: [{searchTerm}] formatted to: [{formattedSearchTerm}]");

            if (formattedSearchTerm.Length < 3)
            {
                _logger.Info(
                    $"Formatted search term causing SearchTermTooShort: [{formattedSearchTerm}]");

                return MapProviderNameSearchResultsAndPaginationTooShortDetails(formattedSearchTerm);
            }

            var term = $"*{formattedSearchTerm}*";
            _logger.Info($"Provider Name Search provider getting total hits");

            var totalHits = _providerNameSearchProviderQuery.GetTotalMatches(term);
            _logger.Info($"Provider Name Search provider total hits retrieved: [{totalHits}]");

            var paginationDetails = _paginationOrientationService.GeneratePaginationDetails(page, take, totalHits);

            _logger.Debug($"Provider Name Search provider getting results based on pagination details: take: [{take}] skip:[{paginationDetails.Skip}], current page [{paginationDetails.CurrentPage}], last page [{paginationDetails.LastPage}] ");

            var returnedResults = _providerNameSearchProviderQuery.GetResults(term, take, paginationDetails);

            _logger.Debug($"Provider Name Search provider retrieved results, mapping to returned format");

            var results = MapResultsAndPaginationDetails(paginationDetails, formattedSearchTerm, returnedResults, totalHits);

            _logger.Debug($"Provider Name Search provider retrieved results mapped to returned format");

            return results;
        }

        private static ProviderNameSearchResultsAndPagination MapProviderNameSearchResultsAndPaginationTooShortDetails(string formattedSearchTerm)
        {
            return new ProviderNameSearchResultsAndPagination
            {
                ActualPage = 1,
                LastPage = 1,
                HasError = false,
                SearchTerm = formattedSearchTerm,
                ResponseCode = ProviderNameSearchResponseCodes.SearchTermTooShort
            };
        }

        private ProviderNameSearchResultsAndPagination MapResultsAndPaginationDetails(PaginationOrientationDetails paginationDetails, string formattedSearchTerm, ISearchResponse<ProviderNameSearchResult> returnedResults, long totalHits)
        {
            return new ProviderNameSearchResultsAndPagination
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
        }
    }
}