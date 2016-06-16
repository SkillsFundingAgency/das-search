using System.Collections.Generic;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    public interface IDashboardViewModelFactory
    {
        DashboardViewModel GetDashboardViewModel(IEnumerable<IShortlistApprenticeshipViewModel> apprenticeships);
    }
}
