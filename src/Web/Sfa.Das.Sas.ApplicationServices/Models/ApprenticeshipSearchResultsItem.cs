using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ApprenticeshipSearchResultsItem
    {
        public string Title { get; set; }

        // Standards
        public string StandardId { get; set; }

        public List<string> JobRoles { get; set; }

        public List<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        // Frameworks
        public string FrameworkId { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public int Level { get; set; }

        public IEnumerable<JobRoleItem> JobRoleItems { get; set; }

        public bool Published { get; set; }
    }
}