using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.Mapping
{
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
                NotionalEndLevel = document.Level,
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
