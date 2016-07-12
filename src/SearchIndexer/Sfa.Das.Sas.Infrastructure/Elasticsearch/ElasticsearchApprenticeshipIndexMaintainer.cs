using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Nest;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.Core.Exceptions;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
    using System.Globalization;

    using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration;

    public sealed class ElasticsearchApprenticeshipIndexMaintainer : ElasticsearchIndexMaintainerBase, IMaintainApprenticeshipIndex
    {
        private readonly IElasticsearchConfiguration _elasticsearchConfiguration;

        public ElasticsearchApprenticeshipIndexMaintainer(
            IElasticsearchCustomClient elasticsearchClient,
            IElasticsearchMapper elasticsearchMapper,
            ILog logger,
            IElasticsearchConfiguration elasticsearchConfiguration)
            : base(elasticsearchClient, elasticsearchMapper, logger, "Apprenticeship")
        {
            _elasticsearchConfiguration = elasticsearchConfiguration;
        }

        public override void CreateIndex(string indexName)
        {
            var response = Client.CreateIndex(indexName, i => i
                .Settings(settings => settings
                    .Analysis(a => _elasticsearchConfiguration.ApprenticeshipAnalysisDescriptor()))
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
            var bulkProviderClient = new BulkProviderClient(indexName, Client);

            foreach (var entry in entries)
            {
                try
                {
                    var doc = method(entry);

                    bulkProviderClient.Create<T2>(c => c.Document(doc));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error indexing {typeof(T1)}");
                }
            }

            var bulkTasks = new List<Task<IBulkResponse>>();
            bulkTasks.AddRange(bulkProviderClient.GetTasks());
            LogResponse(await Task.WhenAll(bulkTasks), typeof(T1).Name.ToLower(CultureInfo.CurrentCulture));
        }
    }
}