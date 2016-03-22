using System.Collections.Generic;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Web.ViewModels
{
    public class ProviderViewModel
    {
        public string Name { get; set; }

        public string LocationName { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public List<string> DeliveryModes { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public Address Address { get; set; }

        public double EmployerSatisfaction { get; set; }

        public double LearnerSatisfaction { get; set; }

        public StandardBasic Standard { get; set; }

        public string StandardNameWithLevel { get; set; }

        public LinkViewModel SearchResultLink { get; set; }
    }
}