using System.Collections.Generic;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Web.ViewModels
{
    public class ProviderViewModel
    {
        public string Name { get; set; }

        public Location Location { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public List<string> DeliveryModes { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public Address Address { get; set; }

        public string EmployerSatisfactionMessage { get; set; }

        public string LearnerSatisfactionMessage { get; set; }

        public ApprenticeshipBasic Apprenticeship { get; set; }

        public string ApprenticeshipNameWithLevel { get; set; }

        public LinkViewModel SearchResultLink { get; set; }

        public ApprenticeshipTrainingType Training { get; set; }
    }
}