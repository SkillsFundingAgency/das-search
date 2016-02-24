namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Vsts;

    public interface IMetaData
    {
        /// <summary>
        /// 
        ///    Will 
        ///    - download zip file from course director and unzip standard.csv file.  
        ///    - Creates metadata json for new standards and then push them to git repository
        /// </summary>
        string GenerateStandardMetadataFiles(IEnumerable<string> ids);
        

        /// <summary>
        /// Returns all standards 
        /// </summary>
        IEnumerable<string> GetStandards();
    }

    public class MetaData : IMetaData
    {
        private readonly IMetaDataCreation _meta;
        private readonly IVstsService _vstsService;
        private readonly ISettings _settings;
        public MetaData()
        {
            var container = ContainerBootstrapper.BootstrapStructureMap();
            _meta = container.GetInstance<IMetaDataCreation>();
            _vstsService = container.GetInstance<IVstsService>();
            _settings = container.GetInstance<ISettings>();
        }

        public void Start()
        {
            var ids = GetExistingStandardIds();
            ids = new List<string>(); // Delete
            var zipFilePath = _meta.GetZipFilePath();
            var extractedPath = _meta.DownloadZipFile(zipFilePath);
            var standards = _meta.GenerateJsons(extractedPath, ids);
            {
                var oldObjectId = _vstsService.GetLatesCommit();
                var git = new GitDynamicModel();
                var body = git.GenerateCommitBody(_settings.GitBranch, oldObjectId, standards);
                _vstsService.PushCommit(body);
            }
        }

        public IEnumerable<string> GetStandards()
        {
            return _vstsService.GetStandards();
        }

        public IEnumerable<string> GetExistingStandardIds()
        {
            return _vstsService.GetStandardObjectsIds();
        }

        public string GenerateStandardMetadataFiles(IEnumerable<string> ids)
        {
            var zipFilePath = _meta.GetZipFilePath();
            var extractedPath = _meta.DownloadZipFile(zipFilePath);
            var items = _meta.GenerateJsons(extractedPath, ids);
            var git = new GitDynamicModel();
            return git.GenerateCommitBody("", "", items);
        }
    }
}
