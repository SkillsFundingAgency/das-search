using Sfa.Das.FatApi.Client.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using System.Collections.Generic;
    using Sfa.Das.Sas.ApplicationServices.Models;

    public interface IApprenticeshipSearchResultsMapping
    {
        ApprenticeshipSearchResults Map(SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResults document);
    }
}