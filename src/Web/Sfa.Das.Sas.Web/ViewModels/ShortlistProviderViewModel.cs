using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ShortlistProviderViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Address Address { get; set; }
    }
}