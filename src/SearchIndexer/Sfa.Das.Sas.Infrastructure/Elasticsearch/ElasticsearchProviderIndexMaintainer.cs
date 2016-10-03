using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Nest;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.Core.Exceptions;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
    using System;
    using System.Linq;

    public sealed class ElasticsearchProviderIndexMaintainer : ElasticsearchIndexMaintainerBase, IMaintainProviderIndex
    {
        private readonly IElasticsearchConfiguration _elasticsearchConfiguration;
        private readonly Func<DeliveryInformation, bool> _onlyAtEmployer = x => x.DeliveryModes.All(xx => xx == ModesOfDelivery.OneHundredPercentEmployer);
        private readonly Func<DeliveryInformation, bool> _anyNotAtEmployer = x => x.DeliveryModes.Any(xx => xx != ModesOfDelivery.OneHundredPercentEmployer);

        public ElasticsearchProviderIndexMaintainer(
            IElasticsearchCustomClient elasticsearchClient,
            IElasticsearchMapper elasticsearchMapper,
            ILog log,
            IElasticsearchConfiguration elasticsearchConfiguration)
            : base(elasticsearchClient, elasticsearchMapper, log, "Provider")
        {
            _elasticsearchConfiguration = elasticsearchConfiguration;
        }

        public override void CreateIndex(string indexName)
        {
            var response = Client.CreateIndex(
                indexName,
                i => i
                .Settings(settings => settings
                    .NumberOfShards(_elasticsearchConfiguration.ProviderIndexShards())
                    .NumberOfReplicas(_elasticsearchConfiguration.ProviderIndexReplicas()))
                .Mappings(ms => ms
                    .Map<StandardProvider>(m => m.AutoMap())
                    .Map<FrameworkProvider>(m => m.AutoMap())));

            if (response.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                throw new ConnectionException($"Received non-200 response when trying to create the Apprenticeship Provider Index, Status Code:{response.ApiCall.HttpStatusCode}");
            }
        }

        public async Task IndexEntries(string indexName, ICollection<Provider> indexEntries)
        {
            var bulkStandardTasks = new List<Task<IBulkResponse>>();
            var bulkFrameworkTasks = new List<Task<IBulkResponse>>();
            var bulkProviderTasks = new List<Task<IBulkResponse>>();

            bulkStandardTasks.AddRange(IndexStandards(indexName, indexEntries));
            bulkFrameworkTasks.AddRange(IndexFrameworks(indexName, indexEntries));
            bulkProviderTasks.AddRange(IndexProviders(indexName, indexEntries));

            LogResponse(await Task.WhenAll(bulkStandardTasks), "StandardProvider");
            LogResponse(await Task.WhenAll(bulkFrameworkTasks), "FrameworkProvider");
            LogResponse(await Task.WhenAll(bulkProviderTasks), "ProviderDocument");
        }

        private List<Task<IBulkResponse>> IndexFrameworks(string indexName, ICollection<Provider> indexEntries)
        {
            var bulkProviderLocation = new BulkProviderClient(indexName, Client);

            foreach (var provider in indexEntries)
            {
                foreach (var framework in provider.Frameworks)
                {
                    var deliveryLocationsOnly100 = framework.DeliveryLocations
                        .Where(_onlyAtEmployer)
                        .Where(x => x.DeliveryLocation.Address.GeoPoint != null)
                        .ToArray();

                    if (deliveryLocationsOnly100.Any())
                    {
                        var frameworkProvider = ElasticsearchMapper.CreateFrameworkProviderDocument(provider, framework, deliveryLocationsOnly100);
                        bulkProviderLocation.Create<FrameworkProvider>(c => c.Document(frameworkProvider));
                    }

                    foreach (var location in framework.DeliveryLocations.Where(_anyNotAtEmployer))
                    {
                        if (location.DeliveryLocation.Address.GeoPoint != null)
                        {
                            var frameworkProvider = ElasticsearchMapper.CreateFrameworkProviderDocument(provider, framework, location);
                            bulkProviderLocation.Create<FrameworkProvider>(c => c.Document(frameworkProvider));
                        }
                    }
                }
            }

            return bulkProviderLocation.GetTasks();
        }

        private List<Task<IBulkResponse>> IndexProviders(string indexName, ICollection<Provider> indexEntries)
        {
            var bulkProviderLocation = new BulkProviderClient(indexName, Client);

            foreach (var provider in indexEntries)
            {
                var mappedProvider = ElasticsearchMapper.CreateProviderDocument(provider);
                bulkProviderLocation.Create<ProviderDocument>(c => c.Document(mappedProvider));
            }

            return bulkProviderLocation.GetTasks();
        }

        private List<Task<IBulkResponse>> IndexStandards(string indexName, IEnumerable<Provider> indexEntries)
        {
            var bulkProviderLocation = new BulkProviderClient(indexName, Client);

            foreach (var provider in indexEntries)
            {
                foreach (var standard in provider.Standards)
                {
                    var deliveryLocationsOnly100 = standard.DeliveryLocations
                        .Where(_onlyAtEmployer)
                        .Where(x => x.DeliveryLocation.Address.GeoPoint != null)
                        .ToArray();

                    if (deliveryLocationsOnly100.Any())
                    {
                        var standardProvider = ElasticsearchMapper.CreateStandardProviderDocument(provider, standard, deliveryLocationsOnly100);
                        bulkProviderLocation.Create<StandardProvider>(c => c.Document(standardProvider));
                    }

                    foreach (var location in standard.DeliveryLocations.Where(_anyNotAtEmployer))
                    {
                        if (location.DeliveryLocation.Address.GeoPoint != null)
                        {
                            var standardProvider = ElasticsearchMapper.CreateStandardProviderDocument(provider, standard, location);
                            bulkProviderLocation.Create<StandardProvider>(c => c.Document(standardProvider));
                        }
                    }
                }
            }

            return bulkProviderLocation.GetTasks();
        }
    }
}