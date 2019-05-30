using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ProviderSearchResultItem
    {
        public int Ukprn { get; set; }
        public string ProviderName { get; set; }
        public bool NationalProvider { get; set; }
        public double? EmployerSatisfaction { get; set; }
        public double? LearnerSatisfaction { get; set; }
        public double? OverallAchievementRate { get; set; }
        public List<string> DeliveryModes { get; set; }
        public double Distance { get; set; }
    }
}
