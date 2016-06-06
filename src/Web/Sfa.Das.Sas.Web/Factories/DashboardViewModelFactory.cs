using System.Collections.Generic;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public class DashboardViewModelFactory : IDashboardViewModelFactory
    {
        public DashboardViewModel GetDashboardViewModel(
            ICollection<ShortlistStandardViewModel> standardViewModels,
            ICollection<ShortlistFrameworkViewModel> frameworkViewModels)
        {
            return new DashboardViewModel
            {
                Title = "Shortlisted training and providers",
                Standards = standardViewModels,
                Frameworks = frameworkViewModels
            };
        }
    }
}