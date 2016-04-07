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
        public ElasticsearchApprenticeshipIndexMaintainer(IElasticsearchClientFactory factory, IElasticsearchMapper elasticsearchMapper, ILog logger)
            : base(factory, elasticsearchMapper, logger, "Apprenticeship")
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
            foreach (var standard in entries)
            {
                try
                {
                    var doc = ElasticsearchMapper.CreateStandardDocument(standard);

                    await Client.IndexAsync(doc, i => i.Index(indexName).Id(doc.StandardId));
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing standard PDF", ex);
                    throw;
                }
            }
        }

        public async Task IndexFrameworks(string indexName, ICollection<FrameworkMetaData> entries)
        {
            foreach (var standard in entries)
            {
                try
                {
                    var doc = ElasticsearchMapper.CreateFrameworkDocument(standard);

                    await Client.IndexAsync(doc, i => i.Index(indexName));
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing framework", ex);
                    throw;
                }
            }
        }

        public override bool IndexContainsDocuments(string indexName)
        {
            var a = Client.Search<StandardDocument>(s => s.Index(indexName).From(0).Size(10).MatchAll()).Documents;
            return a.Any();
        }
    }
}