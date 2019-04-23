using System.Linq;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using System.Collections.Generic;
    using Sfa.Das.Sas.ApplicationServices.Models;
    public class ApprenticeshipSearchResultsMapping : IApprenticeshipSearchResultsMapping
    {
        private readonly IApprenticeshipSearchResultsItemMapping _apprenticeshipSearchResultsItemMapping;

        public ApprenticeshipSearchResultsMapping(IApprenticeshipSearchResultsItemMapping apprenticeshipSearchResultsItemMapping)
        {
            _apprenticeshipSearchResultsItemMapping = apprenticeshipSearchResultsItemMapping;
        }

        public ApprenticeshipSearchResults Map(IEnumerable<SFA.DAS.Apprenticeships.Api.Types.ApprenticeshipSearchResultsItem> document)
        {
            var apprenticeshipSearchResults = new ApprenticeshipSearchResults();

            if (document != null && document.Any())
            {
                apprenticeshipSearchResults.Results = document.Select(_apprenticeshipSearchResultsItemMapping.Map);
            }

            return apprenticeshipSearchResults;
        }
    }
}