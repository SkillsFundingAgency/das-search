namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System;
    using System.Linq;

    using Core.Models;
    using Models;

    using Sfa.Das.Sas.ApplicationServices.Models;

    internal class MongoMappingService
    {
        internal StandardMetaData MapToCoreModel(MongoStandard arg)
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

        internal FrameworkMetaData MapToCoreModel(MongoFramework arg)
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
                NasTitle = arg.NasTitle,
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

        internal MongoStandard MapFromCoreModel(StandardMetaData arg)
        {
            return new MongoStandard
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

        internal MongoFramework MapFromCoreModel(FrameworkMetaData arg)
        {
            return new MongoFramework
            {
                Id = arg.Id,
                FrameworkCode = arg.FrameworkCode,
                FrameworkName = arg.FrameworkName,
                ProgType = arg.ProgType,
                PathwayCode = arg.PathwayCode,
                Pathway = arg.Pathway,
                NasTitle = arg.NasTitle,
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

        internal MongoStandard MapFromVstsModel(VstsStandardMetaData arg)
        {
            return new MongoStandard
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
                EntryRequirements = arg.EntryRequirements,
                WhatApprenticesWillLearn = arg.WhatApprenticesWillLearn,
                Qualifications = arg.Qualifications,
                ProfessionalRegistration = arg.ProfessionalRegistration,
                OverviewOfRole = arg.OverviewOfRole,
            };
        }

        internal MongoFramework MapFromVstsModel(VstsFrameworkMetaData arg)
        {
            return new MongoFramework
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

        private int ToInt(string str)
        {
            int v;
            int.TryParse(str, out v);
            return v;
        }

        private MongoTypicalLength MapTypicalLength(TypicalLength arg)
        {
            return new MongoTypicalLength { From = arg.From, To = arg.To, Unit = arg.Unit };
        }

        private TypicalLength MapTypicalLength(MongoTypicalLength arg)
        {
            return new TypicalLength { From = arg.From, To = arg.To, Unit = arg.Unit };
        }

        private JobRoleItem MapJobRoleItems(MongoJobRoleItem arg)
        {
            return new JobRoleItem { Title = arg.Title, Description = arg.Description };
        }

        private MongoJobRoleItem MapJobRoleItems(JobRoleItem arg)
        {
            return new MongoJobRoleItem { Title = arg.Title, Description = arg.Description };
        }
    }
}