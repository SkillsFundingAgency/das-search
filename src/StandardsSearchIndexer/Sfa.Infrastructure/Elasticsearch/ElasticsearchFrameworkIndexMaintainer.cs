using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.ApplicationServices;
using Sfa.Eds.Das.Indexer.Core.Services;
using Sfa.Infrastructure.Services;

namespace Sfa.Infrastructure.Elasticsearch
{
    using NLog;

    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    public sealed class ElasticsearchFrameworkIndexMaintainer : ElasticsearchIndexMaintainerBase<FrameworkMetaData>
    {
        private readonly ILog _logger;

        public ElasticsearchFrameworkIndexMaintainer(IElasticsearchClientFactory factory, ILog logger)
            : base(factory, logger, "Framework")
        {
            _logger = logger;
        }

        public override void CreateIndex(string indexName)
        {
            _logger.Warn("CreateIndex in ElasticsearchFrameworkIndexMaintainer should not be used");
        }

        public override bool IndexContainsDocuments(string indexName)
        {
            var a = Client.Search<FrameworkDocument>(s => s.Index(indexName).From(0).Size(10).MatchAll()).Documents;

            return a.Any();
        }

        public override async Task IndexEntries(string indexName, ICollection<FrameworkMetaData> entries)
        {
            foreach (var framework in entries)
            {
                try
                {
                    var doc = CreateDocument(framework);

                    await Client.IndexAsync(doc, i => i.Index(indexName));
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing standard PDF", ex);
                    throw;
                }
            }
        }

        private FrameworkDocument CreateDocument(FrameworkMetaData frameworkMetaData)
        {

            try
            {
                var doc = new FrameworkDocument
                {
                    Title = CreateFrameworkTitle(frameworkMetaData.IssuingAuthorityTitle, frameworkMetaData.PathwayName),
                    FrameworkCode = frameworkMetaData.FworkCode,
                    FrameworkName = frameworkMetaData.NASTitle,
                    PathwayCode = frameworkMetaData.PwayCode,
                    PathwayName = frameworkMetaData.PathwayName
                };

                return doc;
            }
            catch (Exception ex)
            {
                Log.Error("Error creating document", ex);

                throw;
            }
        }

        private string CreateFrameworkTitle(string framworkname, string pathwayName)
        {
            if (framworkname.Equals(pathwayName))
            {
                return framworkname;
            }

            return $"{framworkname}: {pathwayName}";
        }
    }
}
