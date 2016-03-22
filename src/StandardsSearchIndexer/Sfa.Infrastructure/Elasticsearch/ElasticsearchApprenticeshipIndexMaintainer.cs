namespace Sfa.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Nest;

    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Infrastructure.Elasticsearch.Models;

    public sealed class ElasticsearchApprenticeshipIndexMaintainer : ElasticsearchIndexMaintainerBase, IMaintainApprenticeshipIndex
    {
        public ElasticsearchApprenticeshipIndexMaintainer(IElasticsearchClientFactory factory, IElasticsearchMapper elasticsearchMapper, ILog logger)
            : base(factory, elasticsearchMapper, logger, "Standard")
        {
        }

        public override void CreateIndex(string indexName)
        {
            Client.CreateIndex(indexName, c => c
                .AddMapping<StandardDocument>(m => m.MapFromAttributes())
                .AddMapping<FrameworkDocument>(m => m.MapFromAttributes()));
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