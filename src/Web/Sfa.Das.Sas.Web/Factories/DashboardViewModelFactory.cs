using System.Collections.Generic;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public class DashboardViewModelFactory : IDashboardViewModelFactory
    {
        public DashboardViewModel GetDashboardViewModel(ICollection<ShortlistStandardViewModel> standardViewModels)
        {
            return new DashboardViewModel
            {
                Title = "Your apprenticeship shortlist",
                Standards = standardViewModels
            };
        }
    }
}