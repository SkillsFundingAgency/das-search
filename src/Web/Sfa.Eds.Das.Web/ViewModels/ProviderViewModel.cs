using System.Collections.Generic;

namespace Sfa.Eds.Das.Web.ViewModels
{
    using Sfa.Das.ApplicationServices.Models;

    public class ProviderViewModel
    {
        public int Id { get; set; }

        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public int StandardCode { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public string MarketingName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactUsUrl { get; set; }

        public string StandardInfoUrl { get; set; }

        public List<string> DeliveryModes { get; set; }

        public string Website { get; set; }

        public Address Address { get; set; }

        public double Distance { get; set; }

        public double EmployerSatisfaction { get; set; }

        public double LearnerSatisfaction { get; set; }
    }
}