using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System;
    using System.Linq;

    using ApplicationServices.Services.Interfaces;

    using Newtonsoft.Json;

    using Models;

    using ApplicationServices.Models;
    using ApplicationServices.Services.Models;

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

        public MapperResponse Import(string text, string type)
        {
            var typeStatus = GetImportType(text, type);
            switch (typeStatus)
            {
                case MongoImportType.Nothing:
                    return new MapperResponse { Data = text, Message = "Input not valid" };
                case MongoImportType.Framework:
                    return ImportData(
                        text,
                        EnsureFramework,
                        _mongoSettings.CollectionNameFrameworks);
                case MongoImportType.Standard:
                    return ImportData(
                        text,
                        EnsureStandard,
                        _mongoSettings.CollectionNameStandards);
                case MongoImportType.VstsFramework:
                    return ImportFromVsts<VstsFrameworkMetaData, Framework>(
                        text,
                        _mappingService.MapFromVstsModel,
                        EnsureFramework,
                        _mongoSettings.CollectionNameFrameworks);
                case MongoImportType.VstsStandard:
                    return ImportFromVsts<VstsStandardMetaData, Standard>(
                        text,
                        _mappingService.MapFromVstsModel,
                        EnsureStandard,
                        _mongoSettings.CollectionNameStandards);
            }
            return new MapperResponse { Data = text, Message = string.Empty, InnerMessage = string.Empty };
        }

        private static Func<Framework, bool> EnsureFramework => x => !string.IsNullOrEmpty(x.FrameworkName);
        private static Func<Standard, bool> EnsureStandard => x => !string.IsNullOrEmpty(x.Title);


        private MapperResponse ImportData<T>(string text, Func<T, bool> ensureType, string collectionName)
        {
            Func<List<T>> f = () => JsonConvert.DeserializeObject<List<T>>(text).Where(ensureType).ToList();

            var d = ImportDocuments(f, collectionName);
            return new MapperResponse { Data = text, InnerMessage = d.InnerMessage, Message = d.Message };
        }

        private MapperResponse ImportFromVsts<T, T2>(string text, Func<T, T2> mapFromVstsModel, Func<T2, bool> ensureType, string collectionName)
        {
            Func<List<T2>> f = () => JsonConvert.DeserializeObject<List<T>>(text)
                .Select(mapFromVstsModel)
                .Where(ensureType).ToList();

            var d = ImportDocuments(f, collectionName);
            return new MapperResponse { Data = text, InnerMessage = d.InnerMessage, Message = d.Message };
        }

        private MapperResponse ImportDocuments<T>(Func<List<T>> func, string collectionName)
        {
            var message = string.Empty;
            var innerMessage = string.Empty;
            try
            {
                var entries = func.Invoke();

                if (entries.Count > 0)
                {
                    ImportDocuments(entries, collectionName);
                    message = $"Imported {entries.Count} {typeof(T).Name}";
                }
            }
            catch (Exception exception)
            {
                message = $"Not possible to parse json to {typeof(T).Name}";
                innerMessage = exception.Message;
            }

            return new MapperResponse { Data = string.Empty, Message = message, InnerMessage = innerMessage };
        }
        private MongoImportType GetImportType(string text, string type)
        {
            if (!text.StartsWith("["))
            {
                return MongoImportType.Nothing;
            }
            MongoImportType t;
            return Enum.TryParse(type, true, out t) ? t : t;
        }

        private void ImportDocuments<T>(IEnumerable<T> documents, string collectionName)
        {
            _mongoDataClient.Insert(documents, collectionName);
        }
    }
}