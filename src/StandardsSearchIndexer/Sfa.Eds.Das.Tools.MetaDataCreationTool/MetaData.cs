namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Vsts;

    public class MetaData : IMetaData
    {
        private readonly ILarsDataService larsDataService;
        private readonly IVstsService vstsService;
        private readonly ISettings settings;

        public MetaData()
        {
            var container = ContainerBootstrapper.BootstrapStructureMap();
            larsDataService = container.GetInstance<ILarsDataService>();
            vstsService = container.GetInstance<IVstsService>();
            settings = container.GetInstance<ISettings>();
        }

        public IEnumerable<string> GetStandards()
        {
            return vstsService.GetStandards();
        }

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
                var oldObjectId = vstsService.GetLatesCommit();
                var git = new GitDynamicModel();
                var body = git.GenerateCommitBody(settings.GitBranch, oldObjectId, standards);
                vstsService.PushCommit(body);
            }
        }
    }
}
