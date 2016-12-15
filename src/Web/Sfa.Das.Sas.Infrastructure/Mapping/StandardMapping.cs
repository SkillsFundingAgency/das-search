using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using ApiStandard = SFA.DAS.Apprenticeships.Api.Types.Standard;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public class StandardMapping : IStandardMapping
    {
        private readonly ITypicalLengthMapping _typicalLengthMapping;

        public StandardMapping(ITypicalLengthMapping typicalLengthMapping)
        {
            _typicalLengthMapping = typicalLengthMapping;
        }

        public Standard MapToStandard(ApiStandard document)
        {
            return new Standard
            {
                StandardId = document.StandardId,
                Title = document.Title,
                StandardPdf = document.StandardPdf,
                AssessmentPlanPdf = document.AssessmentPlanPdf,
                Level = document.Level,
                //IsPublished = document.Published, Api Client doesn't return this property yet
                JobRoles = document.JobRoles.ToList(),
                Keywords = document.Keywords.ToList(),
                TypicalLength = _typicalLengthMapping.MapTypicalLength(document.TypicalLength),
                IntroductoryText = document.IntroductoryText,
                EntryRequirements = document.EntryRequirements,
                WhatApprenticesWillLearn = document.WhatApprenticesWillLearn,
                Qualifications = document.Qualifications,
                ProfessionalRegistration = document.ProfessionalRegistration,
                OverviewOfRole = document.OverviewOfRole
            };
        }

        public Standard MapToStandard(StandardSearchResultsItem document)
        {
            return new Standard
            {
                StandardId = document.StandardId,
                Title = document.Title,
                StandardPdf = document.StandardPdf,
                AssessmentPlanPdf = document.AssessmentPlanPdf,
                Level = document.Level,
                IsPublished = document.Published,
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
    }
}
