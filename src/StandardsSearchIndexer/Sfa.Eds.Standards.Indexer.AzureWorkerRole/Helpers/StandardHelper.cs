using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public class StandardHelper : IStandardHelper
    {
        private readonly IDedsService _dedsService;
        private readonly IBlobStorageHelper _blobStorageHelper;
        private readonly IStandardIndexSettings _standardIndexSettings;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private IElasticClient _client;
        
        public StandardHelper(
            IDedsService dedsService,
            IBlobStorageHelper blobStorageHelper,
            IStandardIndexSettings standardIndexSettings,
            IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _dedsService = dedsService;
            _blobStorageHelper = blobStorageHelper;
            _standardIndexSettings = standardIndexSettings;
            _elasticsearchClientFactory = elasticsearchClientFactory;

            _client = _elasticsearchClientFactory.GetElasticClient();
        }

        public string GetIndexAlias()
        {
            return _standardIndexSettings.StandardIndexesAlias;
        }

        public string GetIndexNameAndDateExtension(DateTime dateTime)
        {
            return string.Format("{0}-{1}", GetIndexAlias(), dateTime.ToUniversalTime().ToString("yyyy-MM-dd-HH")).ToLower();
        }

        public bool CreateIndex(DateTime scheduledRefreshDateTime)
        {
            var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);
            var indexExistsResponse = _client.IndexExists(indexName);

            // If it already exists and is empty, let's delete it.
            if (indexExistsResponse.Exists)
            {
                var totalResults = _client.Count<StandardDocument>(c =>
                {
                    c.Index(indexName);
                    return c;
                });

                if (totalResults.Count == 0)
                {
                    _client.DeleteIndex(indexName);
                    indexExistsResponse = _client.IndexExists(indexName);
                }
            }

            // create index
            if (!indexExistsResponse.Exists)
            {
                CreateDocumentIndex(indexName);
            }

            return indexExistsResponse.Exists;
        }

        public void CreateDocumentIndex(string indexName)
        {
            _client.CreateIndex(indexName, c => c.AddMapping<StandardDocument>(m => m.MapFromAttributes()));
        }

        public async Task IndexStandards(DateTime scheduledRefreshDateTime)
        {
            var standards = await GetStandardsFromAzureAsync();

            await UploadStandardsContentToAzure(standards);

            try
            {
                await IndexStandardPdfs(GetIndexNameAndDateExtension(scheduledRefreshDateTime), standards);
            }
            catch (Exception e)
            {
                var error = e;
            }
        }

        public async Task UploadStandardsContentToAzure(List<JsonMetadataObject> standardList)
        {
            foreach (var standard in standardList)
            {
                // await UploadStandardJson(standard);
                await UploadStandardPdf(standard);
            }
        }

        public async Task UploadStandardJson(JsonMetadataObject standard)
        {
            await
                _blobStorageHelper.UploadStandardAsync("standardsjson",
                    string.Format(standard.Id.ToString(), ".txt"),
                    JsonConvert.SerializeObject(standard));
        }

        public async Task UploadStandardPdf(JsonMetadataObject standard)
        {
            await _blobStorageHelper.UploadPdfFromUrl("standardspdf", string.Format(standard.Id.ToString(), ".txt"), standard.Pdf);
        }

        public async Task<List<JsonMetadataObject>> GetStandardsFromAzureAsync()
        {
            var standardsList = await _blobStorageHelper.ReadStandardsAsync("standardsjson");

            standardsList = standardsList.OrderBy(s => s.Id).ToList();

            return standardsList;
        }

        public async Task IndexStandardPdfs(string indexName, List<JsonMetadataObject> standards)
        {
            // index the items
            foreach (var standard in standards)
            {
                try
                {
                    var doc = await CreateDocument(standard);

                    // _client.Index(doc);
                    _client
                        .Index(doc, i => i
                            .Index(indexName)
                            .Id(doc.StandardId));
                }
                catch (Exception e)
                {
                    var error = e.Message;
                    throw;
                }
            }
        }

        public async Task<StandardDocument> CreateDocument(JsonMetadataObject standard)
        {
            try
            {
                var attachment = new Attachment
                {
                    Content =
                    Convert.ToBase64String(
                        await _blobStorageHelper.ReadStandardPdfAsync("standardspdf", string.Format(standard.Id.ToString(), ".txt"))),
                    ContentType = "application/pdf",
                    Name = standard.PdfFileName
                };

                var doc = new StandardDocument
                {
                    StandardId = standard.Id,
                    Title = standard.Title,
                    NotionalEndLevel = _dedsService.GetNotationLevelFromLars(standard.Id),
                    PdfFileName = standard.PdfFileName,
                    PdfUrl = standard.Pdf,
                    File = attachment
                };

                return doc;
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw;
            }
        }

        public bool IsIndexCorrectlyCreated()
        {
            var searchResults = _client.Search<StandardDocument>(s => s.From(0).Size(1000).MatchAll()).Documents.ToList();

            return searchResults.Any();
        }

        public void SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);

            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            _client.Alias(aliasRequest);
        }
    }
}