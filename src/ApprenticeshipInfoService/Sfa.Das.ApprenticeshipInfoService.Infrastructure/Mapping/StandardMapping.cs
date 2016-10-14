using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public class StandardMapping : IStandardMapping
    {
        public Standard MapToStandard(StandardSearchResultsItem document)
        {
            return new Standard
            {
                StandardId = document.StandardId,
                Title = document.Title,
                StandardPdf = document.StandardPdf,
                AssessmentPlanPdf = document.AssessmentPlanPdf,
                Level = document.Level,
                JobRoles = document.JobRoles,
                Keywords = document.Keywords,
                TypicalLength = document.TypicalLength,
                IntroductoryText = document.IntroductoryText,
                EntryRequirements = document.EntryRequirements,
                WhatApprenticesWillLearn = document.WhatApprenticesWillLearn,
                Qualifications = document.Qualifications,
                ProfessionalRegistration = document.ProfessionalRegistration,
                OverviewOfRole = document.OverviewOfRole
            };
        }

        public StandardSummary MapToStandardSummary(StandardSearchResultsItem document)
        {
            return new StandardSummary
            {
                Id = document.StandardId,
                Title = document.Title,
                Level = document.Level,
                TypicalLength = document.TypicalLength,
            };
        }
    }
}
