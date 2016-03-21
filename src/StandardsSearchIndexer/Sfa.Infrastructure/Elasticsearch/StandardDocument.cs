namespace Sfa.Infrastructure.Elasticsearch
{
    using System.Collections.Generic;

    using Nest;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Core.Models;

    public class StandardDocument : IIndexEntry
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> JobRoles { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfFileName { get; set; }

        public string StandardPdf { get; set; }

        public string AssessmentPlanPdf { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string IntroductoryText { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string OverviewOfRole { get; set; }
    }
}