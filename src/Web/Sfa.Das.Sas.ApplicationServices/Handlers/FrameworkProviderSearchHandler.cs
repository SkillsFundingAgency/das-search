using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public sealed class FrameworkProviderSearchHandler : IAsyncRequestHandler<FrameworkProviderSearchQuery, FrameworkProviderSearchResponse>
    {
        private readonly ILog _logger;
        private readonly IProviderSearchService _searchService;
        private readonly IShortlistCollection<int> _shortlist;
        private readonly IPaginationSettings _paginationSettings;
        private readonly AbstractValidator<ProviderSearchQuery> _validator;

        public FrameworkProviderSearchHandler(
            AbstractValidator<ProviderSearchQuery> validator,
            IProviderSearchService searchService,
            IShortlistCollection<int> shortlist,
            IPaginationSettings paginationSettings,
            ILog logger)
        {
            _validator = validator;
            _searchService = searchService;
            _shortlist = shortlist;
            _paginationSettings = paginationSettings;
            _logger = logger;
        }

        public async Task<FrameworkProviderSearchResponse> Handle(FrameworkProviderSearchQuery message)
        {
            var result = _validator.Validate(message);

            if (!result.IsValid)
            {
                var response = new FrameworkProviderSearchResponse { Success = false };

                if (result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidId))
                {
                    response.StatusCode = FrameworkProviderSearchResponse.ResponseCodes.InvalidApprenticeshipId;
                }

                if (result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidPostcode))
                {
                    response.StatusCode = FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat;
                }

                return response;
            }

            message.Page = message.Page <= 0 ? 1 : message.Page;

            return await PerformSearch(message);
        }

        private async Task<FrameworkProviderSearchResponse> PerformSearch(FrameworkProviderSearchQuery message)
        {
            var pageNumber = message.Page <= 0 ? 1 : message.Page;

            var searchResults = await _searchService.SearchFrameworkProviders(
                message.ApprenticeshipId,
                message.PostCode,
                new Pagination {Page = pageNumber, Take = message.Take},
                message.DeliveryModes,
                message.ShowAll);

            if (searchResults.TotalResults > 0 && !searchResults.Hits.Any())
            {
                var take = message.Take <= 0 ? _paginationSettings.DefaultResultsAmount : 1;
                var lastPage = take > 0 ? (int) System.Math.Ceiling((double) searchResults.TotalResults/take) : 1;
                return new FrameworkProviderSearchResponse {StatusCode = FrameworkProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound, CurrentPage = lastPage};
            }

            var shortlistItems = _shortlist.GetAllItems(Constants.FrameworksShortListName)
                ?.SingleOrDefault(x => x.ApprenticeshipId.Equals(message.ApprenticeshipId));

            return new FrameworkProviderSearchResponse
            {
                Success = !(searchResults.HasError || searchResults.FrameworkIsMissing),
                CurrentPage = pageNumber,
                Results = searchResults,
                Shortlist = shortlistItems,
                TotalResultsForCountry = await GetCountResultForCountry(searchResults, message),
                SearchTerms = message.Keywords,
                ShowAllProviders = message.ShowAll,
                StatusCode = searchResults.FrameworkIsMissing ? FrameworkProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound : default(FrameworkProviderSearchResponse.ResponseCodes)
            };
        }

        private async Task<long> GetCountResultForCountry(BaseProviderSearchResults searchResults, ProviderSearchQuery message)
        {
            long totalRestultsForCountry = 0;
            if (searchResults.TotalResults > 0)
            {
                return totalRestultsForCountry;
            }

            var totalProvidersCountry = await _searchService.SearchFrameworkProviders(
                message.ApprenticeshipId,
                message.PostCode,
                new Pagination(),
                message.DeliveryModes,
                true);

            totalRestultsForCountry = totalProvidersCountry.TotalResults;

            return totalRestultsForCountry;
        }
    }
}