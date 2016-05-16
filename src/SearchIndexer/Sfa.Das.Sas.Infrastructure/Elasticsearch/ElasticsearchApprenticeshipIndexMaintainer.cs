using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Nest;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.Core.Exceptions;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
    using System.Globalization;

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
            await IndexApprenticeships(indexName, entries, ElasticsearchMapper.CreateStandardDocument).ConfigureAwait(true);
        }

        public async Task IndexFrameworks(string indexName, ICollection<FrameworkMetaData> entries)
        {
            await IndexApprenticeships(indexName, entries, ElasticsearchMapper.CreateFrameworkDocument).ConfigureAwait(true);
        }

        private async Task IndexApprenticeships<T1, T2>(string indexName, ICollection<T1> entries, Func<T1, T2> method)
            where T1 : class
            where T2 : class
        {
            var tasks = new List<Task<IBulkResponse>>();
            int count = 0;
            int totalCount = 0;
            var bulkDescriptor = CreateBulkDescriptor(indexName);

            foreach (var entry in entries)
            {
                try
                {
                    var doc = method(entry);

                    bulkDescriptor.Create<T2>(c => c.Document(doc));
                    count++;

                    if (HaveReachedBatchLimit(count))
                    {
                        // Execute batch
                        tasks.Add(Client.BulkAsync(bulkDescriptor));
                        totalCount += count;

                        // Reset state - New descriptor
                        bulkDescriptor = CreateBulkDescriptor(indexName);
                        count = 0;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error indexing {typeof(T1)}");
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
            var properties = new Dictionary<string, object> { { "DocumentType", typeof(T1).Name.ToLower(CultureInfo.CurrentCulture) }, { "TotalCount", totalCount }, { "Identifier", "DocumentCount" } };
            Log.Info($"Sent a total of {totalCount} {typeof(T1)} documents to be indexed", properties);
        }
    }
}