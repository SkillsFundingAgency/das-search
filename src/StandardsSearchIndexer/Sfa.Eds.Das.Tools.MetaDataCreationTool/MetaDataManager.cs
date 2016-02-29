namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;
    using Newtonsoft.Json;

    public class MetaDataManager : IGetStandardMetaData, IGenerateStandardMetaData
    {
        private readonly ILarsDataService _larsDataService;
        private readonly IVstsService _vstsService;
        private readonly ISettings _settings;
        private readonly ILog4NetLogger _logger;

        public MetaDataManager(ILarsDataService larsDataService, IVstsService vstsService, ISettings settings, ILog4NetLogger logger)
        {
            _larsDataService = larsDataService;
            _vstsService = vstsService;
            _settings = settings;
            _logger = logger;
        }

        public IEnumerable<string> GetAllAsJson()
        {
            return _vstsService.GetStandards();
        }

        /// <summary>
        ///
        ///    Will
        ///    - download zip file from course directory and unzip standard.csv file.
        ///    - Creates metadata json for new standards and then push them to git repository
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

        private List<StandardObject> DetermineMissingMetaData(IEnumerable<Standard> currentStandards, IEnumerable<string> currentMetaDataIds)
        {
            var missingStandards = new List<StandardObject>();

            foreach (var standard in currentStandards.Where(m => !currentMetaDataIds.Contains($"{m.Id}")))
            {
                var json = JsonConvert.SerializeObject(standard, Formatting.Indented);
                var standardTitle = Path.GetInvalidFileNameChars().Aggregate(standard.Title, (current, c) => current.Replace(c, '_')).Replace(" ", string.Empty);
                var gitFilePath = $"{_settings.VstsGitFolderPath}/{standard.Id}-{standardTitle}.json";
                missingStandards.Add(new StandardObject(gitFilePath, json));
            }

            return missingStandards;
        }

        private void PushStandardsToGit(List<StandardObject> standards)
        {
            if (standards.Any())
            {
                _vstsService.PushCommit(standards);
            }
        }
    }
}
