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
                    Level = MapLevel(frameworkMetaData.ProgType)
                };

                return doc;
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating document", ex);

                throw;
            }
        }

        public int MapRevertLevel(int level)
        {
            switch (level)
            {
                case 3:
                    return 2;
                case 2:
                    return 3;
                case 20:
                    return 4;
                case 21:
                    return 5;
                case 22:
                    return 6;
                case 23:
                    return 7;
                default:
                    return -1;
            }
        }

        private int MapLevel(int level)
        {
            switch (level)
            {
                case 3:
                    return 2;
                case 2:
                    return 3;
                case 4:
                    return 20;
                case 5:
                    return 21;
                case 6:
                    return 22;
                case 7:
                    return 23;
                default:
                    return -1;
            }
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
