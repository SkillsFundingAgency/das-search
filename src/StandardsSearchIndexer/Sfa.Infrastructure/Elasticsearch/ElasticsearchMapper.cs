namespace Sfa.Infrastructure.Elasticsearch
{
    using System;

    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public class ElasticsearchMapper : IElasticsearchMapper
    {
        private readonly ILog _logger;

        public ElasticsearchMapper(ILog logger)
        {
            _logger = logger;
        }

        public StandardDocument CreateStandardDocument(MetaDataItem standard)
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
                _logger.Error("Error creating document", ex);

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
