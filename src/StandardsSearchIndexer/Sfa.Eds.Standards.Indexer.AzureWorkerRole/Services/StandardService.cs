using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;
using Nest;
using Newtonsoft.Json;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public class StandardService
    {
        private static ElasticClient _client = new ElasticClient();

        private static int counter = 0;

        public async void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Trace.TraceInformation(counter++ + "Getting index alias");
            var indexAlias = GetIndexAlias();

            Trace.TraceInformation(counter++ + "Getting new index name");
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);

            Trace.TraceInformation(counter++ + "Setting node");
            //var node = new Uri(ConfigurationManager.AppSettings["ServerUri"]);
            var node = new Uri("http://104.45.94.2:9200/");

            var connectionSettings = new ConnectionSettings(node, newIndexName);

            Trace.TraceInformation(counter++ + "New Elasticsearch client");
            _client = new ElasticClient(connectionSettings);

            Trace.TraceInformation(counter++ + "Create index if doesn't exists");
            var existingPreviousIndex = CreateIndex(newIndexName);
            Trace.TraceInformation(counter++ + "Checking if index exists");
            if (existingPreviousIndex) return;

            Trace.TraceInformation(counter++ + "Indexing pdfs");
            await IndexStandards(newIndexName);

            PauseWhileIndexingIsBeingRun();

            if (IsIndexCorrectlyCreated())
            {
                SwapIndexes(scheduledRefreshDateTime);
            }
        }

        private static string GetIndexAlias()
        {
            //return ConfigurationManager.AppSettings["StandardIndexesAlias"];
            return "standardIndexAlias";
        }

        private static string GetIndexNameAndDateExtension(string indexAlias, DateTime dateTime)
        {
            return string.Format("{0}-{1}", indexAlias, dateTime.ToUniversalTime().ToString("yyyy-MM-dd-HH")).ToLower();
        }

        private static bool CreateIndex(string indexName)
        {
            //indexName = "patatuela";
            Trace.TraceInformation("Creating index with name: " + indexName);

            var indexExistsResponse = _client.IndexExists(indexName);// IndexExists(i => i.Index(indexName));

            //If it already exists and is empty, let's delete it.
            if (indexExistsResponse.Exists)
            {
                var totalResults = _client.Count<StandardDocument>(c =>
                {
                    c.Index(indexName);
                    return c;
                });

                //if (totalResults.Count == 0)
                //{
                    _client.DeleteIndex(indexName);
                    indexExistsResponse = _client.IndexExists(indexName);
                //}
            }

            //create index
            if (!indexExistsResponse.Exists)
            {
                CreateDocumentIndex(indexName);
            }

            return indexExistsResponse.Exists;
        }

        static public IIndicesOperationResponse CreateDocumentIndex(string indexName)
        {
            var creation = _client.CreateIndex(indexName, c => c.AddMapping<StandardDocument>(m => m.MapFromAttributes()));
            return creation;
        }

        static public async Task UploadStandardsContentToAzure(List<JsonMetadataObject> standardList)
        {
            Trace.TraceInformation(counter++ + "EMPIEZA updateo de pdfs");

            foreach (var standard in standardList)
            {
                //await UploadStandardJson(standard);
                await UploadStandardPdf(standard);
            }
            Trace.TraceInformation(counter++ + "ACABADO updateo de pdfs");

        }

        static public async Task UploadStandardJson(JsonMetadataObject standard)
        {
            BlobStorageHelper bsh = new BlobStorageHelper();
            await bsh.UploadStandardAsync("standardsjson", string.Format(standard.Id.ToString(), ".txt"), JsonConvert.SerializeObject(standard));
        }

        static public async Task UploadStandardPdf(JsonMetadataObject standard)
        {
            BlobStorageHelper bsh = new BlobStorageHelper();
            await bsh.UploadPdfFromUrl("standardspdf", string.Format(standard.Id.ToString(), ".txt"), standard.Pdf);
        }

        static public async Task<List<JsonMetadataObject>> GetStandardsFromAzure()
        {
            Trace.TraceInformation(counter++ + "Pillando Standards de Azure");

            var standardsList = new List<JsonMetadataObject>();

            BlobStorageHelper bsh = new BlobStorageHelper();
            standardsList = await bsh.ReadStandardsAsync("standardsjson");

            standardsList = standardsList.OrderBy(s => s.Id).ToList();
            
            Trace.TraceInformation(counter++ + "Standards de Azure ya pillados: " + standardsList.Count);

            return standardsList;
        }

        private static async Task IndexStandardPdfs(string indexName, List<JsonMetadataObject> standards)
        {
            Trace.TraceInformation(counter++ + "Indexando documentos");

            //index the items
            foreach (var standard in standards)
            {
                var doc = await CreateDocument(standard);

                var indexResponse = _client.Index(doc, i => i
                    .Index(indexName)
                    .Id(doc.StandardId));
            }

            Trace.TraceInformation(counter++ + "Supuestamente indexado");
        }

        private static async Task IndexStandards(string newIndexName)
        {
            Trace.TraceInformation(counter++ + "Vamos a pillar la info de Azure");

            var standards = await GetStandardsFromAzure();

            Trace.TraceInformation(counter++ + "Ahora toca updatear los pdfs a azure");

            await UploadStandardsContentToAzure(standards);

            Trace.TraceInformation(counter++ + "VAMOS A INDEXAR ALGO");
            
            try
            {
                await IndexStandardPdfs(newIndexName, standards);
            }
            catch (Exception e)
            {
                Trace.TraceError(counter++ + "ERROR!!!! -> " + e.Message);
                var error = e;
            }
        }

        static public async Task<StandardDocument> CreateDocument(JsonMetadataObject standard)
        {
            BlobStorageHelper bsh = new BlobStorageHelper();

            var attachment = new Attachment
            {
                Content = Convert.ToBase64String(await bsh.ReadStandardPdfAsync("standardspdf", string.Format(standard.Id.ToString(), ".txt"))),
                ContentType = "application/pdf",
                Name = standard.PdfFileName
            };

            var doc = new StandardDocument()
            {
                StandardId = standard.Id,
                Title = standard.Title,
                NotionalEndLevel = DedsService.GetNotationLevelFromLars(standard.Id),
                PdfFileName = standard.PdfFileName,
                PdfUrl = standard.Pdf,
                File = attachment
            };

            return doc;
        }

        private static void PauseWhileIndexingIsBeingRun()
        {
            var sleepTime = 1000;
            Thread.Sleep(sleepTime);
        }

        private static bool IsIndexCorrectlyCreated()
        {
            var searchResults = _client.Search<StandardDocument>(s => s.From(0).Size(1000).MatchAll()).Documents.ToList();

            return searchResults.Any();
        }

        private static bool SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);

            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            _client.Alias(aliasRequest);

            return true;
        }
    }
}
