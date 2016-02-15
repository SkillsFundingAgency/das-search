using System.Collections.Generic;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Web.ViewModels
{
    public class ProviderViewModel
    {
        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string VenueName { get; set; }

        public string PostCode { get; set; }

        public Coordinate Coordinate { get; set; }

        public int Radius { get; set; }

        public List<int> StandardsId { get; set; }

        public LinkViewModel SearchResultLink { get; set; }
    }
}