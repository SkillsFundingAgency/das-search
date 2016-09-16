using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    public interface IApprenticeshipProviderSearchResultsItem
    {
        string ContactUsUrl { get; set; }
        List<string> DeliveryModes { get; set; }
        double Distance { get; set; }
        string Email { get; set; }
        bool NationalProvider { get; set; }
        double? EmployerSatisfaction { get; set; }
        double? LearnerSatisfaction { get; set; }
        double? OverallAchievementRate { get; set; }
        string MarketingName { get; set; }
        string ProviderMarketingInfo { get; set; }
        string ApprenticeshipMarketingInfo { get; set; }
        string ProviderName { get; set; }
        string Phone { get; set; }
        string ApprenticeshipInfoUrl { get; set; }
        int Ukprn { get; set; }
        string Website { get; set; }
        IEnumerable<TrainingLocation> TrainingLocations { get; set; }
        int? MatchingLocationId { get; set; }

        double? NationalOverallAchievementRate { get; set; }

        string OverallCohort { get; set; }
    }
}
