using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ShortlistStandardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public List<ShortlistProviderViewModel> Providers { get; set; }
    }
}