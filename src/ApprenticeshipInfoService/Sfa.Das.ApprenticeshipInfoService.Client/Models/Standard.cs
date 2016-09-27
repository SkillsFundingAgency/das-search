using System.Collections.Generic;

namespace Sfa.Das.ApprenticeshipInfoService.Client.Models
{
    public sealed class Standard
    {
        public int StandardId { get; set; }

        public string Uri { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public string StandardPdf { get; set; }

        public string AssessmentPlanPdf { get; set; }

        public IEnumerable<string> JobRoles { get; set; }

        public IEnumerable<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string IntroductoryText { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string OverviewOfRole { get; set; }
    }
}
