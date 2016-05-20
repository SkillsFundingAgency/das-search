namespace Sfa.Das.Sas.Web.Factories
{
    using System.Web.Mvc;

    using Sfa.Das.Sas.Web.ViewModels;

    public interface IApprenticeshipViewModelFactory
    {
        ProviderSearchViewModel GetStandardViewModel(int id, UrlHelper urlHelper);

        ProviderSearchViewModel GetFrameworkProvidersViewModel(int id, UrlHelper urlHelper);
    }
}