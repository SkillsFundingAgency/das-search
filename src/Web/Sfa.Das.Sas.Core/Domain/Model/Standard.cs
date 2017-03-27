using System.Collections.Generic;

namespace Sfa.Das.Sas.Core.Domain.Model
{
    public sealed class Standard : IApprenticeshipProduct
    {
        public string StandardId { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public int MaxFunding { get; set; }

        public bool IsPublished { get; set; }

        public string StandardPdf { get; set; }

        public string AssessmentPlanPdf { get; set; }

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
}
