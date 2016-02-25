namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;
    using Newtonsoft.Json;
    using System.IO;
    public class MetaDataManager : IGetStandardMetaData, IGenerateStandardMetaData
    {
        private readonly ILarsDataService _larsDataService;
        private readonly IVstsService _vstsService;
        private readonly ISettings _settings;

        public MetaDataManager()
        {
            var container = ContainerBootstrapper.BootstrapStructureMap();
            _larsDataService = container.GetInstance<ILarsDataService>();
            _vstsService = container.GetInstance<IVstsService>();
            _settings = container.GetInstance<ISettings>();
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
            var currentMetaDataIds = _vstsService.GetStandardObjectsIds();

            var missingStandards = DetermineMissingMetaData(currentStandards, currentMetaDataIds);

            PushStandardsToGit(missingStandards);
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
