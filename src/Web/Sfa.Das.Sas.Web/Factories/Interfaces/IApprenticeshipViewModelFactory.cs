using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    public interface IApprenticeshipViewModelFactory
    {
        ProviderSearchViewModel GetProviderSearchViewModelForStandard(int id, UrlHelper urlHelper);

        ProviderSearchViewModel GetFrameworkProvidersViewModel(int id, UrlHelper urlHelper);

        StandardViewModel GetStandardViewModel(Standard standard);

        FrameworkViewModel GetFrameworkViewModel(Framework framework);

        ApprenticeshipSearchResultViewModel GetApprenticeshipSearchResultViewModel(ApprenticeshipSearchResponse response);
    }
}