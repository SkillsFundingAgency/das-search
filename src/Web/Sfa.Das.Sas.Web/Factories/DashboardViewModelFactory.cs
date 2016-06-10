using System.Collections.Generic;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    using Sfa.Das.Sas.Web.Factories.Interfaces;

    public class DashboardViewModelFactory : IDashboardViewModelFactory
    {
        public DashboardViewModel GetDashboardViewModel(
            ICollection<ShortlistStandardViewModel> standardViewModels,
            ICollection<ShortlistFrameworkViewModel> frameworkViewModels)
        {
            var list = new List<IShortlistApprenticeshipViewModel>();
            list.AddRange(standardViewModels);
            list.AddRange(frameworkViewModels);
            return new DashboardViewModel
            {
                Title = "Shortlisted training and providers",
                Apprenticeships = list
            };
        }
    }
}