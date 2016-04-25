namespace Sfa.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Eds.Das.Indexer.Core.Exceptions;

    using Nest;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.Elasticsearch.Models;

    public sealed class ElasticsearchApprenticeshipIndexMaintainer : ElasticsearchIndexMaintainerBase, IMaintainApprenticeshipIndex
    {
        public ElasticsearchApprenticeshipIndexMaintainer(IElasticsearchCustomClient elasticsearchClient, IElasticsearchMapper elasticsearchMapper, ILog logger)
            : base(elasticsearchClient, elasticsearchMapper, logger, "Apprenticeship")
        {
        }

        public override void CreateIndex(string indexName)
        {
            var response = Client.CreateIndex(indexName, i => i
                .Mappings(ms => ms
                    .Map<StandardDocument>(m => m.AutoMap())
                    .Map<FrameworkDocument>(m => m.AutoMap())));

            if (response.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                throw new ConnectionException($"Received non-200 response when trying to create the Apprenticeship Index, Status Code:{response.ApiCall.HttpStatusCode}");
            }
        }

        public async Task IndexStandards(string indexName, ICollection<StandardMetaData> entries)
        {
            var tasks = new List<Task<IBulkResponse>>();
            int count = 0;
            int totalCount = 0;
            var bulkDescriptor = CreateBulkDescriptor(indexName);

            foreach (var standard in entries)
            {
                try
                {
                    var doc = ElasticsearchMapper.CreateStandardDocument(standard);
                    bulkDescriptor.Create<StandardDocument>(c => c.Document(doc));
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
                catch (Exception ex)
                {
                    Log.Error("Error indexing standard PDF", ex);
                }
            }

            if (count > 0)
            {
                tasks.Add(Client.BulkAsync(bulkDescriptor));
                totalCount += count;
            }

            var bulkTasks = new List<Task<IBulkResponse>>();
            bulkTasks.AddRange(tasks);
            LogResponse(await Task.WhenAll(bulkTasks));

            Log.Debug($"Sent a total of {totalCount} Standard documents to be indexed");
        }

        public async Task IndexFrameworks(string indexName, ICollection<FrameworkMetaData> entries)
        {
            var tasks = new List<Task<IBulkResponse>>();
            int count = 0;
            int totalCount = 0;
            var bulkDescriptor = CreateBulkDescriptor(indexName);

            foreach (var standard in entries)
            {
                try
                {
                    var doc = ElasticsearchMapper.CreateFrameworkDocument(standard);

                    bulkDescriptor.Create<FrameworkDocument>(c => c.Document(doc));
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
                catch (Exception ex)
                {
                    Log.Error("Error indexing framework", ex);
                    throw;
                }
            }

            if (count > 0)
            {
                tasks.Add(Client.BulkAsync(bulkDescriptor));
                totalCount += count;
            }

            var bulkTasks = new List<Task<IBulkResponse>>();
            bulkTasks.AddRange(tasks);
            LogResponse(await Task.WhenAll(bulkTasks));

            Log.Debug($"Sent a total of {totalCount} Framework documents to be indexed");
        }
    }
}