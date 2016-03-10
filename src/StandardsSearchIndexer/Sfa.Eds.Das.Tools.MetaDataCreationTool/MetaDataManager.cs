namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Indexer.Core.Services;
    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class MetaDataManager : IGetStandardMetaData, IGenerateStandardMetaData
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

        /// <summary>
        ///     Will
        ///     - download zip file from course directory and unzip standard.csv file.
        ///     - Creates metadata json for new standards and then push them to git repository
        /// </summary>
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

        private List<FileContents> DetermineMissingMetaData(IEnumerable<Standard> currentStandards, IEnumerable<string> currentMetaDataIds)
        {
            var missingStandards = new List<FileContents>();

            foreach (var standard in currentStandards.Where(m => !currentMetaDataIds.Contains($"{m.Id}")))
            {
                var json = JsonConvert.SerializeObject(standard, Formatting.Indented);
                var standardTitle = Path.GetInvalidFileNameChars()
                    .Aggregate(standard.Title, (current, c) => current.Replace(c, '_'))
                    .Replace(" ", string.Empty);
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
    }
}