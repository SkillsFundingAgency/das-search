using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class ApprenticeshipDetailsViewModel
    {
        public string Ukprn { get; set; }

        public bool IsHigherEducationInstitute { get; set; }

        public string Name { get; set; }

        public Location Location { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public List<string> DeliveryModes { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public Address Address { get; set; }

        public int EmployerSatisfaction { get; set; }

        public string EmployerSatisfactionMessage { get; set; }

        public double LearnerSatisfaction { get; set; }

        public string LearnerSatisfactionMessage { get; set; }

        public ApprenticeshipBasic Apprenticeship { get; set; }

        public string ApprenticeshipName { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public ApprenticeshipTrainingType ApprenticeshipType { get; set; }

        public string SurveyUrl { get; set; }

        public string NationalAchievementRateMessage { get; set; }

        public bool NationalProvider { get; set; }

        public string AchievementRateMessage { get; set; }

        public string OverallCohort { get; set; }

        public int AchievementRate { get; set; }

        public int NationalAchievementRate { get; set; }

        public string SatisfactionSourceUrl { get; set; }

        public string AchievementRateSourceUrl { get; set; }

        public string LocationAddressLine { get; set; }

        public bool? IsLevyPayingEmployer { get; set; }

        public bool HasParentCompanyGuarantee { get; set; }

        public bool IsNewProvider { get; set; }

        public bool HasNonLevyContract { get; set; }
    }
}