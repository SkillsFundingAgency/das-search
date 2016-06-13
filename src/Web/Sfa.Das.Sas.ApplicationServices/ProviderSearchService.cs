using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Exceptions;
using Sfa.Das.Sas.ApplicationServices.Logging;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices
{
    public sealed class ProviderSearchService : IProviderSearchService
    {
        private readonly IProviderLocationSearchProvider _searchProvider;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly ILookupLocations _postCodeLookup;
        private readonly ILog _logger;
        private readonly IPaginationSettings _paginationSettings;

        public ProviderSearchService(
            IProviderLocationSearchProvider searchProvider,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            ILookupLocations postcodeLookup,
            ILog logger,
            IPaginationSettings paginationSettings)
        {
            _searchProvider = searchProvider;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _postCodeLookup = postcodeLookup;
            _logger = logger;
            _paginationSettings = paginationSettings;
        }

        public async Task<ProviderStandardSearchResults> SearchStandardProviders(int standardId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool showAll)
        {
            if (!showAll)
            {
                return await SearchByStandardPostCode(standardId, postCode, pagination, deliveryModes);
            }

            return await SearchByStandard(standardId, postCode, pagination, deliveryModes);
        }

        public async Task<ProviderFrameworkSearchResults> SearchFrameworkProviders(int frameworkId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool showAll)
        {
            if (!showAll)
            {
                return await SearchByFrameworkPostCode(frameworkId, postCode, pagination, deliveryModes);
            }

            return await SearchByFramework(frameworkId, postCode, pagination, deliveryModes);
        }

        private async Task<ProviderStandardSearchResults> SearchByStandardPostCode(int standardId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderStandardSearchResults { StandardId = standardId, PostCodeMissing = true };
            }

            var standardName = string.Empty;

            try
            {
                if (standardId < 0)
                {
                    throw new SearchException("Standard ID cannot be negative");
                }

                var standard = _getStandards.GetStandardById(standardId);
                standardName = standard?.Title;

                var standardMissing = standard == null;

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                if (coordinates == null)
                {
                    return new ProviderStandardSearchResults
                    {
                        TotalResults = 0,
                        StandardId = standardId,
                        StandardName = standardName,
                        PostCode = postCode,
                        StandardNotFound = standardMissing,
                        Hits = new IApprenticeshipProviderSearchResultsItem[0],
                        HasError = false
                    };
                }

                var takeElements = pagination.Take == 0 ? _paginationSettings.DefaultResultsAmount : pagination.Take;
                
                var logEntry = new ApprenticeshipSearchLogEntry
                {
                    Postcode = postCode,
                    Coordinates = new[] { coordinates.Lon, coordinates.Lat }
                };

                _logger.Info("Provider location search", logEntry);

                var searchResults = _searchProvider.SearchByStandardLocation(standardId, coordinates, pagination.Page, takeElements, deliveryModes);

                var result = new ProviderStandardSearchResults
                {
                    TotalResults = searchResults.Total,
                    ResultsToTake = takeElements,
                    StandardId = standardId,
                    StandardName = standardName,
                    PostCode = postCode,
                    StandardNotFound = standardMissing,
                    Hits = searchResults.Hits,
                    TrainingOptionsAggregation = searchResults.TrainingOptionsAggregation,
                    SelectedTrainingOptions = deliveryModes
                };

                return result;
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for provider failed.");

                return new ProviderStandardSearchResults
                {
                    TotalResults = 0,
                    StandardId = standardId,
                    StandardName = standardName,
                    PostCode = postCode,
                    Hits = new IApprenticeshipProviderSearchResultsItem[0],
                    HasError = true
                };
            }
        }

        private async Task<ProviderStandardSearchResults> SearchByStandard(int standardId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderStandardSearchResults { StandardId = standardId, PostCodeMissing = true };
            }

            var standardName = string.Empty;

            try
            {
                if (standardId < 0)
                {
                    throw new SearchException("Standard ID cannot be negative");
                }

                var standard = _getStandards.GetStandardById(standardId);
                standardName = standard?.Title;

                var standardMissing = standard == null;

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                if (coordinates == null)
                {
                    return new ProviderStandardSearchResults
                    {
                        TotalResults = 0,
                        StandardId = standardId,
                        StandardName = standardName,
                        PostCode = postCode,
                        StandardNotFound = standardMissing,
                        Hits = new IApprenticeshipProviderSearchResultsItem[0],
                        HasError = false
                    };
                }

                var takeElements = pagination.Take == 0 ? _paginationSettings.DefaultResultsAmount : pagination.Take;

                var logEntry = new ApprenticeshipSearchLogEntry
                {
                    Postcode = postCode,
                    Coordinates = new[] { coordinates.Lon, coordinates.Lat }
                };

                _logger.Info("Provider Location Search", logEntry);

                var searchResults = _searchProvider.SearchByStandard(standardId, coordinates, pagination.Page, takeElements, deliveryModes);

                var result = new ProviderStandardSearchResults
                {
                    TotalResults = searchResults.Total,
                    ResultsToTake = takeElements,
                    StandardId = standardId,
                    StandardName = standardName,
                    PostCode = postCode,
                    StandardNotFound = standardMissing,
                    Hits = searchResults.Hits,
                    TrainingOptionsAggregation = searchResults.TrainingOptionsAggregation,
                    SelectedTrainingOptions = deliveryModes
                };

                return result;
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for provider failed.");

                return new ProviderStandardSearchResults
                {
                    TotalResults = 0,
                    StandardId = standardId,
                    StandardName = standardName,
                    PostCode = postCode,
                    Hits = new IApprenticeshipProviderSearchResultsItem[0],
                    HasError = true
                };
            }
        }

        private async Task<ProviderFrameworkSearchResults> SearchByFrameworkPostCode(int frameworkId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderFrameworkSearchResults { FrameworkId = frameworkId, PostCodeMissing = true };
            }

            try
            {
                if (frameworkId < 0)
                {
                    throw new SearchException("Framework ID cannot be negative");
                }

                var framework = _getFrameworks.GetFrameworkById(frameworkId);

                var frameworkMissing = framework == null;

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                IEnumerable<IApprenticeshipProviderSearchResultsItem> hits;
                var total = 0L;
                Dictionary<string, long?> trainingOptionsAggregation;
                var takeElements = pagination.Take <= 0 ? _paginationSettings.DefaultResultsAmount : pagination.Take;

                if (coordinates != null)
                {
                    var searchResults = _searchProvider.SearchByFrameworkLocation(frameworkId, coordinates, pagination.Page, takeElements, deliveryModes);
                    hits = searchResults.Hits;
                    total = searchResults.Total;
                    trainingOptionsAggregation = searchResults.TrainingOptionsAggregation;
                }
                else
                {
                    hits = new IApprenticeshipProviderSearchResultsItem[0];
                    trainingOptionsAggregation = new Dictionary<string, long?>();
                }

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
                    PostCode = postCode,
                    Hits = hits,
                    TrainingOptionsAggregation = trainingOptionsAggregation,
                    SelectedTrainingOptions = deliveryModes,
                    FrameworkIsMissing = frameworkMissing
                };
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for provider failed.");

                return new ProviderFrameworkSearchResults
                {
                    TotalResults = 0,
                    FrameworkId = frameworkId,
                    FrameworkName = string.Empty,
                    PathwayName = string.Empty,
                    PostCode = postCode,
                    Hits = new IApprenticeshipProviderSearchResultsItem[0],
                    HasError = true
                };
            }
        }

        private async Task<ProviderFrameworkSearchResults> SearchByFramework(int frameworkId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderFrameworkSearchResults { FrameworkId = frameworkId, PostCodeMissing = true };
            }

            try
            {
                if (frameworkId < 0)
                {
                    throw new SearchException("Framework ID cannot be negative");
                }

                var framework = _getFrameworks.GetFrameworkById(frameworkId);

                var frameworkMissing = framework == null;

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                IEnumerable<IApprenticeshipProviderSearchResultsItem> hits;
                var total = 0L;
                Dictionary<string, long?> trainingOptionsAggregation;
                var takeElements = pagination.Take <= 0 ? _paginationSettings.DefaultResultsAmount : pagination.Take;

                if (coordinates != null)
                {
                    var searchResults = _searchProvider.SearchByFramework(frameworkId, coordinates, pagination.Page, takeElements, deliveryModes);
                    hits = searchResults.Hits;
                    total = searchResults.Total;
                    trainingOptionsAggregation = searchResults.TrainingOptionsAggregation;
                }
                else
                {
                    hits = new IApprenticeshipProviderSearchResultsItem[0];
                    trainingOptionsAggregation = new Dictionary<string, long?>();
                }

                return new ProviderFrameworkSearchResults
                {
                    Title = framework?.Title,
                    TotalResults = total,
                    ResultsToTake = takeElements,
                    FrameworkId = frameworkId,
                    FrameworkCode = framework?.FrameworkCode ?? 0,
                    FrameworkName = framework?.FrameworkName ?? string.Empty,
                    PathwayName = framework?.PathwayName,
                    FrameworkLevel = framework?.Level ?? 0,
                    PostCode = postCode,
                    Hits = hits,
                    TrainingOptionsAggregation = trainingOptionsAggregation,
                    SelectedTrainingOptions = deliveryModes,
                    FrameworkIsMissing = frameworkMissing
                };
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for provider failed.");

                return new ProviderFrameworkSearchResults
                {
                    TotalResults = 0,
                    FrameworkId = frameworkId,
                    FrameworkName = string.Empty,
                    PathwayName = string.Empty,
                    PostCode = postCode,
                    Hits = new IApprenticeshipProviderSearchResultsItem[0],
                    HasError = true
                };
            }
        }
    }
}
