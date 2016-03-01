namespace Sfa.Eds.Das.Web.ViewModels
{
    public class StandardViewModel
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        public string StandardPdfUrl { get; set; }

        public string AssessmentPlanPdfUrl { get; set; }

        public string TypicalLengthMessage { get; set; }

        public string IntroductoryTextHtml { get; set; }

        public string EntryRequirementsHtml { get; set; }

        public string WhatApprenticesWillLearnHtml { get; set; }

        public string QualificationsHtml { get; set; }

        public string ProfessionalRegistrationHtml { get; set; }

        public string OverviewOfRoleHtml { get; set; }

        // Page specific
        public bool HasError { get; set; }

        public LinkViewModel SearchResultLink { get; set; }

        public string StandardPdfUrlTitle { get; set; }

        public string AssessmentPlanPdfUrlTitle { get; set; }
    }
}