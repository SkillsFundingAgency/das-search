namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class MetaDataManager : IGetStandardMetaData, IGenerateStandardMetaData
    {
        private readonly ILarsDataService larsDataService;
        private readonly IVstsService vstsService;
        private readonly ISettings settings;

        public MetaDataManager()
        {
            var container = ContainerBootstrapper.BootstrapStructureMap();
            larsDataService = container.GetInstance<ILarsDataService>();
            vstsService = container.GetInstance<IVstsService>();
            settings = container.GetInstance<ISettings>();
        }

        public IEnumerable<string> GetAllAsJson()
        {
            return vstsService.GetStandards();
        }

        /// <summary>
        ///
        ///    Will
        ///    - download zip file from course director and unzip standard.csv file.
        ///    - Creates metadata json for new standards and then push them to git repository
        /// </summary>
        public void GenerateStandardMetadataFiles()
        {
            var zipFilePath = larsDataService.GetZipFilePath();
            var extractedPath = larsDataService.DownloadZipFile(zipFilePath);
            var ids = vstsService.GetStandardObjectsIds();
            var standards = larsDataService.GenerateJsons(extractedPath, ids);
            PushStandardsToGit(standards);
        }

        private void PushStandardsToGit(List<StandardObject> standards)
        {
            if (standards.Any())
            {
                vstsService.PushCommit(standards);
            }
        }
    }
}
