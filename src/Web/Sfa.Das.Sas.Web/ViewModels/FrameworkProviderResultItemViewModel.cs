using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class FrameworkProviderResultItemViewModel
    {
        public int UkPrn { get; set; }

        public string ProviderName { get; set; }

        public bool NationalProvider { get; set; }

        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public int Level { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public string MarketingName { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public string ApprenticeshipMarketingInfo { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactUsUrl { get; set; }

        public string ApprenticeshipInfoUrl { get; set; }

        public List<string> DeliveryModes { get; set; }

        public string Website { get; set; }

        public Address Address { get; set; }

        public double Distance { get; set; }

        public string EmployerSatisfactionMessage { get; set; }

        public string LearnerSatisfactionMessage { get; set; }

        public string AchievementRateMessage { get; set; }

        public string DeliveryOptionsMessage { get; set; }

        public string FrameworkId { get; set; }
    }
}