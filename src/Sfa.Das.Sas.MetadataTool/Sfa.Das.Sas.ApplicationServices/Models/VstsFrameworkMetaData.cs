using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class VstsFrameworkMetaData
    {
        public string Id { get; set; }

        public string FrameworkName { get; set; }

        public int FrameworkCode { get; set; }

        public int ProgType { get; set; }

        public string Pathway { get; set; }

        public int PathwayCode { get; set; }

        public IEnumerable<JobRoleItem> JobRoleItems { get; set; }

        public IEnumerable<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string EntryRequirements { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string CompletionQualifications { get; set; }

        public string FrameworkOverview { get; set; }
    }
}
