using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Nest;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.Core.Exceptions;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
    public sealed class ElasticsearchProviderIndexMaintainer : ElasticsearchIndexMaintainerBase, IMaintainProviderIndex
    {
        public ElasticsearchProviderIndexMaintainer(IElasticsearchCustomClient elasticsearchClient, IElasticsearchMapper elasticsearchMapper, ILog log)
            : base(elasticsearchClient, elasticsearchMapper, log, "Provider")
        {
        }

        public override void CreateIndex(string indexName)
        {
            var response = Client.CreateIndex(indexName, i => i
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

            bulkStandardTasks.AddRange(IndexStandards(indexName, indexEntries));
            bulkFrameworkTasks.AddRange(IndexFrameworks(indexName, indexEntries));

            LogResponse(await Task.WhenAll(bulkStandardTasks), "StandardProvider");
            LogResponse(await Task.WhenAll(bulkFrameworkTasks), "FrameworkProvider");
        }

        private List<Task<IBulkResponse>> IndexFrameworks(string indexName, ICollection<Provider> indexEntries)
        {
            var tasks = new List<Task<IBulkResponse>>();
            int count = 0;
            var bulkDescriptor = CreateBulkDescriptor(indexName);

            foreach (var provider in indexEntries)
            {
                foreach (var framework in provider.Frameworks)
                {
                    foreach (var location in framework.DeliveryLocations)
                    {
                        // Add standard to descriptor
                        var frameworkProvider = ElasticsearchMapper.CreateFrameworkProviderDocument(provider, framework, location);
                        bulkDescriptor.Create<FrameworkProvider>(c => c.Document(frameworkProvider));
                        count++;

                        if (HaveReachedBatchLimit(count))
                        {
                            // Execute batch
                            tasks.Add(Client.BulkAsync(bulkDescriptor));

                            // New descriptor
                            bulkDescriptor = CreateBulkDescriptor(indexName);

                            count = 0;
                        }
                    }
                }
            }

            if (count > 0)
            {
                tasks.Add(Client.BulkAsync(bulkDescriptor));
            }

            return tasks;
        }

        private List<Task<IBulkResponse>> IndexStandards(string indexName, IEnumerable<Provider> indexEntries)
        {
            var tasks = new List<Task<IBulkResponse>>();
            int count = 0;
            var bulkDescriptor = CreateBulkDescriptor(indexName);

            foreach (var provider in indexEntries)
            {
                foreach (var standard in provider.Standards)
                {
                    foreach (var location in standard.DeliveryLocations)
                    {
                        // Add standard to descriptor
                        var standardProvider = ElasticsearchMapper.CreateStandardProviderDocument(provider, standard, location);
                        bulkDescriptor.Create<StandardProvider>(c => c.Document(standardProvider));
                        count++;

                        if (HaveReachedBatchLimit(count))
                        {
                            // Execute batch
                            tasks.Add(Client.BulkAsync(bulkDescriptor));

                            // New descriptor
                            bulkDescriptor = CreateBulkDescriptor(indexName);

                            count = 0;
                        }
                    }
                }
            }

            if (count > 0)
            {
                tasks.Add(Client.BulkAsync(bulkDescriptor));
            }

            return tasks;
        }
    }
}