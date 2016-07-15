using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class ApprenticeshipDetailsViewModel
    {
        public string Ukprn { get; set; }

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

        public ApprenticeshipTrainingType ApprenticeshipType { get; set; }

        public bool IsShortlisted { get; set; }
    }
}