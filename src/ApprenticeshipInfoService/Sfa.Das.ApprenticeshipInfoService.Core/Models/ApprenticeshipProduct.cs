namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    using System.Collections.Generic;

    public class ApprenticeshipProduct
    {
        public ApprenticeshipBasic Apprenticeship { get; set; }

        public double? EmployerSatisfaction { get; set; }

        public double? LearnerSatisfaction { get; set; }

        public double? AchievementRate { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public List<string> DeliveryModes { get; set; }

        public double? NationalAchievementRate { get; set; }

        public string OverallCohort { get; set; }
    }
}
