using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.ApplicationServices
{
    public interface IApprenticeshipSearchService
    {
        ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, List<string> selectedLevels);
    }
}
