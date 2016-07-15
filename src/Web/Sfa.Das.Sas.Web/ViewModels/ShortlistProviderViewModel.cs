using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ShortlistProviderViewModel
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Address Address { get; set; }
        public string Url { get; set; }
    }
}