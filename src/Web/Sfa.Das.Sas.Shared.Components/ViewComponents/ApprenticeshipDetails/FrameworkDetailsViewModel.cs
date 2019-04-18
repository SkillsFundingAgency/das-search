using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sfa.Das.Sas.Resources;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails
{
    public class FrameworkDetailsViewModel
    {
        private string _professionalRegistration;
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int Level { get; set; }
        public string EquivalentText => EquivalenceLevelService.GetApprenticeshipLevel(Level.ToString());
        public int Duration { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public string Overview { get; set; }
        public IEnumerable<string> CompetencyQualification{ get; set; }
        public IEnumerable<string> KnowledgeQualification { get; set; }
        public IEnumerable<string> CombinedQualification { get; set; }
        public string CompletionQualifications { get; set; }

        public string ProfessionalRegistration
        {
            get => string.IsNullOrEmpty(_professionalRegistration) ? "None specified." : _professionalRegistration;
            set => _professionalRegistration = value;
        }

        public int MaxFunding { get; set; }
        public string FundingCap => MaxFunding.ToString("C0", new CultureInfo("en-GB"));
    }
}
