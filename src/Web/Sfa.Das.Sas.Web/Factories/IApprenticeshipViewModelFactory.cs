namespace Sfa.Das.Sas.Web.Factories
{
    using System.Web.Mvc;

    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Web.ViewModels;

    public interface IApprenticeshipViewModelFactory
    {
        ProviderSearchViewModel GetProviderSearchViewModelForStandard(int id, UrlHelper urlHelper);

        ProviderSearchViewModel GetFrameworkProvidersViewModel(int id, UrlHelper urlHelper);

        StandardViewModel GetStandardViewModel(int id);

        FrameworkViewModel GetFrameworkViewModel(int id);

        ApprenticeshipSearchResultViewModel GetSApprenticeshipSearchResultViewModel(ApprenticeshipSearchResults searchResults);
    }
}