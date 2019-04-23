using System.Linq;
using Sfa.Das.FatApi.Client.Api;

namespace Sfa.Das.Sas.Infrastructure.Providers
{
    using Sfa.Das.Sas.ApplicationServices;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Infrastructure.Mapping;
    using System.Collections.Generic;

    public class ApprenticeshipsSearchApiProvider : IApprenticeshipSearchProvider
    {
        private readonly ISearchApi _apprenticeshipProgrammeApiClient;
        private readonly IApprenticeshipSearchResultsMapping _apprenticeshipSearchResultsMapping;
        public ApprenticeshipsSearchApiProvider(ISearchApi apprenticeshipProgrammeApiClient, IApprenticeshipSearchResultsMapping apprenticeshipSearchResultsMapping)
        {
            _apprenticeshipProgrammeApiClient = apprenticeshipProgrammeApiClient;
            _apprenticeshipSearchResultsMapping = apprenticeshipSearchResultsMapping;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, int order, List<int> selectedLevels)
        {
            var selectedLevelsCsv = (selectedLevels != null && selectedLevels.Any()) ? string.Join(",", selectedLevels) : null;
            var results = _apprenticeshipSearchResultsMapping.Map(_apprenticeshipProgrammeApiClient.SearchActiveApprenticeships(keywords, page, take, order, selectedLevelsCsv));
            results.SearchTerm = keywords;
            return results;
        }
    }
}
