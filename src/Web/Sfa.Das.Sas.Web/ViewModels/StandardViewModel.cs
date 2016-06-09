namespace Sfa.Das.Sas.Web.ViewModels
{
    public class StandardViewModel
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        public string StandardPdf { get; set; }

        public string AssessmentPlanPdf { get; set; }

        public string TypicalLengthMessage { get; set; }

        public string IntroductoryText { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string OverviewOfRole { get; set; }

        // Page specific
        public LinkViewModel PreviousPageLink { get; set; }

        public string StandardPdfUrlTitle { get; set; }

        public string AssessmentPlanPdfUrlTitle { get; set; }

        public bool IsShortlisted { get; set; }

        public string SearchTerm { get; set; }
    }
}