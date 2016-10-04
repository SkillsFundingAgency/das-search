namespace Sfa.Das.ApprenticeshipInfoService.Core.Models.Responses
{
    using System.Collections.Generic;

    public sealed class StandardProviderSearchResultsItemResponse : IApprenticeshipProviderSearchResultsItem
    {
        // TODO Add URI
        public int Ukprn { get; set; }

        public string ProviderName { get; set; }

        public double? OverallAchievementRate { get; set; }

        public string MarketingName { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public string ApprenticeshipMarketingInfo { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool NationalProvider { get; set; }

        public string ContactUsUrl { get; set; }

        public string ApprenticeshipInfoUrl { get; set; }

        public List<string> DeliveryModes { get; set; }

        public string Website { get; set; }

        public IEnumerable<TrainingLocation> TrainingLocations { get; set; }

        public double Distance { get; set; }

        public double? EmployerSatisfaction { get; set; }

        public double? LearnerSatisfaction { get; set; }

        public double? NationalOverallAchievementRate { get; set; }

        public string OverallCohort { get; set; }
    }
}
