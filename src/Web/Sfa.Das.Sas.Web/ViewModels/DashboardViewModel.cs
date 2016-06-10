using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class DashboardViewModel : IHasError
    {
        public bool HasError { get; set; }

        public IEnumerable<IShortlistApprenticeshipViewModel> Apprenticeships { get; set; }

        public string Title { get; set; }
    }
}