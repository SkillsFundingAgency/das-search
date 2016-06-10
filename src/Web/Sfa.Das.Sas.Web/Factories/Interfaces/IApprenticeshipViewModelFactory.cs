namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    using System.Web.Mvc;

    using ApplicationServices.Models;
    using ViewModels;

    public interface IApprenticeshipViewModelFactory
    {
        ProviderSearchViewModel GetProviderSearchViewModelForStandard(int id, UrlHelper urlHelper);

        ProviderSearchViewModel GetFrameworkProvidersViewModel(int id, UrlHelper urlHelper);

        StandardViewModel GetStandardViewModel(int id);

        FrameworkViewModel GetFrameworkViewModel(int id);

        ApprenticeshipSearchResultViewModel GetSApprenticeshipSearchResultViewModel(ApprenticeshipSearchResults searchResults);
    }
}