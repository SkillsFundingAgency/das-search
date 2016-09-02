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
    using Sfa.Das.Sas.ApplicationServices.Services.Models;
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

        public MapperResponse Import(string text, string type)
        {
            var message = string.Empty;
            var innerMessage = string.Empty;

            if (!text.StartsWith("["))
            {
                return new MapperResponse { Data = text, Message = "Input needs to be a list" };
            }

            if (type.Equals("framework"))
            {
                try
                {
                    var entries = JsonConvert.DeserializeObject<List<MongoFramework>>(text);
                    if (entries != null && entries.Count > 0)
                    {
                        ImportDocuments(entries, _mongoSettings.CollectionNameFrameworks);
                        message = $"Imported {entries.Count} framework";
                    }
                }
                catch (Exception exception)
                {
                    message = $"Not possible to parse json to {type}";
                    innerMessage = exception.Message;
                }
            }

            if (type.Equals("standard"))
            {
                try
                {

                    var entries = JsonConvert.DeserializeObject<List<MongoStandard>>(text);
                
                    if (entries != null && entries.Count > 0)
                    {
                        ImportDocuments(entries, _mongoSettings.CollectionNameStandards);
                        message = $"Imported {entries.Count} standards";
                    }
                }
                catch (Exception exception)
                {
                    message = $"Not possible to parse json to {type}";
                    innerMessage = exception.Message;
                }
            }

            if (type.Equals("vstsframework"))
            {            
                try
                {
                    var entries = JsonConvert.DeserializeObject<List<VstsFrameworkMetaData>>(text);
                    var result = entries.Select(_mappingService.MapFromVstsModel).ToList();

                    if (result.Count > 0)
                    {
                        ImportDocuments(result, _mongoSettings.CollectionNameFrameworks);
                        message = $"Imported {result.Count} framework";
                    }
                }
                catch (Exception exception)
                {
                    message = $"Not possible to parse json to {type}";
                    innerMessage = exception.Message;
                }
            }

            if (type.Equals("vstsstandard"))
            {
                try
                {
                    var entries = JsonConvert.DeserializeObject<List<VstsStandardMetaData>>(text);

                    var result = entries.Select(_mappingService.MapFromVstsModel).ToList();

                    if (result.Count > 0)
                    {
                        ImportDocuments(result, _mongoSettings.CollectionNameStandards);
                        message = $"Imported {result.Count} standards";
                    }
                }
                catch (Exception exception)
                {
                    message = $"Not possible to parse json to {type}";
                    innerMessage = exception.Message;
                }
            }

            return new MapperResponse { Data = text, Message = message, InnerMessage = innerMessage};

        }
    }
}