using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
using Sfa.Eds.Das.Indexer.Core;
using Sfa.Eds.Das.Indexer.Core.Services;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.Provider
{
    public class ProviderHelper : IGenericIndexerHelper<Core.Models.Provider.Provider>
    {
        private readonly IGetActiveProviders _activeProviderClient;
        private readonly IIndexMaintenanceService _indexMaintenanceService;
        private readonly IGetApprenticeshipProviders _providerRepository;
        private readonly IElasticClient _client;
        private readonly IIndexSettings<Core.Models.Provider.Provider> _settings;
        private readonly ILog Log;

        public ProviderHelper(
            IIndexSettings<Core.Models.Provider.Provider> settings,
            IElasticsearchClientFactory elasticsearchClientFactory,
            IGetApprenticeshipProviders providerRepository,
            IGetActiveProviders activeProviderClient,
            IIndexMaintenanceService indexMaintenanceService,
            ILog log)
        {
            _settings = settings;
            _providerRepository = providerRepository;
            _activeProviderClient = activeProviderClient;
            _indexMaintenanceService = indexMaintenanceService;

            _client = elasticsearchClientFactory.GetElasticClient();
            Log = log;
        }

        public async Task<ICollection<Core.Models.Provider.Provider>> LoadEntries()
        {
            var providers = await _providerRepository.GetApprenticeshipProvidersAsync();
            var activeProviders = _activeProviderClient.GetActiveProviders().ToList();

            return providers.Where(x => activeProviders.Contains(x.Ukprn)).ToList();
        }

        public bool CreateIndex(DateTime scheduledRefreshDateTime)
        {
            var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);

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
                                ""ukprn"":
                                {
                                    ""type"": ""long""
                                },
                                ""id"":
                                {
                                    ""type"": ""long""
                                },
                                ""name"":
                                {
                                    ""type"": ""string""
                                },
                                ""standardCode"":
                                {
                                    ""type"": ""long""
                                },
                                ""locationId"":
                                {
                                    ""type"": ""long""
                                },
                                ""locationName"":
                                {
                                    ""type"": ""string""
                                },
                                ""marketingInfo"":
                                {
                                    ""type"": ""string""
                                },
                                ""standardInfoUrl"":
                                {
                                    ""type"": ""string""
                                },
                                ""deliveryModes"":
                                {
                                    ""type"": ""string""
                                },
                                ""website"":
                                {
                                    ""type"": ""string""
                                },
                                ""phone"":
                                {
                                    ""type"": ""string""
                                },
                                ""email"":
                                {
                                    ""type"": ""string""
                                },
                                ""contactUsUrl"":
                                {
                                    ""type"": ""string""
                                },
                                ""standardsId"":
                                {
                                    ""type"": ""long""
                                },
                                ""locationPoint"":
                                {
                                    ""type"": ""geo_point""
                                },
                                ""location"": 
                                {
                                    ""type"": ""geo_shape""
                                },
                                ""address"": 
                                {
                                    ""type"": ""nested"",
                                    ""properties"": 
                                    {
                                        ""address1"":       
                                        { 
                                            ""type"": ""string""  
                                        },
                                        ""address2"":       
                                        { 
                                            ""type"": ""string""  
                                        },  
                                        ""town"":       
                                        { 
                                            ""type"": ""string""  
                                        },
                                        ""county"":       
                                        { 
                                            ""type"": ""string""  
                                        },
                                        ""postcode"":       
                                        { 
                                            ""type"": ""string""  
                                        }
                                    }
                                }
                            }
                        }
                    }
                }";

            _client.Raw.IndicesCreatePost(indexName, json);

            var exists = _client.IndexExists(indexName).Exists;

            return exists;
        }

        public async Task IndexEntries(DateTime scheduledRefreshDateTime, ICollection<Core.Models.Provider.Provider> entries)
        {
            try
            {
                Log.Debug("Indexing " + entries.Count + " providers");

                var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);
                await IndexProviders(indexName, entries).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("Error indexing providerss: " + e.Message);
            }
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);

            var a = _client.Search<Core.Models.Provider.Provider>(s => s.Index(indexName).From(0).Size(1000).MatchAll()).Documents;
            return a.Any();
        }

        public void SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = _settings.IndexesAlias;
            var newIndexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);

            if (!CheckIfAliasExists(indexAlias))
            {
                Log.Warn("Alias doesn't exists, creating a new one...");

                CreateAlias(newIndexName);
            }

            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(
                    new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            _client.Alias(aliasRequest);
        }

        public void DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var indicesToBeDelete = _indexMaintenanceService.GetOldIndices(_settings.IndexesAlias, scheduledRefreshDateTime, _client.IndicesStats().Indices);

            Log.Debug($"Deleting {indicesToBeDelete.Count()} old provider indexes...");
            foreach (var index in indicesToBeDelete)
            {
                Log.Debug($"Deleting {index}");
                _client.DeleteIndex(index);
            }
            Log.Debug("Deletion completed...");
        }
        
        // TODO: LWA - I think this should probably live in Infrastructure
        public List<string> CreateListRawFormat(Core.Models.Provider.Provider provider, Core.Models.Provider.StandardInformation standard)
        {
            var list = new List<string>();

            foreach (var standardLocation in standard.DeliveryLocations)
            {
                // var location = provider.Locations.FirstOrDefault(x => x.ID == standardLocation.ID);

                var location = standardLocation.DeliveryLocation;

                var i = 0;
                var deliveryModes = new StringBuilder();
                foreach (var deliveryMode in standardLocation.DeliveryModes)
                {
                    // TODO: LWA Need to put textual verion in array???
                    deliveryModes.Append(i == 0 ? string.Concat(@"""", deliveryMode, @"""") : string.Concat(", ", @"""", deliveryMode, @""""));

                    i++;
                }

                var rawProvider = string.Concat(
                @"{ ""ukprn"": """,
                provider.Ukprn,
                @""", ""id"": """,
                provider.Id,
                @""", ""name"": """,
                provider.Name ?? "Unspecified",
                @""", ""standardcode"": ",
                standard.StandardCode,
                @", ""locationId"": ",
                location.Id,
                @", ""locationName"": """,
                location.Name ?? "Unspecified",
                @""", ""marketingInfo"": """,
                standard.MarketingInfo ?? "Unspecified",
                @""", ""phone"": """,
                standard.StandardContact.Phone ?? "Unspecified",
                @""", ""email"": """,
                standard.StandardContact.Email ?? "Unspecified",
                @""", ""contactUsUrl"": """,
                standard.StandardContact.Website ?? "Unspecified",
                @""", ""standardInfoUrl"": """,
                standard.StandardInfoUrl ?? "Unspecified",
                @""", ""deliveryModes"": [",
                deliveryModes,
                @"], ""website"": """,
                location.Contact.Website ?? "Unspecified",
                @""", ""address"": {""address1"":""",
                location.Address.Address1 ?? "Unspecified",
                @""", ""address2"": """,
                location.Address.Address2 ?? "Unspecified",
                @""", ""town"": """,
                location.Address.Town ?? "Unspecified",
                @""", ""county"": """,
                location.Address.County ?? "Unspecified",
                @""", ""postcode"": """,
                location.Address.Postcode ?? "Unspecified",
                @"""}, ""locationPoint"": [",
                location.Address.GeoPoint.Lon,
                ", ",
                location.Address.GeoPoint.Lat,
                @"],""location"": { ""type"": ""circle"", ""coordinates"": [",
                location.Address.GeoPoint.Lon,
                ", ",
                location.Address.GeoPoint.Lat,
                @"], ""radius"": """,
                standardLocation.Radius,
                @"mi"" }}");

                list.Add(rawProvider);
            }

            return list;
        }

        private Task IndexProviders(string indexName, IEnumerable<Core.Models.Provider.Provider> providers)
        {
            // index the items
            foreach (var provider in providers)
            {
                try
                {
                    foreach (var standard in provider.Standards)
                    {
                        var queryList = CreateListRawFormat(provider, standard);
                        foreach (var query in queryList)
                        {
                            _client.Raw.Index(indexName, "provider", query);
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    Log.Error("Error indexing provider: " + e.Message);
                    throw;
                }
            }

            return null;
        }

        private bool CheckIfAliasExists(string aliasName)
        {
            var aliasExistsResponse = _client.AliasExists(aliasName);

            return aliasExistsResponse.Exists;
        }

        private void CreateAlias(string indexName)
        {
            _client.Alias(a => a.Add(add => add.Index(indexName).Alias(_settings.IndexesAlias)));
        }
    }
}