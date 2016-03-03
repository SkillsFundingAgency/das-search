namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Models
{
    using System;
    using System.Collections.Generic;

    public sealed class Standard
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        [Obsolete]
        public string PdfFileName { get; set; }

        public Uri StandardPdf { get; set; }

        public Uri AssessmentPlanPdf { get; set; }

        public List<string> JobRoles { get; set; }

        public List<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string IntroductoryText { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }
        public string ProfessionalRegistration { get; set; }

        public string OverviewOfRole { get; set; }
    }

    public class TypicalLength
    {
        public int From { get; set; }

        public int  To { get; set; }

        public string Unit { get; set; }
    }
}
