using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderCourseViewModel
    {
        public string ProviderId { get; set; }

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

        public string ApprenticeshipNameWithLevel { get; set; }

        public string ApprenticeshipLevel { get; set; }

      //  public LinkViewModel SearchResultLink { get; set; }

        public ApprenticeshipTrainingType Training { get; set; }

        public string SurveyUrl { get; set; }
        public bool IsShortlisted { get; set; }
    }
}