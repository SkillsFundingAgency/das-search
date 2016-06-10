namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    using System.Collections.Generic;

    using Sfa.Das.Sas.Web.ViewModels;

    public interface IDashboardViewModelFactory
    {
        DashboardViewModel GetDashboardViewModel(
            ICollection<ShortlistStandardViewModel> standardViewModels,
            ICollection<ShortlistFrameworkViewModel> frameworkViewModels);
    }
}
