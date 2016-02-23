using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using log4net;
using Nest;
using Sfa.Eds.Das.Indexer.Common.Configuration;
using Sfa.Eds.Das.Indexer.Common.Models;
using Sfa.Eds.Das.ProviderIndexer.Settings;

namespace Sfa.Eds.Das.ProviderIndexer.Helpers
{
    public class ProviderHelper : IProviderHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IProviderIndexSettings _settings;
        private readonly IElasticClient _client;

        public ProviderHelper(
            IProviderIndexSettings settings,
            IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _settings = settings;
            _elasticsearchClientFactory = elasticsearchClientFactory;

            _client = _elasticsearchClientFactory.GetElasticClient();
        }

        public bool CreateIndex(DateTime scheduledRefreshDateTime)
        {
            var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);

            var indexExistsResponse = _client.IndexExists(indexName);

            // If it already exists and is empty, let's delete it.
            if (indexExistsResponse.Exists)
            {
                Log.Warn("Index already exists, deleting and creating a new one");

                _client.DeleteIndex(indexName);
            }

            // create index
            var json = @"
                {
                    ""mappings"": 
                    {
                        ""provider"": 
                        { 
                            ""properties"": 
                            {
                                ""id"":
                                {
                                    ""type"": ""long""
                                },
                                ""providerName"":
                                {
                                    ""type"": ""string""
                                },
                                ""postCode"":
                                {
                                    ""type"": ""string""
                                },
                                ""radius"":
                                {
                                    ""type"": ""long""
                                },
                                ""ukPrn"":
                                {
                                    ""type"": ""string""
                                },
                                ""venueName"":
                                {
                                    ""type"": ""string""
                                },
                                ""standardsId"":
                                {
                                    ""type"": ""long""
                                },
                                ""coordinate"":
                                {
                                    ""properties"": 
                                    {
                                        ""lat"": 
                                        {
                                            ""type"": ""double""
                                        },
                                        ""lon"":
                                        {
                                            ""type"": ""double""
                                        }
                                    }
                                },
                                ""location"": 
                                {
                                    ""type"": ""geo_shape""
                                }
                            }
                        }
                    }
                }";
            _client.Raw.IndicesCreatePost(indexName, json);

            var exists = _client.IndexExists(indexName).Exists;
            return exists;
        }

        public void IndexProviders(DateTime scheduledRefreshDateTime, List<Provider> providers)
        {
            try
            {
                Log.Debug("Indexing " + providers.Count + " providers");

                var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);
                IndexProviders(indexName, providers);
            }
            catch (Exception e)
            {
                Log.Error("Error indexing providerss: " + e.Message);
            }
        }

        public List<Provider> GetProviders()
        {
            // TODO: Replace Demo data
            var providers = new List<Provider>
            {
                new Provider
                {
                    UkPrn = "10003347",
                    PostCode = "CV21 2BB",
                    ProviderName = "Intec Business Colleges",
                    VenueName = "INTEC BUSINESS COLLEGES",
                    Radius = 35,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.3714464,
                        Lon = -1.2669471
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10001309",
                    PostCode = "CV32 4JE",
                    ProviderName = "Coventry & Warwickshire Chamber Training (CWT)",
                    VenueName = "COVENTRY & WARWICKSHIRE CHAMBER TRAINING (CWT)",
                    Radius = 40,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.290897,
                        Lon = -1.528915
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10031241",
                    PostCode = "B4 7LR",
                    ProviderName = "Aspire Achieve Advance Limited",
                    VenueName = "3AAA BIRMINGHAM",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.4819902,
                        Lon = -1.8923181
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10005967",
                    PostCode = "B5 5SU",
                    ProviderName = "South & City College Birmingham",
                    VenueName = "Digbeth Campus",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.4754573,
                        Lon = -1.8857531
                    },
                    StandardsId = new List<int>
                    {
                        12, 25
                    }
                },
                new Provider
                {
                    UkPrn = "10007015",
                    PostCode = "DE24 8AJ",
                    ProviderName = "Training Services 2000 Ltd",
                    VenueName = "TRAINING SERVICES 2000 LTD",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.9106629,
                        Lon = -1.4467433
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10031241",
                    PostCode = "DE1 2JT",
                    ProviderName = "Aspire Archive Advance Limited",
                    VenueName = "3AAA DERBY",
                    Radius = 60,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.918635,
                        Lon = -1.4761639
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10031241",
                    PostCode = "WC1X 8QB",
                    ProviderName = "Aspire Achieve Advance Limited",
                    VenueName = "3AAA KINGS CROSS",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 51.5292025,
                        Lon = -0.1202702
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10012834",
                    PostCode = "W6 7AN",
                    ProviderName = "Skills Team Ltd",
                    VenueName = "EMPLOYERS WORK PLACE",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 51.4938191,
                        Lon = -0.2236763
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10005264",
                    PostCode = "NG10 1LL",
                    ProviderName = "Millbrook Management Services Limited",
                    VenueName = "PROSTART TRAINING",
                    Radius = 60,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.8967801,
                        Lon = -1.2682401
                    },
                    StandardsId = new List<int>
                    {
                        25
                    }
                },
                new Provider
                {
                    UkPrn = "10004355",
                    PostCode = "CV1 2JG",
                    ProviderName = "Midland Group Training Services Limited",
                    VenueName = "Midland Group Training Services Ltd",
                    Radius = 10,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.4050479,
                        Lon = -1.4966412
                    },
                    StandardsId = new List<int>
                    {
                        12
                    }
                },
                new Provider
                {
                    UkPrn = "10001919",
                    PostCode = "DE248JE",
                    ProviderName = "Derby College",
                    VenueName = "DERBY COLLEGE @ THE ROUNDHOUSE",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.9159961,
                        Lon = -1.4589891
                    },
                    StandardsId = new List<int>
                    {
                        12
                    }
                },
                new Provider
                {
                    UkPrn = "10005991",
                    PostCode = "NG7 2RU",
                    ProviderName = "Central College Nottingham",
                    VenueName = "Highfields",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.9367136,
                        Lon = -1.1869524
                    },
                    StandardsId = new List<int>
                    {
                        12
                    }
                },
                new Provider
                {
                    UkPrn = "10007924",
                    PostCode = "DY1 3AH",
                    ProviderName = "Dudley College of Technology",
                    VenueName = "Wolverhampton Street",
                    Radius = 40,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.5113022,
                        Lon = -2.090677
                    },
                    StandardsId = new List<int>
                    {
                        12
                    }
                },
                new Provider
                {
                    UkPrn = "10004355",
                    PostCode = "B98 8LY",
                    ProviderName = "Midland Group Training Services Limited",
                    VenueName = "MIDLAND GROUP TRAINING SERVICES LIMITED",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.3063609,
                        Lon = -1.9297031
                    },
                    StandardsId = new List<int>
                    {
                        12
                    }
                },
                new Provider
                {
                    UkPrn = "10005673",
                    PostCode = "B70 0AE",
                    ProviderName = "Sandwell Training Association Limited",
                    VenueName = "PHOENIX STREET",
                    Radius = 40,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.5257464,
                        Lon = -2.0192208
                    },
                    StandardsId = new List<int>
                    {
                        12
                    }
                }
            };

            return providers;
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);

            var a = _client
                .Search<Provider>(s => s.Index(indexName).From(0).Size(1000).MatchAll())
                .Documents;
            return a.Any();
        }

        public void SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = _settings.ProviderIndexesAlias;
            var newIndexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);

            if (!CheckIfAliasExists(indexAlias))
            {
                Log.Warn("Alias doesn't exists, creating a new one...");

                CreateAlias(newIndexName);
            }

            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            _client.Alias(aliasRequest);
        }

        public void DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var dateTime = scheduledRefreshDateTime.AddDays(-2);

            for (int i = 0; i < 23; i++)
            {
                var timeSpan = new TimeSpan(i, 0, 0);
                var dateTimeTmp = dateTime.Date + timeSpan;

                var indexName = GetIndexNameAndDateExtension(dateTimeTmp);

                var indexExistsResponse = _client.IndexExists(indexName);

                if (indexExistsResponse.Exists)
                {
                    _client.DeleteIndex(indexName);
                }
            }
        }

        public string GetIndexNameAndDateExtension(DateTime dateTime)
        {
            return string.Format("{0}-{1}", _settings.ProviderIndexesAlias, dateTime.ToUniversalTime().ToString("yyyy-MM-dd-HH")).ToLower(CultureInfo.InvariantCulture);
        }

        private string CreateProviderRawFormat(Provider provider)
        {
            int i = 0;
            var standardsId = new StringBuilder();
            foreach (var standardId in provider.StandardsId)
            {
                if (i == 0)
                {
                    standardsId.Append(standardId);
                }
                else
                {
                    standardsId.Append(string.Concat(", ", standardId));
                }

                i++;
            }

            var rawProvider = string.Concat(
                @"{ ""id"": """,
                provider.ProviderId,
                @""", ""providerName"": """,
                provider.ProviderName,
                @""", ""postCode"": """,
                provider.PostCode,
                @""", ""standardsId"": [",
                standardsId,
                @"], ""venueName"": """,
                provider.VenueName,
                @""", ""ukPrn"": """,
                provider.UkPrn,
                @""",""location"": { ""type"": ""circle"", ""coordinates"": [",
                provider.Coordinate.Lon,
                ", ",
                provider.Coordinate.Lat,
                @"], ""radius"": """,
                provider.Radius,
                @"mi"" }}");
            return rawProvider;
        }

        private void IndexProviders(string indexName, List<Provider> providers)
        {
            int id = 1;

            // index the items
            foreach (var provider in providers)
            {
                try
                {
                    provider.ProviderId = id;
                    _client.Raw.Index(indexName, "provider", CreateProviderRawFormat(provider));
                }
                catch (Exception e)
                {
                    Log.Error("Error indexing provider: " + e.Message);
                    throw;
                }
            }
        }

        private bool CheckIfAliasExists(string aliasName)
        {
            var aliasExistsResponse = _client.AliasExists(aliasName);

            return aliasExistsResponse.Exists;
        }

        private void CreateAlias(string indexName)
        {
            _client.Alias(a => a
                .Add(add => add
                    .Index(indexName)
                    .Alias(_settings.ProviderIndexesAlias)));
        }
    }
}