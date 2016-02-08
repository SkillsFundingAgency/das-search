using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Nest;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;
using System.Globalization;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
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
                Log.Info("Index already exists, deleting and creating a new one");

                _client.DeleteIndex(indexName);
            }

            // create index
            _client.CreateIndex(indexName, c => c.AddMapping<Provider>(m => m
                .MapFromAttributes()
                .Properties(p => p
                    .GeoPoint(g => g.Name(n => n.Coordinate).IndexLatLon()))));

            return _client.IndexExists(indexName).Exists;
        }

        public async Task IndexProviders(DateTime scheduledRefreshDateTime)
        {
            var providers = await GetProviders();

            try
            {
                Log.Info("Indexing " + providers.Count() + " providers");

                var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);
                IndexProviders(indexName, providers);
            }
            catch (Exception e)
            {
                Log.Error("Error indexing PDFs: " + e.Message);
            }
        }

        private async Task<IEnumerable<Provider>> GetProviders()
        {
            // TODO: Replace Demo data
            var providers = new List<Provider>
            {
                new Provider
                {
                    UkPrn = "10003347",
                    PostCode = "CV21 2BB",
                    ProviderName = "INTEC BUSINESS COLLEGES",
                    VenueName = "INTEC BUSINESS COLLEGES",
                    Radius = 35,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.3714464,
                        Lon = -1.2669471
                    }
                },
                new Provider
                {
                    UkPrn = "10001309",
                    PostCode = "CV32 4JE",
                    ProviderName = "COVENTRY & WARWICKSHIRE CHAMBER TRAINING (CWT)",
                    VenueName = "COVENTRY & WARWICKSHIRE CHAMBER TRAINING (CWT)",
                    Radius = 40,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.290897,
                        Lon = -1.528915
                    }
                },
                new Provider
                {
                    UkPrn = "10031241",
                    PostCode = "B4 7LR",
                    ProviderName = "ASPIRE ACHIEVE ADVANCE LIMITED",
                    VenueName = "3AAA BIRMINGHAM",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.4819902,
                        Lon = -1.8923181
                    }
                },
                new Provider
                {
                    UkPrn = "10005967",
                    PostCode = "B5 5SU",
                    ProviderName = "SOUTH & CITY COLLEGE BIRMINGHAM",
                    VenueName = "	Digbeth Campus",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.4754573,
                        Lon = -1.8857531
                    }
                },
                new Provider
                {
                    UkPrn = "10007015",
                    PostCode = "DE24 8AJ",
                    ProviderName = "TRAINING SERVICES 2000 LTD",
                    VenueName = "TRAINING SERVICES 2000 LTD",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.9106629,
                        Lon = -1.4467433
                    }
                },
                new Provider
                {
                    UkPrn = "10031241",
                    PostCode = "DE1 2JT",
                    ProviderName = "ASPIRE ACHIEVE ADVANCE LIMITED",
                    VenueName = "3AAA DERBY",
                    Radius = 60,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.918635,
                        Lon = -1.4761639
                    }
                },
                new Provider
                {
                    UkPrn = "10031241",
                    PostCode = "WC1X 8QB",
                    ProviderName = "ASPIRE ACHIEVE ADVANCE LIMITED",
                    VenueName = "3AAA KINGS CROSS",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 51.5292025,
                        Lon = -0.1202702
                    }
                },
                new Provider
                {
                    UkPrn = "10012834",
                    PostCode = "W6 7AN",
                    ProviderName = "SKILLS TEAM LTD",
                    VenueName = "EMPLOYERS WORK PLACE",
                    Radius = 30,
                    Coordinate = new Coordinate
                    {
                        Lat = 51.4938191,
                        Lon = -0.2236763
                    }
                },
                new Provider
                {
                    UkPrn = "10005264",
                    PostCode = "NG10 1LL",
                    ProviderName = "MILLBROOK MANAGEMENT SERVICES LIMITED",
                    VenueName = "PROSTART TRAINING",
                    Radius = 60,
                    Coordinate = new Coordinate
                    {
                        Lat = 52.8967801,
                        Lon = -1.2682401
                    }
                }
            };

            return providers;
        }

        private void IndexProviders(string indexName, IEnumerable<Provider> providers)
        {
            // index the items
            foreach (var provider in providers)
            {
                try
                {
                    _client.Index(provider, i => i.Index(indexName));
                }
                catch (Exception e)
                {
                    Log.Info("Error indexing provider: " + e.Message);
                    throw;
                }
            }
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
                Log.Info("Alias doesn't exists, creating a new one...");

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
                    .Alias(_settings.ProviderIndexesAlias)
                )
            );
        }

        public void DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var ts = new TimeSpan(2, 0, 0, 0, 0);
            var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime.Subtract(ts));

            var indexExistsResponse = _client.IndexExists(indexName);

            if (indexExistsResponse.Exists)
            {
                _client.DeleteIndex(indexName);
            }
        }

        private string GetIndexNameAndDateExtension(DateTime dateTime)
        {
            return string.Format("{0}-{1}", _settings.ProviderIndexesAlias, dateTime.ToUniversalTime().ToString("yyyy-MM-dd-HH")).ToLower(CultureInfo.InvariantCulture);
        }
    }
}