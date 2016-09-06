namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System;
    using System.Linq;

    using Core.Models;
    using Models;

    using Sfa.Das.Sas.ApplicationServices.Models;

    internal class MongoMappingService
    {
        internal StandardMetaData MapToCoreModel(Standard arg)
        {
            return new StandardMetaData
            {
                Id = arg.Id,
                ApprenticeshipId = arg.ApprenticeshipId,
                Title = arg.Title,
                JobRoles = arg.JobRoles,
                Keywords = arg.Keywords,
                NotionalEndLevel = arg.NotionalEndLevel,
                StandardPdfUrl = arg.StandardPdfUrl,
                AssessmentPlanPdfUrl = arg.AssessmentPlanPdfUrl,
                TypicalLength = MapTypicalLength(arg.TypicalLength),
                EntryRequirements = arg.EntryRequirements,
                WhatApprenticesWillLearn = arg.WhatApprenticesWillLearn,
                Qualifications = arg.Qualifications,
                ProfessionalRegistration = arg.ProfessionalRegistration,
                OverviewOfRole = arg.OverviewOfRole,
                SectorSubjectAreaTier1 = arg.SectorSubjectAreaTier1,
                SectorSubjectAreaTier2 = arg.SectorSubjectAreaTier2
            };
        }

        internal FrameworkMetaData MapToCoreModel(Framework arg)
        {
            return new FrameworkMetaData
            {
                Id = arg.Id,
                ApprenticeshipId = arg.ApprenticeshipId,
                FrameworkCode = arg.FrameworkCode,
                FrameworkName = arg.FrameworkName,
                ProgType = arg.ProgType,
                PathwayCode = arg.PathwayCode,
                Pathway = arg.Pathway,
                EffectiveFrom = arg.EffectiveFrom,
                EffectiveTo = arg.EffectiveTo,
                JobRoleItems = arg.JobRoleItems.Select(MapJobRoleItems),
                Keywords = arg.Keywords,
                TypicalLength = MapTypicalLength(arg.TypicalLength),
                SectorSubjectAreaTier1 = arg.SectorSubjectAreaTier1,
                SectorSubjectAreaTier2 = arg.SectorSubjectAreaTier2,
                EntryRequirements = arg.EntryRequirements,
                ProfessionalRegistration = arg.ProfessionalRegistration,
                CompletionQualifications = arg.CompletionQualifications,
                FrameworkOverview = arg.FrameworkOverview,
                CompetencyQualification = arg.CompetencyQualification,
                KnowledgeQualification = arg.KnowledgeQualification,
                CombinedQualification = arg.CombinedQualification
            };
        }

        internal Standard MapFromCoreModel(StandardMetaData arg)
        {
            return new Standard
            {
                Id = arg.Id,
                Title = arg.Title,
                JobRoles = arg.JobRoles,
                Keywords = arg.Keywords,
                NotionalEndLevel = arg.NotionalEndLevel,
                StandardPdfUrl = arg.StandardPdfUrl,
                AssessmentPlanPdfUrl = arg.AssessmentPlanPdfUrl,
                TypicalLength = MapTypicalLength(arg.TypicalLength),
                EntryRequirements = arg.EntryRequirements,
                WhatApprenticesWillLearn = arg.WhatApprenticesWillLearn,
                Qualifications = arg.Qualifications,
                ProfessionalRegistration = arg.ProfessionalRegistration,
                OverviewOfRole = arg.OverviewOfRole,
                SectorSubjectAreaTier1 = arg.SectorSubjectAreaTier1,
                SectorSubjectAreaTier2 = arg.SectorSubjectAreaTier2
            };
        }

        internal Framework MapFromCoreModel(FrameworkMetaData arg)
        {
            return new Framework
            {
                Id = arg.Id,
                FrameworkCode = arg.FrameworkCode,
                FrameworkName = arg.FrameworkName,
                ProgType = arg.ProgType,
                PathwayCode = arg.PathwayCode,
                Pathway = arg.Pathway,
                EffectiveFrom = arg.EffectiveFrom,
                EffectiveTo = arg.EffectiveTo,
                JobRoleItems = arg.JobRoleItems?.Select(MapJobRoleItems),
                Keywords = arg.Keywords,
                TypicalLength = MapTypicalLength(arg.TypicalLength),
                SectorSubjectAreaTier1 = arg.SectorSubjectAreaTier1,
                SectorSubjectAreaTier2 = arg.SectorSubjectAreaTier2,
                EntryRequirements = arg.EntryRequirements,
                ProfessionalRegistration = arg.ProfessionalRegistration,
                CompletionQualifications = arg.CompletionQualifications,
                FrameworkOverview = arg.FrameworkOverview,
                CompetencyQualification = arg.CompetencyQualification,
                KnowledgeQualification = arg.KnowledgeQualification,
                CombinedQualification = arg.CombinedQualification
            };
        }

        internal Standard MapFromVstsModel(VstsStandardMetaData arg)
        {
            return new Standard
            {
                Id = Guid.NewGuid(),
                ApprenticeshipId = arg.Id,
                Title = arg.Title,
                JobRoles = arg.JobRoles,
                Keywords = arg.Keywords,
                NotionalEndLevel = arg.NotionalEndLevel,
                StandardPdfUrl = arg.StandardPdfUrl,
                AssessmentPlanPdfUrl = arg.AssessmentPlanPdfUrl,
                TypicalLength = MapTypicalLength(arg.TypicalLength),
                EntryRequirements = MarkdownToHtml(arg.EntryRequirements),
                WhatApprenticesWillLearn = MarkdownToHtml(arg.WhatApprenticesWillLearn),
                Qualifications = MarkdownToHtml(arg.Qualifications),
                ProfessionalRegistration = MarkdownToHtml(arg.ProfessionalRegistration),
                OverviewOfRole = MarkdownToHtml(arg.OverviewOfRole),
            };
        }

        internal Framework MapFromVstsModel(VstsFrameworkMetaData arg)
        {
            return new Framework
            {
                Id =  new Guid(),
                ApprenticeshipId = ToInt(arg.Id),
                FrameworkCode = arg.FrameworkCode,
                FrameworkName = arg.FrameworkName,
                ProgType = arg.ProgType,
                PathwayCode = arg.PathwayCode,
                Pathway = arg.Pathway,
                JobRoleItems = arg.JobRoleItems.Select(MapJobRoleItems),
                Keywords = arg.Keywords,
                TypicalLength = MapTypicalLength(arg.TypicalLength),
                EntryRequirements = arg.EntryRequirements,
                ProfessionalRegistration = arg.ProfessionalRegistration,
                CompletionQualifications = arg.CompletionQualifications,
                FrameworkOverview = arg.FrameworkOverview
            };
        }

        public JobRoleItem MapJobRoleItems(MongoJobRoleItem arg)
        {
            return new JobRoleItem { Title = arg.Title, Description = arg.Description };
        }

        public MongoJobRoleItem MapJobRoleItems(JobRoleItem arg)
        {
            return new MongoJobRoleItem { Title = arg.Title, Description = arg.Description };
        }

        private int ToInt(string str)
        {
            int v;
            int.TryParse(str, out v);
            return v;
        }

        private MongoTypicalLength MapTypicalLength(TypicalLength arg)
        {
            if (arg == null)
            {
                return new MongoTypicalLength();
            }

            return new MongoTypicalLength { From = arg.From, To = arg.To, Unit = arg.Unit };
        }

        private TypicalLength MapTypicalLength(MongoTypicalLength arg)
        {
            return new TypicalLength { From = arg.From, To = arg.To, Unit = arg.Unit };
        }

        private string MarkdownToHtml(string markdown)
        {
            if (!string.IsNullOrEmpty(markdown))
            {
                return CommonMark.CommonMarkConverter.Convert(markdown.Replace("\\r", "\r").Replace("\\n", "\n"));
            }
            return string.Empty;

        }
    }
}