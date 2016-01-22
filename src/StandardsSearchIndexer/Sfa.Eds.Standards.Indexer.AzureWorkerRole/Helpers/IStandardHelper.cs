using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public interface IStandardHelper
    {
        string GetIndexAlias();
        string GetIndexNameAndDateExtension(DateTime dateTime);
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        void CreateDocumentIndex(string indexName);
        Task IndexStandards(DateTime scheduledRefreshDateTime);
        Task UploadStandardsContentToAzure(List<JsonMetadataObject> standardList);
        Task UploadStandardJson(JsonMetadataObject standard);
        Task UploadStandardPdf(JsonMetadataObject standard);
        Task<List<JsonMetadataObject>> GetStandardsFromAzureAsync();
        Task IndexStandardPdfs(string indexName, List<JsonMetadataObject> standards);
        Task<StandardDocument> CreateDocument(JsonMetadataObject standard);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated();
    }
}