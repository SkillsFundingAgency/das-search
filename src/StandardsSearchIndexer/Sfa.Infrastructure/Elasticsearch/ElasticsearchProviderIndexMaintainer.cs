using System.Linq;

namespace Sfa.Infrastructure.Elasticsearch
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Nest;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public sealed class ElasticsearchProviderIndexMaintainer : ElasticsearchIndexMaintainerBase, IMaintainProviderIndex
    {
        private readonly IIndexSettings<IMaintainProviderIndex> _settings;

        public ElasticsearchProviderIndexMaintainer(IElasticsearchClientFactory factory, IElasticsearchMapper elasticsearchMapper, IIndexSettings<IMaintainProviderIndex> settings, ILog log)
            : base(factory, elasticsearchMapper, log, "Provider")
        {
            _settings = settings;
        }

        public override void CreateIndex(string indexName)
        {
            Client.CreateIndex(indexName, i => i
                .Mappings(ms => ms
                    .Map<StandardProvider>(m => m.AutoMap())
                    .Map<FrameworkProvider>(m => m.AutoMap())));
        }

        public async Task IndexEntries(string indexName, ICollection<Provider> indexEntries)
        {
            LogResponse(await Task.WhenAll(IndexStandards(indexName, indexEntries)));
            LogResponse(await Task.WhenAll(IndexFrameworks(indexName, indexEntries)));
        }

        private void LogResponse(IBulkResponse[] elementIndexResult)
        {
            foreach (var bulkResponse in elementIndexResult.Where(bulkResponse => bulkResponse.Errors))
            {
                ReportErrors(bulkResponse);
            }
        }

        private static BulkDescriptor CreateBulkDescriptor(string indexName)
        {
            var bulkDescriptor = new BulkDescriptor();
            bulkDescriptor.Index(indexName);

            return bulkDescriptor;
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
            }

            Log.Debug($"Sent a total of {totalCount} Framework Provider documents to be indexed");

            return tasks;
        }

        private void ReportErrors(IBulkResponse result)
        {
            foreach (var message in result.ItemsWithErrors.Select(itemsWithError => string.Concat("Error indexing entry ", itemsWithError.Id, " at ", itemsWithError.Index)))
            {
                Log.Warn(message);
            }
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
            }

            Log.Debug($"Sent a total of {totalCount} Standard Provider documents to be indexed");

            return tasks;
        }

        private bool HaveReachedBatchLimit(int count)
        {
            int batchSize = 4000;

            return count >= batchSize;
        }
    }
}