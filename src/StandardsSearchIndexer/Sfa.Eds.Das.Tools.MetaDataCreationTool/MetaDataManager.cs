namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class MetaDataManager : IGetStandardMetaData, IGenerateStandardMetaData, IGetFrameworkMetaData
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly ILarsDataService _larsDataService;

        private readonly ILog _logger;

        private readonly IVstsService _vstsService;

        public MetaDataManager(ILarsDataService larsDataService, IVstsService vstsService, IAppServiceSettings appServiceSettings, ILog logger)
        {
            _larsDataService = larsDataService;
            _vstsService = vstsService;
            _appServiceSettings = appServiceSettings;
            _logger = logger;
        }

        public void GenerateStandardMetadataFiles()
        {
            var currentStandards = _larsDataService.GetListOfCurrentStandards();
            _logger.Info($"Got {currentStandards.Count()} current live standards from LARS data.");

            var currentMetaDataIds = _vstsService.GetExistingStandardIds();
            _logger.Info($"Got {currentMetaDataIds.Count()} current meta data files from vsts.");

            var missingStandards = DetermineMissingMetaData(currentStandards, currentMetaDataIds);
            _logger.Info($"There are {missingStandards.Count} meta data files that need to be created.");

            PushStandardsToGit(missingStandards);
            _logger.Info($"Pushed new meta files to Git Repository.");
        }

        public IDictionary<string, string> GetAllAsJson()
        {
            return _vstsService.GetStandards();
        }

        public List<FrameworkMetaData> GetAllFrameworks()
        {
            return FilterFrameworks(_larsDataService.GetListOfCurrentFrameworks());
        }

        private List<FileContents> DetermineMissingMetaData(IEnumerable<Standard> currentStandards, IEnumerable<string> currentMetaDataIds)
        {
            var missingStandards = new List<FileContents>();

            foreach (var standard in currentStandards.Where(m => !currentMetaDataIds.Contains($"{m.Id}")))
            {
                var json = JsonConvert.SerializeObject(standard, Formatting.Indented);
                var standardTitle = Path.GetInvalidFileNameChars().Aggregate(standard.Title, (current, c) => current.Replace(c, '_')).Replace(" ", string.Empty);
                var gitFilePath = $"{_appServiceSettings.VstsGitFolderPath}/{standard.Id}-{standardTitle}.json";
                missingStandards.Add(new FileContents(gitFilePath, json));
            }

            return missingStandards;
        }

        private void PushStandardsToGit(List<FileContents> standards)
        {
            if (standards.Any())
            {
                _vstsService.PushCommit(standards);
            }
        }

        private List<FrameworkMetaData> FilterFrameworks(List<FrameworkMetaData> frameworks)
        {
            var progTypeList = new[] { 2, 3, 20, 21, 22, 23 };

            return frameworks.Where(s => s.FworkCode > 399)
                .Where(s => s.PwayCode > 0)
                .Where(s => !s.EffectiveFrom.Equals(DateTime.MinValue))
                .Where(s => s.EffectiveTo.Equals(DateTime.MinValue) || s.EffectiveTo > DateTime.Now)
                .Where(s => progTypeList.Contains(s.ProgType))
                .ToList();
        }
    }
}