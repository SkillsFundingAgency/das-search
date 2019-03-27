namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using System.Collections.Generic;
    using Sfa.Das.Sas.ApplicationServices.Models;

    public interface IApprenticeshipSearchResultsMapping
    {
        ApprenticeshipSearchResults Map(IEnumerable<SFA.DAS.Apprenticeships.Api.Types.ApprenticeshipSearchResultsItem> document);
    }
}