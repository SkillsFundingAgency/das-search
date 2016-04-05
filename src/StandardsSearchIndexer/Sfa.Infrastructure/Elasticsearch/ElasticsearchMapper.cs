using Sfa.Eds.Das.Indexer.Core.Elasticsearch;

namespace Sfa.Infrastructure.Elasticsearch
{
    using System;

    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.Elasticsearch.Models;

    public class ElasticsearchMapper : IElasticsearchMapper
    {
        private readonly ILog _logger;

        public ElasticsearchMapper(ILog logger)
        {
            _logger = logger;
        }

        public StandardDocument CreateStandardDocument(StandardMetaData standard)
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
                _logger.Error("Error creating document", ex);

                throw;
            }
        }

        public FrameworkDocument CreateFrameworkDocument(FrameworkMetaData frameworkMetaData)
        {
            try
            {
                var doc = new FrameworkDocument
                {
                    FrameworkId = $"{frameworkMetaData.FworkCode}{frameworkMetaData.ProgType}{frameworkMetaData.PwayCode}",
                    Title = CreateFrameworkTitle(frameworkMetaData.NASTitle, frameworkMetaData.PathwayName),
                    FrameworkCode = frameworkMetaData.FworkCode,
                    FrameworkName = frameworkMetaData.NASTitle,
                    PathwayCode = frameworkMetaData.PwayCode,
                    PathwayName = frameworkMetaData.PathwayName,
                    Level = MapLevelProgType(frameworkMetaData.ProgType)
                };

                return doc;
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating document", ex);

                throw;
            }
        }

        public int MapLevelProgType(int level)
        {
            var em = new ElasticMapper();
            return em.MapLevel(level);
        }

        private string CreateFrameworkTitle(string framworkname, string pathwayName)
        {
            if (framworkname.Equals(pathwayName) || string.IsNullOrWhiteSpace(pathwayName))
            {
                return framworkname;
            }

            return $"{framworkname}: {pathwayName}";
        }
    }
}
