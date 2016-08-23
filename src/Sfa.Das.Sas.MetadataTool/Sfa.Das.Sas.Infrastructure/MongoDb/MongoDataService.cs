namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System.Collections.Generic;
    using System.Linq;

    using ApplicationServices.Services.Interfaces;
    using Core.Models;

    using Models;

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
            var models = _mongoDataClient.GetAll<MongoStandard>(_mongoSettings.CollectionNameStandards);

            return models.Select(_mappingService.MapToCoreModel);
        }

        public StandardMetaData GetStandard(int id)
        {
            var model = _mongoDataClient.GetById<MongoStandard>(_mongoSettings.CollectionNameStandards, id);
            return _mappingService.MapToCoreModel(model);
        }

        public IEnumerable<FrameworkMetaData> GetFrameworks()
        {
            var models = _mongoDataClient.GetAll<MongoFramework>(_mongoSettings.CollectionNameFrameworks);
            return models.Select(_mappingService.MapToCoreModel);
        }

        public FrameworkMetaData GetFramework(int id)
        {
            var model = _mongoDataClient.GetById<MongoFramework>(_mongoSettings.CollectionNameFrameworks, id.ToString());
            return _mappingService.MapToCoreModel(model);
        }
    }
}
