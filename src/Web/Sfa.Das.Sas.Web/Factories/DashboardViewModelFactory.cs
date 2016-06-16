using System.Collections.Generic;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public class DashboardViewModelFactory : IDashboardViewModelFactory
    {
        public DashboardViewModel GetDashboardViewModel(IEnumerable<IShortlistApprenticeshipViewModel> apprenticeships)
        {
            return new DashboardViewModel
            {
                Title = "Shortlisted training and providers",
                Apprenticeships = apprenticeships
            };
        }
    }
}