using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Sfa.Eds.Das.Indexer.ApplicationServices;
using Sfa.Eds.Das.Indexer.Core.Models;
using Sfa.Eds.Das.Indexer.Core.Services;
using Sfa.Infrastructure.Services;

namespace Sfa.Infrastructure.Elasticsearch
{
    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    // ToDo: Rename to more generic Apprenticeship*
    public sealed class ElasticsearchStandardIndexMaintainer : ElasticsearchIndexMaintainerBase<MetaDataItem>
    {
        public ElasticsearchStandardIndexMaintainer(IElasticsearchClientFactory factory, IElasticsearchMapper elasticsearchMapper, ILog logger)
            : base(factory, elasticsearchMapper, logger, "Standard")
        {
        }

        public override void CreateIndex(string indexName)
        {
            Client.CreateIndex(indexName, c => c
                .AddMapping<StandardDocument>(m => m.MapFromAttributes())
                .AddMapping<FrameworkDocument>(m => m.MapFromAttributes()));
        }

        public override async Task IndexEntries<T>(string indexName, ICollection<T> entries)
        {
            if (typeof(T) == typeof(MetaDataItem))
            {
                await IndexMetaDataDocuments(indexName, entries).ConfigureAwait(true);
            }
            else if (typeof(T) == typeof(FrameworkMetaData))
            {
                await IndexFrameworkDocuments(indexName, entries).ConfigureAwait(true);
            }
        }

        public override bool IndexContainsDocuments(string indexName)
        {
            var a = Client.Search<StandardDocument>(s => s.Index(indexName).From(0).Size(10).MatchAll()).Documents;
            return a.Any();
        }

         private async Task IndexMetaDataDocuments<T>(string indexName, ICollection<T> entries)
            where T : IIndexEntry
        {
            foreach (var standard in entries)
            {
                try
                {
                    var doc = ElasticsearchMapper.CreateStandardDocument(standard as MetaDataItem);

                    await Client.IndexAsync(doc, i => i.Index(indexName).Id(doc.StandardId));
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing standard PDF", ex);
                    throw;
                }
            }
        }

        private async Task IndexFrameworkDocuments<T>(string indexName, ICollection<T> entries)
            where T : IIndexEntry
        {
            foreach (var standard in entries)
            {
                try
                {
                    var doc = ElasticsearchMapper.CreateFrameworkDocument(standard as FrameworkMetaData);

                    await Client.IndexAsync(doc, i => i.Index(indexName));
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing framework", ex);
                    throw;
                }
            }
        }
    }
}
