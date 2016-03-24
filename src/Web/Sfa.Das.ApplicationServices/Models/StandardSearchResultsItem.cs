using System.Collections.Generic;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Das.ApplicationServices.Models
{
    public sealed class StandardSearchResultsItem
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

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