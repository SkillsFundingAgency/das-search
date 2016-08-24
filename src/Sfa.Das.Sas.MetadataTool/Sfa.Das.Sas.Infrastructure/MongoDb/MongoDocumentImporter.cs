using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System;
    using System.Linq;

    using ApplicationServices.Services.Interfaces;

    using Newtonsoft.Json;

    using Models;

    using MongoDB.Bson;

    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.ApplicationServices.Services;
    using Sfa.Das.Sas.Core.Models;

    public class MongoDocumentImporter : IDocumentImporter
    {
        private readonly IMongoDataClient _mongoDataClient;

        private readonly IMongoSettings _mongoSettings;

        private readonly MongoMappingService _mappingService;

        public MongoDocumentImporter(
            IMongoDataClient mongoDataClient,
            IMongoSettings mongoSettings)
        {
            _mongoDataClient = mongoDataClient;
            _mongoSettings = mongoSettings;
            _mappingService = new MongoMappingService();
        }

        public void ImportDocuments<T>(IEnumerable<T> documents, string collectionName)
        {
            _mongoDataClient.Insert(documents, collectionName);
        }

        public string Import(string text, string type)
        {
            if (type.Equals("framework"))
            {
                var entries = JsonConvert.DeserializeObject<List<MongoFramework>>(text);

                if (entries != null && entries.Count > 0)
                {
                    ImportDocuments(entries, _mongoSettings.CollectionNameFrameworks);
                    return $"Imported {entries.Count} framework";
                }
            }

            if (type.Equals("standard"))
            {
                var entries = JsonConvert.DeserializeObject<List<MongoStandard>>(text);
                
                if (entries != null && entries.Count > 0)
                {
                    ImportDocuments(entries, _mongoSettings.CollectionNameStandards);
                    return $"Imported {entries.Count} standards";
                }
            }

            if (type.Equals("vstsframework"))
            {
                var entries = JsonConvert.DeserializeObject<List<VstsFrameworkMetaData>>(text);

                var result = entries.Select(_mappingService.MapFromVstsModel).ToList();

                if (result.Count > 0)
                {
                    ImportDocuments(result, _mongoSettings.CollectionNameFrameworks);
                    return $"Imported {result.Count} framework";
                }
            }

            if (type.Equals("vstsstandard"))
            {
                var entries = JsonConvert.DeserializeObject<List<VstsStandardMetaData>>(text);

                var result = entries.Select(_mappingService.MapFromVstsModel).ToList();

                if (result.Count > 0)
                {
                    ImportDocuments(result, _mongoSettings.CollectionNameStandards);
                    return $"Imported {result.Count} standards";
                }
            }

            return "Error importing";
            
        }
    }
}