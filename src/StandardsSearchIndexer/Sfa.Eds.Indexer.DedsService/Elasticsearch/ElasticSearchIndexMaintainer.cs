using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
using Sfa.Eds.Das.Indexer.Core.Models.Provider;
using System.Text;
using Sfa.Eds.Das.Indexer.Core.Services;
using System.Linq;
using Sfa.Infrastructure.Services;
using Sfa.Eds.Das.Indexer.Core.Models;

// TODO: LWA - Split out into individual files.
namespace Sfa.Infrastructure.Elasticsearch
{
    public interface IGenerateIndexDefinitions<T>
    {
        string Generate();
    }

    public sealed class ProviderIndexGenerator : IGenerateIndexDefinitions<Provider>
    {
        public string Generate()
        {
            return @"
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
        }
    }

    public class ElasticsearchProviderIndexMaintainer : IMaintainSearchIndexes<Provider>
    {
        private readonly IElasticClient _client;
        private readonly IGenerateIndexDefinitions<Provider> _indexDefinitionGenerator;
        private readonly ILog _log;

        public ElasticsearchProviderIndexMaintainer(IElasticsearchClientFactory factory, IGenerateIndexDefinitions<Provider> indexDefinitionGenerator, ILog log)
        {
            _client = factory.GetElasticClient();
            _indexDefinitionGenerator = indexDefinitionGenerator;
            _log = log;
        }

        public bool AliasExists(string aliasName)
        {
            var aliasExistsResponse = _client.AliasExists(aliasName);

            return aliasExistsResponse.Exists;
        }

        public void CreateIndex(string indexName)
        {
            _client.Raw.IndicesCreatePost(indexName, _indexDefinitionGenerator.Generate());
        }

        public void CreateIndexAlias(string aliasName, string indexName)
        {
            _client.Alias(a => a.Add(add => add.Index(indexName).Alias(aliasName)));
        }

        public bool DeleteIndex(string indexName)
        {
            return _client.DeleteIndex(indexName).Acknowledged;
        }

        public bool DeleteIndexes(Func<string, bool> indexNameMatch)
        {
            var result = true;

            var indicesToBeDelete = _client.IndicesStats().Indices.Select(x => x.Key).Where(indexNameMatch);

            _log.Debug($"Deleting {indicesToBeDelete.Count()} old provider indexes...");

            foreach (var index in indicesToBeDelete)
            {
                _log.Debug($"Deleting {index}");
                result = result && this.DeleteIndex(index);
            }

            _log.Debug("Deletion completed...");

            return result;
        }

        public bool IndexContainsDocuments(string indexName)
        {
            var a = _client.Search<Provider>(s => s.Index(indexName).From(0).Size(1000).MatchAll()).Documents;

            return a.Any();
        }

        public async Task IndexEntries(string indexName, ICollection<Provider> providers)
        {
            foreach (var provider in providers)
            {
                try
                {
                    foreach (var standard in provider.Standards)
                    {
                        var queryList = CreateListRawFormat(provider, standard);
                        foreach (var query in queryList)
                        {
                            await _client.Raw.IndexAsync(indexName, "provider", query);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Error indexing provider: " + ex.Message, ex);
                    throw;
                }
            }
        }

        public bool IndexExists(string indexName)
        {
            return _client.IndexExists(indexName).Exists;
        }

        public void SwapAliasIndex(string aliasName, string newIndexName)
        {
            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(aliasName);
            var aliasRequest = new AliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(
                    new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = aliasName, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = aliasName, Index = newIndexName } });

            _client.Alias(aliasRequest);
        }

        private List<string> CreateListRawFormat(Provider provider, StandardInformation standard)
        {
            var list = new List<string>();

            foreach (var standardLocation in standard.DeliveryLocations)
            {
                var location = standardLocation.DeliveryLocation;

                var i = 0;
                var deliveryModes = new StringBuilder();

                foreach (var deliveryMode in standardLocation.DeliveryModes)
                {
                    deliveryModes.Append(i == 0 ? string.Concat(@"""", EnumHelper.GetDescription(deliveryMode), @"""") : string.Concat(", ", @"""", deliveryMode, @""""));

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
                location.Address.GeoPoint.Longitude,
                ", ",
                location.Address.GeoPoint.Latitude,
                @"],""location"": { ""type"": ""circle"", ""coordinates"": [",
                location.Address.GeoPoint.Longitude,
                ", ",
                location.Address.GeoPoint.Latitude,
                @"], ""radius"": """,
                standardLocation.Radius,
                @"mi"" }}");

                list.Add(rawProvider);
            }

            return list;
        }

    }

    public class ElasticsearchStandardIndexMaintainer : IMaintainSearchIndexes<MetaDataItem>
    {
        private readonly IElasticClient _client;
        private readonly ILog _log;

        public ElasticsearchStandardIndexMaintainer(IElasticsearchClientFactory factory, ILog logger)
        {
            _client = factory.GetElasticClient();
            _log = logger;
        }

        public bool AliasExists(string aliasName)
        {
            var aliasExistsResponse = _client.AliasExists(aliasName);

            return aliasExistsResponse.Exists;
        }

        public void CreateIndex(string indexName)
        {
            _client.CreateIndex(indexName, c => c.AddMapping<StandardDocument>(m => m.MapFromAttributes()));
        }

        public void CreateIndexAlias(string aliasName, string indexName)
        {
            _client.Alias(a => a.Add(add => add.Index(indexName).Alias(aliasName)));
        }

        public bool DeleteIndex(string indexName)
        {
            return _client.DeleteIndex(indexName).Acknowledged;
        }

        public bool DeleteIndexes(Func<string, bool> indexNameMatch)
        {
            var result = true;

            var indicesToBeDelete = _client.IndicesStats().Indices.Select(x => x.Key).Where(indexNameMatch);

            _log.Debug($"Deleting {indicesToBeDelete.Count()} old provider indexes...");

            foreach (var index in indicesToBeDelete)
            {
                _log.Debug($"Deleting {index}");
                result = result && this.DeleteIndex(index);
            }

            _log.Debug("Deletion completed...");

            return result;
        }

        public bool IndexContainsDocuments(string indexName)
        {
            var a = _client.Search<Provider>(s => s.Index(indexName).From(0).Size(1000).MatchAll()).Documents;

            return a.Any();
        }

        public async Task IndexEntries(string indexName, ICollection<MetaDataItem> entries)
        {
            foreach (var standard in entries)
            {
                try
                {
                    var doc = CreateDocument(standard);

                    await _client.IndexAsync(doc, i => i.Index(indexName).Id(doc.StandardId));
                }
                catch (Exception ex)
                {
                    _log.Error("Error indexing standard PDF", ex);
                    throw;
                }
            }
        }

        public bool IndexExists(string indexName)
        {
            return _client.IndexExists(indexName).Exists;
        }

        public void SwapAliasIndex(string aliasName, string newIndexName)
        {
            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(aliasName);
            var aliasRequest = new AliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(
                    new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = aliasName, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = aliasName, Index = newIndexName } });

            _client.Alias(aliasRequest);
        }

        private StandardDocument CreateDocument(MetaDataItem standard)
        {
            try
            {
                var doc = new StandardDocument
                {
                    StandardId = standard.Id,
                    Title = standard.Title,
                    JobRoles = standard.JobRoles,
                    NotionalEndLevel = standard.NotionalEndLevel,
                    PdfFileName = standard.PdfFileName,
                    StandardPdf = standard.StandardPdfUrl,
                    AssessmentPlanPdf = standard.AssessmentPlanPdfUrl,
                    TypicalLength = standard.TypicalLength,
                    IntroductoryText = standard.IntroductoryText,
                    OverviewOfRole = standard.OverviewOfRole,
                    EntryRequirements = standard.EntryRequirements,
                    WhatApprenticesWillLearn = standard.WhatApprenticesWillLearn,
                    Qualifications = standard.Qualifications,
                    ProfessionalRegistration = standard.ProfessionalRegistration,
                };

                return doc;
            }
            catch (Exception ex)
            {
                _log.Error("Error creating document", ex);

                throw;
            }
        }
    }
}
