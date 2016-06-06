using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class DashboardViewModel : IHasError
    {
        public DashboardViewModel()
        {
            Standards = new List<ShortlistStandardViewModel>();
            Frameworks = new List<ShortlistFrameworkViewModel>();
        }

        public bool HasError { get; set; }

        public IEnumerable<ShortlistStandardViewModel> Standards { get; set; }
        public IEnumerable<ShortlistFrameworkViewModel> Frameworks { get; set; }

        public string Title { get; set; }
    }
}