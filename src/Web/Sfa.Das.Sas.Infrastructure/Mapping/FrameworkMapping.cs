namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Domain.Model;
    using ApiFramework = SFA.DAS.Apprenticeships.Api.Types.Framework;
    using ApiJobRoleItem = SFA.DAS.Apprenticeships.Api.Types.JobRoleItem;

    public class FrameworkMapping : IFrameworkMapping
    {
        private readonly ITypicalLengthMapping _typicalLengthMapping;

        public FrameworkMapping(ITypicalLengthMapping typicalLengthMapping)
        {
            _typicalLengthMapping = typicalLengthMapping;
        }

        public Framework MapToFramework(FrameworkSearchResultsItem document)
        {
            var framework = new Framework
            {
                Title = document.Title,
                Level = document.Level,
                FrameworkCode = document.FrameworkCode,
                FrameworkId = document.FrameworkId,
                FrameworkName = document.FrameworkName,
                PathwayCode = document.PathwayCode,
                PathwayName = document.PathwayName,
                TypicalLength = document.TypicalLength,
                ExpiryDate = document.ExpiryDate,
                JobRoleItems = document.JobRoleItems,
                CompletionQualifications = document.CompletionQualifications,
                FrameworkOverview = document.FrameworkOverview,
                EntryRequirements = document.EntryRequirements,
                ProfessionalRegistration = document.ProfessionalRegistration,
                CompetencyQualification = document.CompetencyQualification?.OrderBy(x => x),
                KnowledgeQualification = document.KnowledgeQualification?.OrderBy(x => x),
                CombinedQualification = document.CombinedQualification?.OrderBy(x => x)
            };

            return framework;
        }

        public Framework MapToFramework(ApiFramework document)
        {
            return new Framework
            {
                Title = document.Title,
                Level = document.Level,
                FrameworkCode = document.FrameworkCode,
                FrameworkId = document.FrameworkId,
                FrameworkName = document.FrameworkName,
                PathwayCode = document.PathwayCode,
                PathwayName = document.PathwayName,
                TypicalLength = _typicalLengthMapping.MapTypicalLength(document.TypicalLength),
                ExpiryDate = document.ExpiryDate,
                JobRoleItems = MapJobRoleItems(document.JobRoleItems),
                CompletionQualifications = document.CompletionQualifications,
                FrameworkOverview = document.FrameworkOverview,
                EntryRequirements = document.EntryRequirements,
                ProfessionalRegistration = document.ProfessionalRegistration,
                CompetencyQualification = document.CompetencyQualification?.OrderBy(x => x),
                KnowledgeQualification = document.KnowledgeQualification?.OrderBy(x => x),
                CombinedQualification = document.CombinedQualification?.OrderBy(x => x)
            };
        }

        private IEnumerable<JobRoleItem> MapJobRoleItems(IEnumerable<ApiJobRoleItem> jobRoleItems)
        {
            foreach (var jobRoleItem in jobRoleItems)
            {
                yield return new JobRoleItem
                {
                    Description = jobRoleItem.Description,
                    Title = jobRoleItem.Title
                };
            }
        }
    }
}
