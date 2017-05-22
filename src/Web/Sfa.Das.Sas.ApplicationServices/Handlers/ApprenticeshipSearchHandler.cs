using System.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class ApprenticeshipSearchHandler : IRequestHandler<ApprenticeshipSearchQuery, ApprenticeshipSearchResponse>
    {
        private readonly IApprenticeshipSearchService _searchService;

        public ApprenticeshipSearchHandler(IApprenticeshipSearchService searchService)
        {
            _searchService = searchService;
        }

        public ApprenticeshipSearchResponse Handle(ApprenticeshipSearchQuery message)
        {
            var response = new ApprenticeshipSearchResponse
            {
                SortOrder = message.Order == 0 ? "1" : message.Order.ToString(),
                SearchTerm = message.Keywords
            };

            message.Page = message.Page <= 0 ? 1 : message.Page;

            var searchResults = _searchService.SearchByKeyword(message.Keywords, message.Page, 10, message.Order, message.SelectedLevels);

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