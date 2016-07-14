using System.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Settings;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class ApprenticeshipSearchHandler : IRequestHandler<ApprenticeshipSearchQuery, ApprenticeshipSearchResponse>
    {
        private readonly IApprenticeshipSearchService _searchService;
        private readonly IShortlistCollection<int> _shortlistCollection;

        public ApprenticeshipSearchHandler(IApprenticeshipSearchService searchService, IShortlistCollection<int> shortlistCollection)
        {
            _searchService = searchService;
            _shortlistCollection = shortlistCollection;
        }

        public ApprenticeshipSearchResponse Handle(ApprenticeshipSearchQuery message)
        {
            var response = new ApprenticeshipSearchResponse();

            message.Page = message.Page <= 0 ? 1 : message.Page;

            var searchResults = _searchService.SearchByKeyword(message.Keywords, message.Page, message.Take, message.Order, message.SelectedLevels);

            if (searchResults.TotalResults > 0 && !searchResults.Results.Any())
            {
                response.LastPage = searchResults.LastPage;
                response.StatusCode = ApprenticeshipSearchResponse.ResponseCodes.SearchPageLimitExceeded;

                return response;
            }

            var shotListedStandardsCollection = _shortlistCollection.GetAllItems(Constants.StandardsShortListName).Select(standard => standard.ApprenticeshipId).ToList();
            var shotListedFrameworksCollection = _shortlistCollection.GetAllItems(Constants.FrameworksShortListName).Select(framework => framework.ApprenticeshipId).ToList();

            response.ShortlistedStandards = shotListedStandardsCollection.ToDictionary(standard => standard, standard => true);
            response.ShortlistedFrameworks = shotListedFrameworksCollection.ToDictionary(framework => framework, framework => true);

            response.SortOrder = message.Order == 0 ? "1" : message.Order.ToString();
            response.ActualPage = message.Page;

            response.StatusCode = ApprenticeshipSearchResponse.ResponseCodes.Success;

            return response;
        }
    }
}
