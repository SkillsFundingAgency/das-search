using System.Collections.Generic;
using System.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Settings;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class ApprenticeshipSearchHandler : IRequestHandler<ApprenticeshipSearchQuery, ApprenticeshipSearchResponse>
    {
        private readonly IApprenticeshipSearchService _searchService;
        private readonly IPaginationSettings _paginationSettings;

        public ApprenticeshipSearchHandler(IApprenticeshipSearchService searchService, IPaginationSettings paginationSettings)
        {
            _searchService = searchService;
            _paginationSettings = paginationSettings;
        }

        public ApprenticeshipSearchResponse Handle(ApprenticeshipSearchQuery message)
        {
            var response = new ApprenticeshipSearchResponse
            {
                SortOrder = message.Order == 0 ? "1" : message.Order.ToString(),
                SearchTerm = message.Keywords
            };

            message.Page = message.Page <= 0 ? 1 : message.Page;

            var searchResults = new ApprenticeshipSearchResults
            {
                Results = new List<ApprenticeshipSearchResultsItem> { new ApprenticeshipSearchResultsItem { Title = "TEST", FrameworkId = "123", FrameworkName = "NAME", StandardId = "standardid"} },
                ResultsToTake = 1,
                LastPage = 1,
                ActualPage = 1,
                LevelAggregation = new Dictionary<int, long?>(),
                SelectedLevels = new List<int> {1}
            };// _searchService.SearchByKeyword(message.Keywords, message.Page, _paginationSettings.DefaultResultsAmount, message.Order, message.SelectedLevels);

            response.ActualPage = message.Page;
            response.AggregationLevel = searchResults.LevelAggregation;
            response.SearchTerm = message.Keywords;
            response.Results = searchResults.Results;
            response.ResultsToTake = searchResults.ResultsToTake;
            response.SelectedLevels = searchResults.SelectedLevels?.ToList();
            response.TotalResults = searchResults.TotalResults;
            response.LastPage = searchResults.LastPage;

            if (searchResults.TotalResults > 0 && !searchResults.Results.Any())
            {
                response.StatusCode = ApprenticeshipSearchResponse.ResponseCodes.PageNumberOutOfUpperBound;
                return response;
            }

            response.StatusCode = ApprenticeshipSearchResponse.ResponseCodes.Success;

            return response;
        }
    }
}