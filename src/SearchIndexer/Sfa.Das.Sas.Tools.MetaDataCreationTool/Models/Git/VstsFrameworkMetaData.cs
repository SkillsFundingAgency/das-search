namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git
{
    using System.Collections.Generic;

    using Sfa.Das.Sas.Indexer.Core.Models;

    public class VstsFrameworkMetaData
    {
        public int FrameworkCode { get; set; }

        public int ProgType { get; set; }

        public int PathwayCode { get; set; }

        public IEnumerable<JobRoleItem> JobRoleItems { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string EntryRequirements { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string CompletionQualifications { get; set; }
    }
}