using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using ApplicationServices.Services.Interfaces;

    public class MongoDocumentImporter : IDocumentImporter
    {
        private readonly IMongoDataClient _mongoDataClient;

        public MongoDocumentImporter(IMongoDataClient mongoDataClient)
        {
            _mongoDataClient = mongoDataClient;
        }

        public void ImportDocuments<T>(IEnumerable<T> documents, string collectionName)
        {
            _mongoDataClient.Insert(documents, collectionName);
        }
    }
}