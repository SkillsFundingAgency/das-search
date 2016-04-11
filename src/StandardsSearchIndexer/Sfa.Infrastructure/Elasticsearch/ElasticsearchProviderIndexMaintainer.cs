using System.Linq;

namespace Sfa.Infrastructure.Elasticsearch
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Eds.Das.Indexer.Core.Exceptions;
    using Models;
    using Nest;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

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
            var bulkTasks = new List<Task<IBulkResponse>>();
            bulkTasks.AddRange(IndexStandards(indexName, indexEntries));
            bulkTasks.AddRange(IndexFrameworks(indexName, indexEntries));

            LogResponse(await Task.WhenAll(bulkTasks));
        }

        private List<Task<IBulkResponse>> IndexFrameworks(string indexName, ICollection<Provider> indexEntries)
        {
            var tasks = new List<Task<IBulkResponse>>();
            int count = 0;
            int totalCount = 0;
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

                            totalCount += count;
                            count = 0;
                        }
                    }
                }
            }

            if (count > 0)
            {
                tasks.Add(Client.BulkAsync(bulkDescriptor));
                totalCount += count;
            }

            Log.Debug($"Sent a total of {totalCount} Framework Provider documents to be indexed");

            return tasks;
        }

        private List<Task<IBulkResponse>> IndexStandards(string indexName, IEnumerable<Provider> indexEntries)
        {
            var tasks = new List<Task<IBulkResponse>>();
            int count = 0;
            int totalCount = 0;
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

                            totalCount += count;
                            count = 0;
                        }
                    }
                }
            }

            if (count > 0)
            {
                tasks.Add(Client.BulkAsync(bulkDescriptor));
                totalCount += count;
            }

            Log.Debug($"Sent a total of {totalCount} Standard Provider documents to be indexed");

            return tasks;
        }
    }
}