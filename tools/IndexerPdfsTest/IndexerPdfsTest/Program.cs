using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using IndexerPdfsTest.Model;
using IndexerPdfsTest.DedsSearchService;
using Newtonsoft.Json;

namespace IndexerPdfsTest
{
    public class Program
    {
        private static ElasticClient _client = new ElasticClient();

        static void Main(string[] args)
        {
            //var result = DedsService.GetLarsData(1);

            var indexName = ConfigurationManager.AppSettings["indexName"];
            
            var node = new Uri(ConfigurationManager.AppSettings["serverUri"]);

            var connectionSettings = new ConnectionSettings(node, indexName);

            _client = new ElasticClient(connectionSettings);

            var standards = GetStandards();

            CreateIndex(indexName);

            IndexStandardPdfs(indexName, standards);

            //WARNING: The next two lines don't work.  DO NOT USE index request (or find out how to use it properly)
            //var indexRequest = new IndexRequest<MyDocument>(doc);
            //var indexResponse = client.Index(indexRequest, i => i.Index(indexName));

            //search for document by Id
            //var getDocResponse = client.Get<MyDocument>(indexResponse.Id, indexName);

            PauseWhileIndexingIsBeingRun();

            RunSomeTestQueries(indexName);

            Console.ReadKey();
        }

        private static void PauseWhileIndexingIsBeingRun()
        {
            var sleepTime = 1000;
            Console.WriteLine("Wait for {0}ms while indexing occurs...", sleepTime);
            Thread.Sleep(sleepTime);
        }

        private static void RunSomeTestQueries(string indexName)
        {
            //search on file property
            //Attempted here to replicate a REST API search:
            //  {query:{match:{_file._content:"Accounting"}}}
            //  /elasticsearchmapperattachments-test/mydocument/_search?_source=%7Bquery%3A%7Bmatch%3A%7B_file._content%3A%22Accounting%22%7D%7D%7D
            var query = Query<MyDocument>.Term("file.content", "surveyors");
            
            var searchResults = _client.Search<MyDocument>(s => s
                .Index(indexName)
                .From(0)
                .Size(100)
                .Query(q => q
                    .Match(m => m
                        .OnField(p => p.File.Content)
                        .Query("surveyors")
                    )
                )
            );

            var searchResultsFuzzy = _client.Search<MyDocument>(s => s
                .Index(indexName)
                .From(0)
                .Size(100)
                .Query(q => q
                    .Fuzzy(fz => fz.OnField(f => f.Title).Value("surveyor").OnField("title"))
                )
            );

            var searchResults2 = _client.Search<MyDocument>(s => s.From(0).Size(1000).MatchAll());

            var searchResults3 = _client.Search<MyDocument>(s => s
                .From(0)
                .Size(10)
                //.Query(q => q.QueryString(x => x.Query("surveyors")))
                //.Query(query)
                .QueryRaw(@"{""query_string"": {""fields"": [""file.content""],""query"": """ + "surveyor~1" + @"""}}")
            );

            var result = _client.Search<MyDocument>(s => s
                .From(0)
                .Size(10000)
                .QueryString("surveyors"));

            DisplayListOfResults(searchResults2);
        }

        private static void DisplayListOfResults(ISearchResponse<MyDocument> searchResults)
        {
            foreach (var document in searchResults.Documents)
            {
                Console.WriteLine("{0}", document.Title);
            }

            Console.WriteLine("\n{0} results found", searchResults.Total);
        }

        private static void CreateIndex(string indexName)
        {
            var indexExistsResponse = _client.IndexExists(i => i.Index(indexName));

            //If it already exists and is empty, let's delete it. This is only useful for testing.
            //Apparently, this code also runs if you get a 404. yay!
            if (indexExistsResponse.Exists)
            {
                var totalResults = _client.Count<MyDocument>(c =>
                {
                    c.Index(indexName);
                    return c;
                });

                //if (totalResults.Count == 0)
                //{
                    _client.DeleteIndex(i => i.Index(indexName));
                    indexExistsResponse = _client.IndexExists(i => i.Index(indexName));
                //}
            }

            //create index
            if (!indexExistsResponse.Exists)
            {
                CreateDocumentIndex(indexName);
            }
        }

        private static void IndexStandardPdfs(string indexName, List<JsonMetadataObject> standards)
        {
            string basePath = ConfigurationManager.AppSettings["PdfDownloadDirectory"];

            //index the items
            foreach (var standard in standards)
            {
                string path = string.Concat(basePath, "\\", standard.PdfFileName, ".pdf");

                DownloadPdf(standard, path);

                var doc = CreateDocument(standard, path);

                var indexResponse = _client.Index(doc, i => i
                    .Index(indexName)
                    .Id(doc.StandardId));
            }
        }

        static public List<JsonMetadataObject> GetStandards()
        {
            var standardsJsonBaseRoute = ConfigurationManager.AppSettings["JsonDirectory"];

            var standardsList = new List<JsonMetadataObject>();

            if (Directory.Exists(standardsJsonBaseRoute))
            {
                var directory = new DirectoryInfo(standardsJsonBaseRoute);
                var files = directory.GetFiles();

                foreach (var fileInfo in files)
                {
                    using (StreamReader file = File.OpenText(fileInfo.FullName))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        JsonMetadataObject standard = (JsonMetadataObject)serializer.Deserialize(file, typeof(JsonMetadataObject));
                        standardsList.Add(standard);
                    }
                }
                
            }

            return standardsList;
            
        }

        static public IIndicesOperationResponse CreateDocumentIndex(string indexName)
        {
            return _client.CreateIndex(indexName, c => c.AddMapping<MyDocument>(m => m.MapFromAttributes()));
        }

        static private void DownloadPdf(JsonMetadataObject standard, string path)
        {
            using (WebClient wClient = new WebClient())
            {
                wClient.DownloadFile(standard.Pdf, path);
            }
        }

        static public void UpdateStandard(JsonMetadataObject standard)
        {
            string basePath = ConfigurationManager.AppSettings["PdfDownloadDirectory"];

            string path = string.Concat(basePath, "\\", standard.PdfFileName, ".pdf");

            DownloadPdf(standard, path);

            var doc = CreateDocument(standard, path);

             var status =_client.Update<MyDocument, object>(u => u
                .Id(standard.Id)
                .Doc(doc)
                .RetryOnConflict(3)
                .Refresh()
            );
        }

        static public MyDocument CreateDocument(JsonMetadataObject standard, string path)
        {
            var attachment = new Attachment
            {
                Content = Convert.ToBase64String(File.ReadAllBytes(path)),
                ContentType = "application/pdf",
                Name = standard.PdfFileName
            };

            var doc = new MyDocument()
            {
                StandardId = standard.Id,
                Title = standard.Title,
                NotionalEndLevel = standard.NotionalEndLevel,
                PdfFileName = standard.PdfFileName,
                PdfUrl = standard.Pdf,
                File = attachment
            };

            return doc;
        }
    }
}
