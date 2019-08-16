using System;
using Sfa.Das.Sas.ApplicationServices.Exceptions;
using Sfa.Das.Sas.ApplicationServices.Logging;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sfa.Das.Sas.ApplicationServices
{
    public sealed class ProviderSearchService : IProviderSearchService
    {
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly ILookupLocations _postCodeLookup;
        private readonly ILogger<ProviderSearchService> _logger;
        private readonly PaginationSettings _paginationSettings;
        private readonly IProviderSearchProvider _providerSearchProvider;

        public ProviderSearchService(
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            ILookupLocations postcodeLookup,
            ILogger<ProviderSearchService> logger,
            IOptions<PaginationSettings> paginationSettings,
            IProviderSearchProvider providerSearchProvider)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _postCodeLookup = postcodeLookup;
            _logger = logger;
            _paginationSettings = paginationSettings.Value;
            _providerSearchProvider = providerSearchProvider;
        }

        private static ProviderSearchResults GetProviderSearchResultErrorResponse(string apprenticeshipId, string Title, string postCode, string responseCode)
        {

            var errorResponse = new ProviderSearchResults
            {
                TotalResults = 0,
                ApprenticeshipId = apprenticeshipId,
                Title = Title,
                PostCode = postCode,
                Hits = new ProviderSearchResultItem[0],
                ResponseCode = responseCode
            };
            return errorResponse;
        }
        private void LogSearchRequest(string postCode, Coordinate coordinates)
        {
            var logEntry = new ApprenticeshipSearchLogEntry
            {
                Postcode = postCode,
                Coordinates = new[] { coordinates.Lon, coordinates.Lat }
            };

            _logger.LogInformation("Provider location search {apprenticeshipSearch}", logEntry);
        }


        public async Task<ProviderSearchResults> SearchProviders(string apprenticeshipId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool hasNonLevyContract, bool showNationalOnly)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderSearchResults { ApprenticeshipId = apprenticeshipId, PostCodeMissing = true };
            }

            int apprenticeshipIdInt;

            IApprenticeshipProduct apprenticeship;

            if (int.TryParse(apprenticeshipId, out apprenticeshipIdInt))
            {
                apprenticeship = _getStandards.GetStandardById(apprenticeshipId);
            }
            else
            {
                apprenticeship = _getFrameworks.GetFrameworkById(apprenticeshipId);
            }

            if (apprenticeship == null)
            {
                return GetProviderSearchResultErrorResponse(apprenticeshipId, null, postCode, LocationLookupResponse.ApprenticeshipNotFound);
            }

            try
            {
                var coordinateResponse = await _postCodeLookup.GetLatLongFromPostCode(postCode);
                var coordinates = coordinateResponse.Coordinate;


                if (coordinateResponse.ResponseCode != LocationLookupResponse.Ok)
                {
                    return GetProviderSearchResultErrorResponse(apprenticeshipId, apprenticeship?.Title, postCode, coordinateResponse.ResponseCode);
                }

                var takeElements = pagination.Take == 0 ? _paginationSettings.DefaultResultsAmount : pagination.Take;

                LogSearchRequest(postCode, coordinates);

                var filter = new ProviderSearchFilter
                {
                    DeliveryModes = deliveryModes,
                    HasNonLevyContract = hasNonLevyContract,
                    ShowNationalOnly = showNationalOnly
                };

                var searchResults = await _providerSearchProvider.SearchProvidersByLocation(apprenticeshipId, coordinates, pagination.Page, takeElements, filter);

                var result = new ProviderSearchResults
                {
                    TotalResults = searchResults.Total,
                    ResultsToTake = takeElements,
                    ApprenticeshipId = apprenticeshipId,
                    Title = apprenticeship?.Title,
                    Level = apprenticeship.Level,
                    PostCode = postCode,
                    TrainingOptionsAggregation = searchResults.TrainingOptionsAggregation,
                    NationalProviders = searchResults.NationalProvidersAggregation,
                    SelectedTrainingOptions = deliveryModes,
                    ResponseCode = LocationLookupResponse.Ok,
                    ShowNationalProvidersOnly = false,
                    Hits = searchResults.Hits,
                    LastPage = takeElements > 0 ? (int)System.Math.Ceiling((double)searchResults.Total / takeElements) : 1
                };

                return result;
            }
            catch (SearchException ex)
            {
                _logger.LogError(ex, "Search for provider failed.");

                return GetProviderSearchResultErrorResponse(apprenticeshipId, apprenticeship?.Title, postCode, ServerLookupResponse.InternalServerError);
            }
        }
    }
}
