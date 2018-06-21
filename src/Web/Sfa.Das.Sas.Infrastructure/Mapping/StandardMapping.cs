﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using ApiStandard = SFA.DAS.Apprenticeships.Api.Types.Standard;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public class StandardMapping : IStandardMapping
    {
        public Standard MapToStandard(ApiStandard document)
        {
            return new Standard
            {
                StandardId = document.StandardId,
                Title = document.Title,
                StandardPdf = document.StandardPdf,
                AssessmentPlanPdf = document.AssessmentPlanPdf,
                Level = document.Level,
                JobRoles = document.JobRoles.ToList(),
                Keywords = document.Keywords.ToList(),
                MaxFunding = document.MaxFunding,
                Duration = document.Duration,
                IntroductoryText = document.IntroductoryText,
                EntryRequirements = document.EntryRequirements,
                WhatApprenticesWillLearn = document.WhatApprenticesWillLearn,
                Qualifications = document.Qualifications,
                ProfessionalRegistration = document.ProfessionalRegistration,
                OverviewOfRole = document.OverviewOfRole,
                EffectiveFrom = document.EffectiveFrom,
                EffectiveTo = document.EffectiveTo,
                IsActiveStandard = document.IsActiveStandard,
                StandardPageUri = document.StandardPageUri,
                LastDateForNewStarts = document.LastDateForNewStarts,
                //FundingPeriods = null //MFCMFCMFC
                FundingPeriods = new List<FundingPeriod> { new FundingPeriod { EffectiveFrom = new DateTime(2018,07,30), FundingCap = 1400} }
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
                Duration = document.Duration,
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
