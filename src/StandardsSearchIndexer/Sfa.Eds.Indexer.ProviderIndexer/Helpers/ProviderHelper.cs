namespace Sfa.Eds.Das.ProviderIndexer.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using log4net;

    using Nest;

    using Sfa.Eds.Das.Indexer.Common.Configuration;
    using Sfa.Eds.Das.Indexer.Common.Helpers;
    using Sfa.Eds.Das.Indexer.Common.Settings;
    using Sfa.Eds.Das.ProviderIndexer.Clients;
    using Sfa.Eds.Das.ProviderIndexer.Models;

    public class ProviderHelper : IGenericIndexerHelper<Provider>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IActiveProviderClient _activeProviderClient;

        private readonly IElasticClient _client;

        private readonly ICourseDirectoryClient _courseDirectoryClient;

        private readonly IIndexSettings<Provider> _settings;

        public ProviderHelper(
            IIndexSettings<Provider> settings,
            IElasticsearchClientFactory elasticsearchClientFactory,
            ICourseDirectoryClient courseDirectoryClient,
            IActiveProviderClient activeProviderClient)
        {
            _settings = settings;
            _courseDirectoryClient = courseDirectoryClient;
            _activeProviderClient = activeProviderClient;

            _client = elasticsearchClientFactory.GetElasticClient();
        }

        public ICollection<Provider> LoadEntries()
        {
            var providers = _courseDirectoryClient.GetProviders();
            var activeProviders = _activeProviderClient.GetProviders();
            Task.WaitAll();

            return providers.Result.Where(x => activeProviders.Contains(x.UkPrn)).ToList();
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
                                ""locationPoint"":
                                {
                                    ""type"": ""geo_point""
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

        public async Task IndexEntries(DateTime scheduledRefreshDateTime, ICollection<Provider> entries)
        {
            try
            {
                Log.Debug("Indexing " + entries.Count + " providers");

                var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);
                await IndexProviders(indexName, entries).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("Error indexing providerss: " + e.Message);
            }
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);

            var a = _client.Search<Provider>(s => s.Index(indexName).From(0).Size(1000).MatchAll()).Documents;
            return a.Any();
        }

        public void SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = _settings.IndexesAlias;
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
                aliasRequest.Actions.Add(
                    new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            _client.Alias(aliasRequest);
        }

        public void DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var dateTime = scheduledRefreshDateTime.AddDays(-2);

            for (var i = 0; i < 23; i++)
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
            return
                string.Format("{0}-{1}", _settings.IndexesAlias, dateTime.ToUniversalTime().ToString("yyyy-MM-dd-HH"))
                    .ToLower(CultureInfo.InvariantCulture);
        }

        private string CreateProviderRawFormat(Provider provider)
        {
            var i = 0;
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
                @""", ""locationPoint"": [",
                provider.Coordinate.Lon,
                ", ",
                provider.Coordinate.Lat,
                @"],""location"": { ""type"": ""circle"", ""coordinates"": [",
                provider.Coordinate.Lon,
                ", ",
                provider.Coordinate.Lat,
                @"], ""radius"": """,
                provider.Radius,
                @"mi"" }}");
            return rawProvider;
        }

        private Task IndexProviders(string indexName, IEnumerable<Provider> providers)
        {
            var id = 1;

            // index the items
            foreach (var provider in providers)
            {
                try
                {
                    provider.ProviderId = id;
                    _client.Raw.Index(indexName, "provider", CreateProviderRawFormat(provider));
                    id++;
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