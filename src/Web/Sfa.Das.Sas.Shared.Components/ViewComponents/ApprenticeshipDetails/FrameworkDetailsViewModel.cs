using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sfa.Das.Sas.Resources;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails
{
    public class FrameworkDetailsViewModel : ApprenticeshipDetailBaseClass
    {  
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> CompetencyQualification{ get; set; }
        public IEnumerable<string> KnowledgeQualification { get; set; }
        public IEnumerable<string> CombinedQualification { get; set; }
        public string CompletionQualifications { get; set; }
    }
}
