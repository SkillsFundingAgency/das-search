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
    public sealed class ElasticsearchStandardIndexMaintainer : ElasticsearchIndexMaintainerBase<MetaDataItem>
    {
        public ElasticsearchStandardIndexMaintainer(IElasticsearchClientFactory factory, ILog logger)
            : base(factory, logger, "Standard")
        {
        }

        public override void CreateIndex(string indexName)
        {
            Client.CreateIndex(indexName, c => c.AddMapping<StandardDocument>(m => m.MapFromAttributes()));
        }

        public override async Task IndexEntries(string indexName, ICollection<MetaDataItem> entries)
        {
            foreach (var standard in entries)
            {
                try
                {
                    var doc = CreateDocument(standard);

                    await Client.IndexAsync(doc, i => i.Index(indexName).Id(doc.StandardId));
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing standard PDF", ex);
                    throw;
                }
            }
        }

        public override bool IndexContainsDocuments(string indexName)
        {
            var a = Client.Search<StandardDocument>(s => s.Index(indexName).From(0).Size(1000).MatchAll()).Documents;

            return a.Any();
        }

        private StandardDocument CreateDocument(MetaDataItem standard)
        {
            try
            {
                var doc = new StandardDocument
                {
                    StandardId = standard.Id,
                    Title = standard.Title,
                    JobRoles = standard.JobRoles,
                    NotionalEndLevel = standard.NotionalEndLevel,
                    PdfFileName = standard.PdfFileName,
                    StandardPdf = standard.StandardPdfUrl,
                    AssessmentPlanPdf = standard.AssessmentPlanPdfUrl,
                    TypicalLength = standard.TypicalLength,
                    IntroductoryText = standard.IntroductoryText,
                    OverviewOfRole = standard.OverviewOfRole,
                    EntryRequirements = standard.EntryRequirements,
                    WhatApprenticesWillLearn = standard.WhatApprenticesWillLearn,
                    Qualifications = standard.Qualifications,
                    ProfessionalRegistration = standard.ProfessionalRegistration,
                };

                return doc;
            }
            catch (Exception ex)
            {
                Log.Error("Error creating document", ex);

                throw;
            }
        }
    }
}
