using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public sealed class StandardProviderSearchHandler : IAsyncRequestHandler<StandardProviderSearchQuery, StandardProviderSearchResponse>
    {
        private readonly ILog _logger;
        private readonly IProviderSearchService _searchService;
        private readonly IShortlistCollection<int> _shortlist;
        private readonly IPaginationSettings _paginationSettings;
        private readonly AbstractValidator<ProviderSearchQuery> _validator;

        public StandardProviderSearchHandler(
            ProviderSearchQueryValidator validator,
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

        public async Task<StandardProviderSearchResponse> Handle(StandardProviderSearchQuery message)
        {
            var result = _validator.Validate(message);
            StandardProviderSearchResponse response;

            if (!result.IsValid)
            {
                response = new StandardProviderSearchResponse { Success = false };

                if (result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidId))
                {
                    response.StatusCode = StandardProviderSearchResponse.ResponseCodes.InvalidApprenticeshipId;
                }

                if (result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidPostcode))
                {
                    response.StatusCode = StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat;
                }

                return response;
            }

            message.Page = message.Page <= 0 ? 1 : message.Page;

            return await PerformSearch(message);
        }

        private async Task<StandardProviderSearchResponse> PerformSearch(StandardProviderSearchQuery message)
        {
            var pageNumber = message.Page <= 0 ? 1 : message.Page;

            var searchResults = await _searchService.SearchStandardProviders(
                message.ApprenticeshipId,
                message.PostCode,
                new Pagination { Page = pageNumber, Take = message.Take },
                message.DeliveryModes,
                message.ShowAll);

            if (searchResults.TotalResults > 0 && !searchResults.Hits.Any())
            {
                var take = message.Take <= 0 ? _paginationSettings.DefaultResultsAmount : 1;
                var lastPage = take > 0 ? (int)System.Math.Ceiling((double)searchResults.TotalResults / take) : 1;
                return new StandardProviderSearchResponse() { StatusCode = StandardProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound, CurrentPage = lastPage };
            }

            var shortlistItems = _shortlist.GetAllItems(Constants.StandardsShortListName)
                ?.SingleOrDefault(x => x.ApprenticeshipId.Equals(message.ApprenticeshipId));

            return new StandardProviderSearchResponse
            {
                Success = !(searchResults.HasError || searchResults.StandardNotFound),
                CurrentPage = pageNumber,
                Results = searchResults,
                Shortlist = shortlistItems,
                TotalResultsForCountry = await GetCountResultForCountry(searchResults, message),
                SearchTerms = message.Keywords,
                ShowAllProviders = message.ShowAll,
                StatusCode = searchResults.StandardNotFound ? StandardProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound : default(StandardProviderSearchResponse.ResponseCodes)
            };
        }

        private async Task<long> GetCountResultForCountry(ProviderStandardSearchResults searchResults, StandardProviderSearchQuery message)
        {
            long totalRestultsForCountry = 0;
            if (searchResults.TotalResults <= 0)
            {
                var totalProvidersCountry = await _searchService.SearchStandardProviders(
                    message.ApprenticeshipId,
                    message.PostCode,
                    new Pagination(),
                    message.DeliveryModes,
                    true);

                totalRestultsForCountry = totalProvidersCountry.TotalResults;
            }

            return totalRestultsForCountry;
        }
    }
}