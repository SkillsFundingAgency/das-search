namespace Sfa.Das.Sas.Web.ViewModels
{
    using System.Collections.Generic;

    public class FrameworkViewModel
    {
        public string FrameworkId { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public int MaxFunding { get; set; }

        public int Duration { get; set; }

        public string ExpiryDateString { get; set; }

        public string SearchTerm { get; set; }

        public string CompletionQualifications { get; set; }

        public string FrameworkOverview { get; set; }

        public string EntryRequirements { get; set; }

        public string ProfessionalRegistration { get; set; }

        public IEnumerable<string> JobRoles { get; set; }

        public IEnumerable<string> CompetencyQualification { get; set; }

        public IEnumerable<string> KnowledgeQualification { get; set; }

        public IEnumerable<string> CombinedQualificiation { get; set; }
    }
}