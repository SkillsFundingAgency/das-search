using System;
using System.Collections.Generic;

namespace SFA.DAS.Apprenticeships.Api.Types
{
    public sealed class Framework
    {
        /// <summary>
        /// A composite framework Id {framework-code}{program-type}{pathway-code}
        /// </summary>
        public string FrameworkId { get; set; }

        /// <summary>
        /// a link to the framework information
        /// </summary>
        public string Uri { get; set; }

        public string Title { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public int Level { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string CompletionQualifications { get; set; }

        public string FrameworkOverview { get; set; }

        public string EntryRequirements { get; set; }

        public string ProfessionalRegistration { get; set; }

        public IEnumerable<JobRoleItem> JobRoleItems { get; set; }

        public IEnumerable<string> CompetencyQualification { get; set; }

        public IEnumerable<string> KnowledgeQualification { get; set; }

        public IEnumerable<string> CombinedQualification { get; set; }
    }
}
