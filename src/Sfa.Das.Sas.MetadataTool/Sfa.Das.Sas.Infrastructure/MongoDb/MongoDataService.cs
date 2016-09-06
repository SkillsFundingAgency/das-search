namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ApplicationServices.Services.Interfaces;
    using Core.Models;

    using Models;

    using MongoDB.Driver;

    public class MongoDataService : IMetaDataService
    {
        private readonly IMongoDataClient _mongoDataClient;

        private readonly IMongoSettings _mongoSettings;

        private readonly MongoMappingService _mappingService;

        public MongoDataService(
            IMongoDataClient mongoDataClient,
            IMongoSettings mongoSettings)
        {
            _mongoDataClient = mongoDataClient;
            _mongoSettings = mongoSettings;
            _mappingService = new MongoMappingService();
        }

        public IEnumerable<StandardMetaData> GetStandards()
        {
            var models = _mongoDataClient.GetAll<Standard>(_mongoSettings.CollectionNameStandards);

            return models.Select(_mappingService.MapToCoreModel);
        }

        public StandardMetaData GetStandard(string id)
        {
            var model = _mongoDataClient.GetById<Standard>(_mongoSettings.CollectionNameStandards, id);
            return _mappingService.MapToCoreModel(model);
        }

        public IEnumerable<FrameworkMetaData> GetFrameworks()
        {
            var models = _mongoDataClient.GetAll<Framework>(_mongoSettings.CollectionNameFrameworks);
            return models.Select(_mappingService.MapToCoreModel);
        }

        public FrameworkMetaData GetFramework(string id)
        {
            var model = _mongoDataClient.GetById<Framework>(_mongoSettings.CollectionNameFrameworks, id);
            return _mappingService.MapToCoreModel(model);
        }

        public void UpdateFramework(FrameworkMetaData updateModel)
        {
            var model = _mongoDataClient.GetById<Framework>(_mongoSettings.CollectionNameFrameworks, updateModel.Id.ToString());

            _mongoDataClient.Save(
                model,
                Builders<Framework>.Filter.Eq(x => x.Id, model.Id),
                Builders<Framework>.Update
                    .Set(x => x.EffectiveFrom, updateModel.EffectiveFrom)
                    .Set(x => x.EffectiveTo, updateModel.EffectiveTo)
                    .Set(x => x.Keywords, updateModel.Keywords)
                    .Set(x => x.JobRoleItems, updateModel.JobRoleItems.Select(_mappingService.MapJobRoleItems))
                    .Set(x => x.FrameworkOverview, updateModel.FrameworkOverview)
                    .Set(x => x.EntryRequirements, updateModel.EntryRequirements)
                    .Set(x => x.ProfessionalRegistration, updateModel.ProfessionalRegistration)
                    .Set(x => x.CompletionQualifications, updateModel.CompletionQualifications)
                    .Set(x => x.CompetencyQualification, updateModel.CompetencyQualification)
                    .Set(x => x.KnowledgeQualification, updateModel.KnowledgeQualification)
                    .Set(x => x.CombinedQualification, updateModel.CombinedQualification)
                    ,
                _mongoSettings.CollectionNameFrameworks);
        }

        public void UpdateStandard(StandardMetaData updateModel)
        {
            var model = _mongoDataClient.GetById<Standard>(_mongoSettings.CollectionNameStandards, updateModel.Id.ToString());

            var updatedTypicalLength = new MongoTypicalLength
            {
                From = updateModel.TypicalLength.From,
                To = updateModel.TypicalLength.To,
                Unit = updateModel.TypicalLength.Unit
            };

            _mongoDataClient.Save(
                model,
                Builders<Standard>.Filter.Eq(x => x.Id, model.Id),
                Builders<Standard>.Update
                    .Set(x => x.AssessmentPlanPdfUrl, updateModel.AssessmentPlanPdfUrl)
                    .Set(x => x.EntryRequirements, updateModel.EntryRequirements)
                    .Set(x => x.OverviewOfRole, updateModel.OverviewOfRole)
                    .Set(x => x.ProfessionalRegistration, updateModel.ProfessionalRegistration)
                    .Set(x => x.Qualifications, updateModel.Qualifications)
                    .Set(x => x.StandardPdfUrl, updateModel.StandardPdfUrl)
                    .Set(x => x.Title, updateModel.Title)
                    .Set(x => x.WhatApprenticesWillLearn, updateModel.WhatApprenticesWillLearn)
                    .Set(x => x.JobRoles, updateModel.JobRoles)
                    .Set(x => x.Keywords, updateModel.Keywords)
                    .Set(x => x.NotionalEndLevel, updateModel.NotionalEndLevel)
                    .Set(x => x.SectorSubjectAreaTier1, updateModel.SectorSubjectAreaTier1)
                    .Set(x => x.SectorSubjectAreaTier2, updateModel.SectorSubjectAreaTier2)
                    .Set(x => x.TypicalLength, updatedTypicalLength),
                _mongoSettings.CollectionNameStandards);
        }
    }
}
