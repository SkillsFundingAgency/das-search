using System;
using Sfa.Das.Sas.ApplicationServices.Exceptions;
using Sfa.Das.Sas.ApplicationServices.Logging;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using SFA.DAS.NLog.Logger;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices
{
    public sealed class ProviderSearchService : IProviderSearchService
    {
        private readonly IProviderLocationSearchProvider _providerLocationSearchProvider;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly ILookupLocations _postCodeLookup;
        private readonly ILog _logger;
        private readonly IPaginationSettings _paginationSettings;
        private readonly IProviderSearchProvider _providerSearchProvider;

        public ProviderSearchService(
            IProviderLocationSearchProvider providerLocationSearchProvider,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            ILookupLocations postcodeLookup,
            ILog logger,
            IPaginationSettings paginationSettings,
            IProviderSearchProvider providerSearchProvider)
        {
            _providerLocationSearchProvider = providerLocationSearchProvider;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _postCodeLookup = postcodeLookup;
            _logger = logger;
            _paginationSettings = paginationSettings;
            _providerSearchProvider = providerSearchProvider;
        }

        public async Task<ProviderStandardSearchResults> SearchStandardProviders(string standardId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool nationalProviders, bool showAll, bool hasNonLevyContract)
        {
            ProviderStandardSearchResults result;
            if (!showAll && !nationalProviders)
            {
                result = await SearchStandardProviders(standardId, postCode, pagination, deliveryModes, ProviderFilterOptions.ApprenticeshipLocation, hasNonLevyContract);
                result.ShowNationalProvidersOnly = nationalProviders;
                return result;
            }

            if (showAll && !nationalProviders)
            {
                result = await SearchStandardProviders(standardId, postCode, pagination, deliveryModes, ProviderFilterOptions.ApprenticeshipId, hasNonLevyContract);
                result.ShowNationalProvidersOnly = nationalProviders;
                return result;
            }

            if (!showAll && nationalProviders)
            {
                result = await SearchStandardProviders(standardId, postCode, pagination, deliveryModes, ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly, hasNonLevyContract);
                result.ShowNationalProvidersOnly = nationalProviders;
                return result;
            }

            result = await SearchStandardProviders(standardId, postCode, pagination, deliveryModes, ProviderFilterOptions.ApprenticeshipIdWithNationalProviderOnly, hasNonLevyContract);
            result.ShowNationalProvidersOnly = nationalProviders;

            return result;
        }

        public async Task<ProviderFrameworkSearchResults> SearchFrameworkProviders(string frameworkId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool nationalProviders, bool showAll, bool hasNonLevyContract)
        {
            ProviderFrameworkSearchResults result;

            ProviderFilterOptions filterOption = ProviderFilterOptions.ApprenticeshipIdWithNationalProviderOnly;
            if (!showAll & !nationalProviders)
            {
                filterOption = ProviderFilterOptions.ApprenticeshipLocation;
            }
            else if (showAll && !nationalProviders)
            {
                filterOption = ProviderFilterOptions.ApprenticeshipId;
            }
            else if (!showAll && nationalProviders)
            {
                filterOption = ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly;
            }

            result = await SearchFrameworkProviders(frameworkId, postCode, pagination, deliveryModes, filterOption, hasNonLevyContract);
            result.ShowNationalProvidersOnly = nationalProviders;
            return result;
        }

        private async Task<ProviderStandardSearchResults> SearchStandardProviders(string standardId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, ProviderFilterOptions searchSelection, bool hasNonLevyContract)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderStandardSearchResults { StandardId = standardId, PostCodeMissing = true };
            }

            var standardName = string.Empty;

            try
            {
                var standard = _getStandards.GetStandardById(standardId);
                standardName = standard?.Title;

                var coordinateResponse = await _postCodeLookup.GetLatLongFromPostCode(postCode);
                var coordinates = coordinateResponse.Coordinate;

                var responseCode = standard == null ? LocationLookupResponse.ApprenticeshipNotFound : coordinateResponse.ResponseCode;

                if (coordinateResponse.ResponseCode != LocationLookupResponse.Ok)
                {
                    return GetProviderStandardSearchResultErrorResponse(standardId, standardName, postCode, responseCode);
                }

                var takeElements = pagination.Take == 0 ? _paginationSettings.DefaultResultsAmount : pagination.Take;

                LogSearchRequest(postCode, coordinates);

                var filter = new ProviderSearchFilter
                {
                    DeliveryModes = deliveryModes,
                    SearchOption = searchSelection,
                    HasNonLevyContract = hasNonLevyContract
                };

                var searchResults = _providerLocationSearchProvider.SearchStandardProviders(standardId, coordinates, pagination.Page, takeElements, filter);

                var result = new ProviderStandardSearchResults
                {
                    TotalResults = searchResults.Total,
                    ResultsToTake = takeElements,
                    StandardId = standardId,
                    StandardName = standardName,
                    StandardLevel = standard.Level,
                    PostCode = postCode,
                    Hits = searchResults.Hits,
                    TrainingOptionsAggregation = searchResults.TrainingOptionsAggregation,
                    NationalProviders = searchResults.NationalProvidersAggregation,
                    SelectedTrainingOptions = deliveryModes,
                    StandardResponseCode = responseCode,
                    ShowNationalProvidersOnly = false,
                    LastPage = takeElements > 0 ? (int)Math.Ceiling((double)searchResults.Total / takeElements) : 1
            };

                return result;
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for provider failed.");

                return GetProviderStandardSearchResultErrorResponse(standardId, standardName, postCode, ServerLookupResponse.InternalServerError);
            }
        }

        private static ProviderStandardSearchResults GetProviderStandardSearchResultErrorResponse(string standardId, string standardName, string postCode, string responseCode)
        {
            return new ProviderStandardSearchResults
            {
                TotalResults = 0,
                StandardId = standardId,
                StandardName = standardName,
                PostCode = postCode,
                Hits = new IApprenticeshipProviderSearchResultsItem[0],
                StandardResponseCode = responseCode
            };
        }

        private static ProviderFrameworkSearchResults GetProviderFrameworkSearchResultErrorResponse(string frameworkId, string postCode, string responseCode)
        {
            return new ProviderFrameworkSearchResults
            {
                TotalResults = 0,
                FrameworkId = frameworkId,
                FrameworkName = string.Empty,
                PathwayName = string.Empty,
                PostCode = postCode,
                Hits = new IApprenticeshipProviderSearchResultsItem[0],
                FrameworkResponseCode = responseCode
            };
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

            _logger.Info("Provider location search", logEntry);
        }

        private async Task<ProviderFrameworkSearchResults> SearchFrameworkProviders(string frameworkId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, ProviderFilterOptions searchSelection, bool hasNonLevyContract)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderFrameworkSearchResults { FrameworkId = frameworkId, PostCodeMissing = true };
            }

            try
            {
                var framework = _getFrameworks.GetFrameworkById(frameworkId);

                var frameworkMissing = framework == null;

                var takeElements = pagination.Take <= 0 ? _paginationSettings.DefaultResultsAmount : pagination.Take;

                var coordinateResponse = await _postCodeLookup.GetLatLongFromPostCode(postCode);
                var coordinates = coordinateResponse.Coordinate;

                var responseCode = frameworkMissing ? LocationLookupResponse.ApprenticeshipNotFound : coordinateResponse.ResponseCode;

                if (coordinateResponse.ResponseCode != LocationLookupResponse.Ok || frameworkMissing)
                {
                    return GetProviderFrameworkSearchResultErrorResponse(frameworkId, postCode, responseCode);
                }

                var filter = new ProviderSearchFilter
                {
                    DeliveryModes = deliveryModes,
                    SearchOption = searchSelection,
                    HasNonLevyContract = hasNonLevyContract
                };

                var searchResults = _providerLocationSearchProvider.SearchFrameworkProviders(frameworkId, coordinates, pagination.Page, takeElements, filter);

                var hits = searchResults.Hits;
                var total = searchResults.Total;
                var trainingOptionsAggregation = searchResults.TrainingOptionsAggregation;

                return new ProviderFrameworkSearchResults
                {
                    Title = framework?.Title,
                    TotalResults = total,
                    ResultsToTake = takeElements,
                    FrameworkId = frameworkId,
                    FrameworkCode = framework?.FrameworkCode ?? 0,
                    FrameworkName = framework?.FrameworkName,
                    PathwayName = framework?.PathwayName,
                    FrameworkLevel = framework?.Level ?? 0,
                    FrameworkResponseCode = responseCode,
                    PostCode = postCode,
                    Hits = hits,
                    TrainingOptionsAggregation = trainingOptionsAggregation,
                    NationalProviders = searchResults.NationalProvidersAggregation,
                    SelectedTrainingOptions = deliveryModes,
                    ShowNationalProvidersOnly = false,
                };
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for provider failed.");

                return GetProviderFrameworkSearchResultErrorResponse(frameworkId, postCode, ServerLookupResponse.InternalServerError);
            }
        }

        public async Task<ProviderSearchResults> SearchProviders(string apprenticeshipId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool hasNonLevyContract)
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
                    HasNonLevyContract = hasNonLevyContract
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
                _logger.Error(ex, "Search for provider failed.");

                return GetProviderSearchResultErrorResponse(apprenticeshipId, apprenticeship?.Title, postCode, ServerLookupResponse.InternalServerError);
            }
        }
    }
}
