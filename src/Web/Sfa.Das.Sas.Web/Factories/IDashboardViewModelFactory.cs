using System.Collections.Generic;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public interface IDashboardViewModelFactory
    {
        DashboardViewModel GetDashboardViewModel(
            ICollection<ShortlistStandardViewModel> standardViewModels,
            ICollection<ShortlistFrameworkViewModel> frameworkViewModels);
    }
}
