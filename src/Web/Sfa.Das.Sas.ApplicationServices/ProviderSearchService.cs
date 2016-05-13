using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Exceptions;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices
{
    public sealed class ProviderSearchService : IProviderSearchService
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly ILookupLocations _postCodeLookup;
        private readonly ILog _logger;

        public ProviderSearchService(
            ISearchProvider searchProvider,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            ILookupLocations postcodeLookup,
            ILog logger)
        {
            _searchProvider = searchProvider;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _postCodeLookup = postcodeLookup;
            _logger = logger;
        }

        public async Task<ProviderStandardSearchResults> SearchByStandardPostCode(int standardId, string postCode, IEnumerable<string> deliveryModes)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderStandardSearchResults { StandardId = standardId, PostCodeMissing = true };
            }

            string standardName = string.Empty;

            try
            {
                if (standardId < 0)
                {
                    throw new SearchException("StandardId can't be negative");
                }

                standardName = _getStandards.GetStandardById(standardId)?.Title;

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                if (coordinates == null)
                {
                    return new ProviderStandardSearchResults
                    {
                        TotalResults = 0,
                        StandardId = standardId,
                        StandardName = standardName,
                        PostCode = postCode,
                        Hits = new IApprenticeshipProviderSearchResultsItem[0],
                        HasError = false
                    };
                }
                _logger.Info($"Provider Location Search: {postCode}, {coordinates}", new Dictionary<string, object> { { "postCode", postCode }, { "coordinates", new double[] { coordinates.Lon, coordinates.Lat } } });

                var searchResults = _searchProvider.SearchByStandardLocation(standardId, coordinates, deliveryModes);

                var result = new ProviderStandardSearchResults
                {
                    TotalResults = searchResults.Total,
                    StandardId = standardId,
                    StandardName = standardName,
                    PostCode = postCode,
                    Hits = searchResults.Hits,
                    TrainingOptionsAggregation = searchResults.TrainingOptionsAggregation,
                    SelectedTrainingOptions = deliveryModes
                };

                return result;
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for Provider failed.");

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

        public async Task<ProviderFrameworkSearchResults> SearchByFrameworkPostCode(int frameworkId, string postCode, IEnumerable<string> deliveryModes)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderFrameworkSearchResults { FrameworkId = frameworkId, PostCodeMissing = true };
            }

            try
            {
                if (frameworkId < 0)
                {
                    throw new SearchException("FrameworkId can't be negative");
                }

                var framework = _getFrameworks.GetFrameworkById(frameworkId);

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                IEnumerable<IApprenticeshipProviderSearchResultsItem> hits;
                var total = 0L;
                Dictionary<string, long?> trainingOptionsAggregation;

                if (coordinates != null)
                {
                    var searchResults = _searchProvider.SearchByFrameworkLocation(frameworkId, coordinates, deliveryModes);
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
                    FrameworkId = frameworkId,
                    FrameworkCode = framework?.FrameworkCode ?? 0,
                    FrameworkName = framework?.FrameworkName,
                    PathwayName = framework?.PathwayName,
                    FrameworkLevel = framework?.Level ?? 0,
                    PostCode = postCode,
                    Hits = hits,
                    TrainingOptionsAggregation = trainingOptionsAggregation,
                    SelectedTrainingOptions = deliveryModes
                };
            }
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for Provider failed.");

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
