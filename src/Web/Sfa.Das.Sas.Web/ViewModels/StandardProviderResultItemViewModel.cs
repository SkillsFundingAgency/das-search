using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class StandardProviderResultItemViewModel
    {
        public int UkPrn { get; set; }

        public bool IsHigherEducationInstitute { get; set; }

        public string ProviderName { get; set; }

        public bool NationalProvider { get; set; }

        public int StandardCode { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public string MarketingName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactUsUrl { get; set; }

        public List<string> DeliveryModes { get; set; }

        public string Website { get; set; }

        public Address Address { get; set; }

        public double Distance { get; set; }

        public string EmployerSatisfactionMessage { get; set; }

        public string LearnerSatisfactionMessage { get; set; }

        public string AchievementRateMessage { get; set; }

        public string DeliveryOptionsMessage { get; set; }

        public string LocationAddressLine { get; set; }
    }
}