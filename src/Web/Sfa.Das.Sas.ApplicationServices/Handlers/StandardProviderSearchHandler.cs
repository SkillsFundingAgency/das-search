namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Domain.Model;
    using Core.Logging;
    using FluentValidation;
    using MediatR;
    using Models;
    using Queries;
    using Responses;
    using Settings;
    using Validators;

    public sealed class StandardProviderSearchHandler : IAsyncRequestHandler<StandardProviderSearchQuery, StandardProviderSearchResponse>
    {
        private readonly ILog _logger;
        private readonly IProviderSearchService _searchService;
        private readonly IPaginationSettings _paginationSettings;
        private readonly AbstractValidator<ProviderSearchQuery> _validator;

        private readonly Dictionary<string, StandardProviderSearchResponse.ResponseCodes> _searchResponseCodes =
            new Dictionary<string, ProviderSearchResponseBase<ProviderStandardSearchResults>.ResponseCodes>
                {
                  { LocationLookupResponse.WrongPostcode, StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat },
                  { LocationLookupResponse.ServerError, StandardProviderSearchResponse.ResponseCodes.LocationServiceUnavailable },
                  { LocationLookupResponse.ApprenticeshipNotFound, StandardProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound },
                  { ServerLookupResponse.InternalServerError, StandardProviderSearchResponse.ResponseCodes.ServerError },
                  { LocationLookupResponse.Ok, StandardProviderSearchResponse.ResponseCodes.Success },
                  { string.Empty, StandardProviderSearchResponse.ResponseCodes.Success }
                };

        public StandardProviderSearchHandler(
            ProviderSearchQueryValidator validator,
            IProviderSearchService searchService,
            IPaginationSettings paginationSettings,
            ILog logger)
        {
            _validator = validator;
            _searchService = searchService;
            _paginationSettings = paginationSettings;
            _logger = logger;
        }

        public async Task<StandardProviderSearchResponse> Handle(StandardProviderSearchQuery message)
        {
            var result = _validator.Validate(message);

            if (!result.IsValid)
            {
                var response = new StandardProviderSearchResponse { Success = false };

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

        private async Task<StandardProviderSearchResponse> PerformSearch(ProviderSearchQuery message)
        {
            var pageNumber = message.Page <= 0 ? 1 : message.Page;

            var searchResults = await _searchService.SearchStandardProviders(
                message.ApprenticeshipId,
                message.PostCode,
                new Pagination { Page = pageNumber, Take = message.Take },
                message.DeliveryModes,
                message.NationalProvidersOnly,
                message.ShowAll);

            if (searchResults.TotalResults > 0 && !searchResults.Hits.Any())
            {
                var take = message.Take <= 0 ? _paginationSettings.DefaultResultsAmount : 1;
                var lastPage = take > 0 ? (int)System.Math.Ceiling((double)searchResults.TotalResults / take) : 1;
                return new StandardProviderSearchResponse { StatusCode = StandardProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound, CurrentPage = lastPage };
            }

            var standardProviderSearchResponse = new StandardProviderSearchResponse
            {
                Success = searchResults.StandardResponseCode == LocationLookupResponse.Ok,
                CurrentPage = pageNumber,
                Results = searchResults,
                TotalResultsForCountry = await GetCountResultForCountry(searchResults, message),
                SearchTerms = message.Keywords,
                ShowOnlyNationalProviders = message.NationalProvidersOnly,
                ShowAllProviders = message.ShowAll,
                StatusCode = GetResponseCode(searchResults.StandardResponseCode)
            };

            return standardProviderSearchResponse;
        }

        private ProviderSearchResponseBase<ProviderStandardSearchResults>.ResponseCodes GetResponseCode(string standardResponseCode)
        {
            return _searchResponseCodes[standardResponseCode ?? string.Empty];
        }

        private async Task<long> GetCountResultForCountry(BaseProviderSearchResults searchResults, ProviderSearchQuery message)
        {
            long totalRestultsForCountry = 0;

            if (searchResults.TotalResults > 0)
            {
                return totalRestultsForCountry;
            }

            var totalProvidersCountry = await _searchService.SearchStandardProviders(
                message.ApprenticeshipId,
                message.PostCode,
                new Pagination(),
                message.DeliveryModes,
                message.NationalProvidersOnly,
                true);

            totalRestultsForCountry = totalProvidersCountry.TotalResults;

            return totalRestultsForCountry;
        }
    }
}