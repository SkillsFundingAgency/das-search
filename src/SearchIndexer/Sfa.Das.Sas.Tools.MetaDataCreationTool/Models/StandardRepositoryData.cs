namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Models
{
    using System.Collections.Generic;

    using Sfa.Das.Sas.Indexer.Core.Models;

    public class StandardRepositoryData
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string OverviewOfRole { get; set; }

        public List<string> JobRoles { get; set; }

        public List<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }

        public string ProfessionalRegistration { get; set; }

        public bool Published { get; set; }
    }
}
