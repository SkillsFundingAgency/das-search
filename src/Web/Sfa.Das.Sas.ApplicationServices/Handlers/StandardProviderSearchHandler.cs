﻿using Sfa.Das.Sas.ApplicationServices.Services;
using SFA.DAS.NLog.Logger;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Domain.Model;
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
        private readonly IPostcodeIoService _postcodeIoService;
        private readonly AbstractValidator<ProviderSearchQuery> _validator;

        private readonly Dictionary<string, ProviderSearchResponseCodes> _searchResponseCodes =
            new Dictionary<string, ProviderSearchResponseCodes>
                {
                  { LocationLookupResponse.WrongPostcode, ProviderSearchResponseCodes.PostCodeInvalidFormat },
                  { LocationLookupResponse.ServerError, ProviderSearchResponseCodes.LocationServiceUnavailable },
                  { LocationLookupResponse.ApprenticeshipNotFound, ProviderSearchResponseCodes.ApprenticeshipNotFound },
                  { ServerLookupResponse.InternalServerError, ProviderSearchResponseCodes.ServerError },
                  { LocationLookupResponse.Ok, ProviderSearchResponseCodes.Success },
                  { string.Empty, ProviderSearchResponseCodes.Success }
                };

        public StandardProviderSearchHandler(
            ProviderSearchQueryValidator validator,
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

        public async Task<StandardProviderSearchResponse> Handle(StandardProviderSearchQuery message)
        {
            var result = _validator.Validate(message);

            if (!result.IsValid)
            {
                var response = new StandardProviderSearchResponse { Success = false };

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
                    return new StandardProviderSearchResponse
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

        private async Task<StandardProviderSearchResponse> PerformSearch(ProviderSearchQuery message)
        {
            var pageNumber = message.Page <= 0 ? 1 : message.Page;

            var hasNonLevyContract = message.IsLevyPayingEmployer == false;

            var searchResults = await _searchService.SearchStandardProviders(
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
                return new StandardProviderSearchResponse { StatusCode = ProviderSearchResponseCodes.PageNumberOutOfUpperBound, CurrentPage = lastPage };
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

        private ProviderSearchResponseCodes GetResponseCode(string standardResponseCode)
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

            var hasNonLevyContract = message.IsLevyPayingEmployer == false;

            var totalProvidersCountry = await _searchService.SearchStandardProviders(
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