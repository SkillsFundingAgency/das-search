using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class StandardViewModel
    {
        public string StandardId { get; set; }

        public string Title { get; set; }

        public int MaxFunding { get; set; }

        public int Level { get; set; }

        public string StandardPdf { get; set; }

        public string AssessmentPlanPdf { get; set; }

        public int Duration { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string OverviewOfRole { get; set; }

        public string SearchTerm { get; set; }

        public List<Organisation> AssessmentOrganisations { get; set; }

        public string StandardPageUri { get; set; }

        public DateTime? LastDateForNewStarts { get; set; }

        public string DayAfterLastDateForNewStartsFormatted => LastDateForNewStarts?.AddDays(1).ToString("dd MMMM yyyy") ?? string.Empty;
        public string FindApprenticeshipTrainingText { get; set; }
        public DateTime? NextEffectiveFrom { get; set; }
        public int? NextFundingCap { get; set; }
    }
}