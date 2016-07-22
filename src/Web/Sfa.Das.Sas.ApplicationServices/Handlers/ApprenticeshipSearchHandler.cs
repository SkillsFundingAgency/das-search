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
        private readonly IShortlistCollection<int> _shortlistCollection;

        public ApprenticeshipSearchHandler(
            IApprenticeshipSearchService searchService,
            IShortlistCollection<int> shortlistCollection)
        {
            _searchService = searchService;
            _shortlistCollection = shortlistCollection;
        }

        public ApprenticeshipSearchResponse Handle(ApprenticeshipSearchQuery message)
        {
            var response = new ApprenticeshipSearchResponse
            {
                SortOrder = message.Order == 0 ? "1" : message.Order.ToString(),
                SearchTerm = message.Keywords
            };

            message.Page = message.Page <= 0 ? 1 : message.Page;

            var searchResults = _searchService.SearchByKeyword(message.Keywords, message.Page, message.Take, message.Order, message.SelectedLevels);

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

            var shotListedStandardsCollection = _shortlistCollection.GetAllItems(Constants.StandardsShortListName)?.Select(standard => standard.ApprenticeshipId).ToList();
            var shotListedFrameworksCollection = _shortlistCollection.GetAllItems(Constants.FrameworksShortListName)?.Select(framework => framework.ApprenticeshipId).ToList();

            response.ShortlistedStandards = shotListedStandardsCollection?.ToDictionary(standard => standard, standard => true);
            response.ShortlistedFrameworks = shotListedFrameworksCollection?.ToDictionary(framework => framework, framework => true);

            response.StatusCode = ApprenticeshipSearchResponse.ResponseCodes.Success;

            return response;
        }
    }
}