using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public sealed class FrameworkProviderSearchHandler : IAsyncRequestHandler<FrameworkProviderSearchQuery, FrameworkProviderSearchResponse>
    {
        private readonly ILog _logger;
        private readonly IProviderSearchService _searchService;
        private readonly IPaginationSettings _paginationSettings;
        private readonly IPostcodeIoService _postcodeIoService;
        private readonly AbstractValidator<ProviderSearchQuery> _validator;

        public FrameworkProviderSearchHandler(
            AbstractValidator<ProviderSearchQuery> validator,
            IProviderSearchService searchService,
            IPaginationSettings paginationSettings,
            IPostcodeIoService postcodeIoService,
            ILog logger)
        {
            _validator = validator;
            _searchService = searchService;
            _paginationSettings = paginationSettings;
            _postcodeIoService = postcodeIoService;
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
                    response.StatusCode = ProviderSearchResponseCodes.InvalidApprenticeshipId;
                }

                if (result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidPostcode))
                {
                    response.StatusCode = ProviderSearchResponseCodes.PostCodeInvalidFormat;
                }
                return response;
            }

            var postcodeStatus = await GetPostcodeStatus(message.PostCode);

            switch (postcodeStatus)
            {
                case ProviderSearchResponseCodes.WalesPostcode:
                case ProviderSearchResponseCodes.ScotlandPostcode:
                case ProviderSearchResponseCodes.NorthernIrelandPostcode:
                case ProviderSearchResponseCodes.PostCodeTerminated:
                case ProviderSearchResponseCodes.PostCodeInvalidFormat:
                    return new FrameworkProviderSearchResponse
                    {
                        Success = false,
                        StatusCode = postcodeStatus
                    };
                default:
                    message.Page = message.Page <= 0 ? 1 : message.Page;

                    return await PerformSearch(message);
            }
        }

        private async Task<ProviderSearchResponseCodes> GetPostcodeStatus(string postcode)
        {
            var status = await _postcodeIoService.GetPostcodeStatus(postcode);
            switch (status)
            {
                case "Wales":
                    return ProviderSearchResponseCodes.WalesPostcode;
                case "Scotland":
                    return ProviderSearchResponseCodes.ScotlandPostcode;
                case "Northern Ireland":
                    return ProviderSearchResponseCodes.NorthernIrelandPostcode;
                case "Terminated":
                    return ProviderSearchResponseCodes.PostCodeTerminated;
                case "Error":
                    return ProviderSearchResponseCodes.PostCodeInvalidFormat;
            }

            return ProviderSearchResponseCodes.Success;
        }

        private async Task<FrameworkProviderSearchResponse> PerformSearch(FrameworkProviderSearchQuery message)
        {
            var pageNumber = message.Page <= 0 ? 1 : message.Page;

            var hasNonLevyContract = message.IsLevyPayingEmployer == false;

            var searchResults = await _searchService.SearchFrameworkProviders(
                message.ApprenticeshipId,
                message.PostCode,
                new Pagination { Page = pageNumber, Take = _paginationSettings.DefaultResultsAmount },
                message.DeliveryModes,
                message.NationalProvidersOnly,
                message.ShowAll,
                hasNonLevyContract);

            if (searchResults.TotalResults > 0 && !searchResults.Hits.Any())
            {
                var take = _paginationSettings.DefaultResultsAmount;
                var lastPage = take > 0 ? (int)System.Math.Ceiling((double)searchResults.TotalResults / take) : 1;
                return new FrameworkProviderSearchResponse { StatusCode = ProviderSearchResponseCodes.PageNumberOutOfUpperBound, CurrentPage = lastPage };
            }

            return new FrameworkProviderSearchResponse
            {
                Success = searchResults.FrameworkResponseCode == LocationLookupResponse.Ok,
                CurrentPage = pageNumber,
                Results = searchResults,
                TotalResultsForCountry = await GetCountResultForCountry(searchResults, message),
                SearchTerms = message.Keywords,
                ShowOnlyNationalProviders = message.NationalProvidersOnly,
                ShowAllProviders = message.ShowAll,
                StatusCode = GetResponseCode(searchResults.FrameworkResponseCode)
            };
        }

        private ProviderSearchResponseCodes GetResponseCode(string frameworkResponseCode)
        {
            switch (frameworkResponseCode)
            {
                case LocationLookupResponse.WrongPostcode:
                    return ProviderSearchResponseCodes.PostCodeInvalidFormat;
                case LocationLookupResponse.ServerError:
                    return ProviderSearchResponseCodes.LocationServiceUnavailable;
                case LocationLookupResponse.ApprenticeshipNotFound:
                    return ProviderSearchResponseCodes.ApprenticeshipNotFound;
                case ServerLookupResponse.InternalServerError:
                    return ProviderSearchResponseCodes.ServerError;
                default:
                    return default(ProviderSearchResponseCodes);
            }
        }

        private async Task<long> GetCountResultForCountry(ProviderFrameworkSearchResults searchResults, FrameworkProviderSearchQuery message)
        {
            long totalRestultsForCountry = 0;
            if (searchResults.TotalResults > 0)
            {
                return totalRestultsForCountry;
            }

            var hasNonLevyContract = message.IsLevyPayingEmployer == false;

            var totalProvidersCountry = await _searchService.SearchFrameworkProviders(
                message.ApprenticeshipId,
                message.PostCode,
                new Pagination(),
                message.DeliveryModes,
                message.NationalProvidersOnly,
                true,
                hasNonLevyContract);

            totalRestultsForCountry = totalProvidersCountry.TotalResults;

            return totalRestultsForCountry;
        }
    }
}