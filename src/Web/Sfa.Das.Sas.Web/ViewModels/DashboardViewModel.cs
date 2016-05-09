using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class DashboardViewModel : IHasError
    {
        public bool HasError { get; set; }

        public IEnumerable<ShortlistStandardViewModel> Standards { get; set; }

        public string Title { get; set; }
    }
}