using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool
{
    public class MetaDataManager : IGetStandardMetaData, IGenerateStandardMetaData, IGetFrameworkMetaData
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly ILarsDataService _larsDataService;

        private readonly ILog _logger;

        private readonly IAngleSharpService _angleSharpService;

        private readonly IVstsService _vstsService;

        public MetaDataManager(
            ILarsDataService larsDataService,
            IVstsService vstsService,
            IAppServiceSettings appServiceSettings,
            IAngleSharpService angleSharpService,
            ILog logger)
        {
            _larsDataService = larsDataService;
            _vstsService = vstsService;
            _appServiceSettings = appServiceSettings;
            _logger = logger;
            _angleSharpService = angleSharpService;
        }

        public void GenerateStandardMetadataFiles()
        {
            var currentMetaDataIds = _vstsService.GetExistingStandardIds().ToArray();

            var standards = _larsDataService
                .GetListOfCurrentStandards()
                .Select(MapStandardData)
                .Where(m => !currentMetaDataIds.Contains($"{m.Id}"))
                .ToArray();

            PushStandardsToGit(standards.Select(MapToFileContent).ToList());
        }

        public List<StandardMetaData> GetStandardsMetaData()
        {
            var standards = _vstsService.GetStandards()?.ToList();
            UpdateStandardsInformationFromLars(standards);
            return standards;
        }

        public List<FrameworkMetaData> GetAllFrameworks()
        {
            var frameworks = _larsDataService.GetListOfCurrentFrameworks().ToList();
            UpdateFrameworkInformation(frameworks);
            return frameworks;
        }

        private FileContents MapToFileContent(StandardRepositoryData standardRepositoryData)
        {
            var json = JsonConvert.SerializeObject(standardRepositoryData, Formatting.Indented);
            var standardTitle = Path.GetInvalidFileNameChars().Aggregate(standardRepositoryData.Title, (current, c) => current.Replace(c, '_')).Replace(" ", string.Empty);
            var gitFilePath = $"{_appServiceSettings.VstsGitStandardsFolderPath}/{standardRepositoryData.Id}-{standardTitle}.json";
            return new FileContents(gitFilePath, json);
        }

        private void UpdateFrameworkInformation(IEnumerable<FrameworkMetaData> frameworks)
        {
            var repositoryFrameworks = _vstsService.GetFrameworks().ToArray();
            foreach (var f in frameworks)
            {
                var repositoryFramework = repositoryFrameworks.FirstOrDefault(m =>
                    m.FrameworkCode == f.FworkCode &&
                    m.ProgType == f.ProgType &&
                    m.PathwayCode == f.PwayCode);

                if (repositoryFramework == null)
                {
                    continue;
                }

                f.JobRoleItems = repositoryFramework.JobRoleItems;
                f.TypicalLength = repositoryFramework.TypicalLength;
            }
        }

        private void UpdateStandardsInformationFromLars(IEnumerable<StandardMetaData> standards)
        {
            var currentStandards = _larsDataService.GetListOfCurrentStandards().ToArray();

            foreach (var standard in standards)
            {
                var standardFromLars = currentStandards.SingleOrDefault(m => m.Id.Equals(standard.Id));

                if (standardFromLars == null)
                {
                    continue;
                }

                standard.NotionalEndLevel = standardFromLars.NotionalEndLevel;
                standard.StandardPdfUrl = GetLinkUri(standardFromLars.StandardUrl, "Apprenticeship");
                standard.AssessmentPlanPdfUrl = GetLinkUri(standardFromLars.StandardUrl, "Assessment");
                standard.SectorSubjectAreaTier1 = standardFromLars.SectorSubjectAreaTier1;
                standard.SectorSubjectAreaTier2 = standardFromLars.SectorSubjectAreaTier2;
            }
        }

        private string GetLinkUri(string link, string linkTitle)
        {
            if (string.IsNullOrEmpty(link))
            {
                return string.Empty;
            }

            var uri = _angleSharpService.GetLinks(link.RemoveQuotationMark(), ".attachment-details h2 a", linkTitle)?.FirstOrDefault();

            return uri != null ? new Uri(new Uri("https://www.gov.uk"), uri).ToString() : string.Empty;
        }

        private static StandardRepositoryData MapStandardData(LarsStandard larsStandard)
        {
            var standardRepositoryData = new StandardRepositoryData
            {
                Id = larsStandard.Id,
                Title = larsStandard.Title,
                JobRoles = new List<string>(),
                Keywords = new List<string>(),
                TypicalLength = new TypicalLength {Unit = "m"},
                OverviewOfRole = string.Empty,
                EntryRequirements = string.Empty,
                WhatApprenticesWillLearn = string.Empty,
                Qualifications = string.Empty,
                ProfessionalRegistration = string.Empty
            };
            return standardRepositoryData;
        }

        private void PushStandardsToGit(List<FileContents> standards)
        {
            if (!standards.Any())
            {
                return;
            }

            _vstsService.PushCommit(standards);
            _logger.Info($"Pushed {standards.Count} new meta files to Git Repository.");
        }
    }
}