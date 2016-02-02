using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using LocationIndexer.Models;
using Nest;

namespace LocationIndexer.Services
{
    public class LocationIndexer
    {
        private static ElasticClient _client = new ElasticClient();

        public async void CreateLocationIndex()
        {

            var indexName = "locationtest";

            var node = new Uri("http://localhost:9200");

            var connectionSettings = new ConnectionSettings(node, indexName);

            _client = new ElasticClient(connectionSettings);

            /*
            TextReader textReader = new StreamReader(@"C:/Projects/ONSPD_NOV_2015_UK.csv");
            //TextReader textReader = new StreamReader(@"C:/Projects/test.csv");
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<OnsPostCode>();

            //var first = records.First();

            CreateIndex(indexName);
            
            int count = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var englishOnly = records.Where(x => x.oscty.StartsWith("E"));

            var testList = new List<int>();


            for (int i = 0; i < 100000; i++)
            {
                testList.Add(i);
            }

            Parallel.ForEach(englishOnly, x =>
            {
                var data = new Location
                {
                    PostCode = x.pcd,
                    Region = x.gor,
                    Coordinate = new Coordinate()
                    {
                        Lat = double.Parse(x.lat),
                        Lon = double.Parse(x.@long)

                    }
                };

                var response = _client.Index(data);

                Interlocked.Increment(ref count);

                if (count % 10000 == 0)
                    Console.WriteLine("Processed {0} in {1}", count, watch.Elapsed);
            });
            */

            /*
            var locations = GetLocations();
            
            CreateIndex(indexName);

            IndexLocations(indexName, locations);
            */
            RunSomeTestQueries(indexName);

            var a = "patata";

        }

        private IEnumerable<Location> GetLocations()
        {
            string path = @"C:/Projects/";
            string filename = "test.csv";
            //string filename = "ONSPD_NOV_2015_UK.csv";

            List<Location> locations = new List<Location>();

            //var csv = new CsvReader(File.OpenText(string.Concat(path, filename)));
            using (CsvReader csv = new CsvReader(new StreamReader(string.Concat(path, filename))))
            {
                while (csv.Read())
                {
                    Location loc = new Location
                    {
                        PostCode = csv.GetField<string>(0),
                        Region = csv.GetField<string>(15),
                        Coordinate = new Coordinate()
                        {
                            Lat = csv.GetField<double>(51),
                            Lon = csv.GetField<double>(52)
                        }
                    };
                    locations.Add(loc);
                }
            }

            return locations;
        }

        private static void CreateIndex(string indexName)
        {
            var indexExistsResponse = _client.IndexExists(i => i.Index(indexName));

            //If it already exists and is empty, let's delete it. This is only useful for testing.
            //Apparently, this code also runs if you get a 404. yay!
            if (indexExistsResponse.Exists)
            {
                var totalResults = _client.Count<Location>(c =>
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

        static public IIndicesOperationResponse CreateDocumentIndex(string indexName)
        {
            var a = _client.CreateIndex(indexName, c => c.AddMapping<Location>(m => m
                .MapFromAttributes()
                .Properties(p => p
                    .GeoPoint(g => g.Name(n => n.Coordinate).IndexLatLon())
                )));
            return a;
        }

        private void IndexLocations(string indexName, IEnumerable<Location> locations)
        {
            foreach (var location in locations)
            {
                var a = _client.Index(location, i => i
                    .Index(indexName));
            }
        }

        private static void RunSomeTestQueries(string indexName)
        {
            //search on file property
            //Attempted here to replicate a REST API search:
            //  {query:{match:{_file._content:"Accounting"}}}
            //  /elasticsearchmapperattachments-test/mydocument/_search?_source=%7Bquery%3A%7Bmatch%3A%7B_file._content%3A%22Accounting%22%7D%7D%7D

            var searchResults = _client.Search<Location>(s => s.MatchAll());

            QueryContainer query1 = new TermQuery
            {
                Field = "postcode",
                Value = "NW6 6AY"
            };

            var results = _client.Search<Location>(s => s
                .QueryString("NW6 6AY"));

            var searchResults2 = _client.Search<Location>(s => s
                .Filter(f => f
                    .GeoDistance(
                        n => n.Coordinate,
                        d => d.Distance(20.0, GeoUnit.Kilometers).Location(51.386615, -0.039525)
                    )
                )
            );
            
            

            var a = "patata";
        }

        private class OnsPostCode
        {
            public string pcd { get; set; }
            public string pcd2 { get; set; }
            public string pcds { get; set; }
            public string dointr { get; set; }
            public string doterm { get; set; }
            public string oscty { get; set; }
            public string oslaua { get; set; }
            public string osward { get; set; }
            public string usertype { get; set; }
            public string oseast1m { get; set; }
            public string osnrth1m { get; set; }
            public string osgrdind { get; set; }
            public string oshlthau { get; set; }
            public string hro { get; set; }
            public string ctry { get; set; }
            public string gor { get; set; }
            public string streg { get; set; }
            public string pcon { get; set; }
            public string eer { get; set; }
            public string teclec { get; set; }
            public string ttwa { get; set; }
            public string pct { get; set; }
            public string nuts { get; set; }
            public string psed { get; set; }
            public string cened { get; set; }
            public string edind { get; set; }
            public string oshaprev { get; set; }
            public string lea { get; set; }
            public string oldha { get; set; }
            public string wardc91 { get; set; }
            public string wardo91 { get; set; }
            public string ward98 { get; set; }
            public string statsward { get; set; }
            public string oa01 { get; set; }
            public string casward { get; set; }
            public string park { get; set; }
            public string lsoa01 { get; set; }
            public string msoa01 { get; set; }
            public string ur01ind { get; set; }
            public string oac01 { get; set; }
            public string oldpct { get; set; }
            public string oa11 { get; set; }
            public string lsoa11 { get; set; }
            public string msoa11 { get; set; }
            public string parish { get; set; }
            public string wz11 { get; set; }
            public string ccg { get; set; }
            public string bua11 { get; set; }
            public string buasd11 { get; set; }
            public string ru11ind { get; set; }
            public string oac11 { get; set; }
            public string lat { get; set; }
            public string @long { get; set; }
            public string lep1 { get; set; }
            public string lep2 { get; set; }
        }
    }

}
