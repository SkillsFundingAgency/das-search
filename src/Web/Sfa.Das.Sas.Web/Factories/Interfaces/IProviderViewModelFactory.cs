namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    using Sfa.Das.Sas.Web.Models;
    using Sfa.Das.Sas.Web.ViewModels;

    public interface IProviderViewModelFactory
    {
        ApprenticeshipDetailsViewModel GenerateDetailsViewModel(ProviderLocationSearchCriteria criteria);
    }
}