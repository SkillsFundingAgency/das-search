using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Core.Models
{
    public class FrameworkMetaData
    {
        public string Id { get; set; }

        public string FrameworkName { get; set; }

        public int FrameworkCode { get; set; }

        public int ProgType { get; set; }

        public int PathwayCode { get; set; }

        public string Pathway { get; set; }

        public string NasTitle { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public IEnumerable<JobRoleItem> JobRoleItems { get; set; }

        public IEnumerable<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public double SectorSubjectAreaTier1 { get; set; }

        public double SectorSubjectAreaTier2 { get; set; }

        public string EntryRequirements { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string CompletionQualifications { get; set; }

        public string FrameworkOverview { get; set; }

        public IEnumerable<string> CompetencyQualification { get; set; }

        public IEnumerable<string> KnowledgeQualification { get; set; }

        public IEnumerable<string> CombinedQualification { get; set; }
    }
}
