namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Indexer.ApplicationServices.MetaData;
    using Indexer.ApplicationServices.Settings;
    using Indexer.Core.Logging;
    using Indexer.Core.Models;
    using Interfaces;
    using Models.Git;
    using Newtonsoft.Json;

    public class VstsService : IVstsService
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly IGitDynamicModelGenerator _gitDynamicModelGenerator;

        private readonly IJsonMetaDataConvert _jsonMetaDataConvert;

        private readonly IVstsClient _vstsClient;

        private readonly ILog _logger;

        public VstsService(
            IAppServiceSettings appServiceSettings,
            IGitDynamicModelGenerator gitDynamicModelGenerator,
            IJsonMetaDataConvert jsonMetaDataConvert,
            IVstsClient vstsClient,
            ILog logger)
        {
            _appServiceSettings = appServiceSettings;
            _gitDynamicModelGenerator = gitDynamicModelGenerator;
            _jsonMetaDataConvert = jsonMetaDataConvert;
            _vstsClient = vstsClient;
            _logger = logger;
        }

        public IEnumerable<string> GetExistingStandardIds()
        {
            var blobs = GetAllBlobs(_appServiceSettings.VstsGitGetFilesUrl);

            var result = blobs?.Select(m => GetIdFromPath(m.Path)) ?? new List<string>();
            _logger.Info($"Got {result.Count()} current meta data files Git Repository.");

            return result;
        }

        public IEnumerable<StandardMetaData> GetStandards()
        {
            var standardsDictionary = GetAllFileContents(_appServiceSettings.VstsGitGetFilesUrl);
            return _jsonMetaDataConvert.DeserializeObject<StandardMetaData>(standardsDictionary);
        }

        public IEnumerable<VstsFrameworkMetaData> GetFrameworks()
        {
            try
            {
                var frameworksDir = GetAllFileContents(_appServiceSettings.VstsGitGetFrameworkFilesUrl);
                return _jsonMetaDataConvert.DeserializeObject<VstsFrameworkMetaData>(frameworksDir);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting framework meta data");
            }

            return new List<VstsFrameworkMetaData>();
        }

        public void PushCommit(List<FileContents> items)
        {
            var body = _gitDynamicModelGenerator.GenerateCommitBody(_appServiceSettings.GitBranch, _vstsClient.GetLatesCommit(), items);
            _vstsClient.Post(_appServiceSettings.VstsGitPushUrl, _appServiceSettings.GitUsername, _appServiceSettings.GitPassword, body);
        }

        public IDictionary<string, string> GetAllFileContents(string vstsBlobUrl)
        {
            var blobs = GetAllBlobs(vstsBlobUrl);

            if (blobs == null)
            {
                return new Dictionary<string, string>(0);
            }

            var standardsAsJson = new Dictionary<string, string>();

            foreach (var blob in blobs)
            {
                var str = _vstsClient.Get(blob.Url);
                standardsAsJson.Add(blob.Path, str);
            }

            return standardsAsJson;
        }

        // Helpers
        private IEnumerable<Entity> GetAllBlobs(string vstsUrl)
        {
            var folderTreeStr = _vstsClient.Get(vstsUrl);
            var tree = JsonConvert.DeserializeObject<GitTree>(folderTreeStr);

            return tree?.Value.Where(x => x.IsBlob);
        }

        private string GetIdFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var i = path.LastIndexOf("/", StringComparison.Ordinal);
            if (i < 0)
            {
                return string.Empty;
            }

            var str = path.Substring(i);
            var standardId = str.Split('-')[0].Replace("/", string.Empty);
            int outData;
            if (int.TryParse(standardId, out outData))
            {
                return standardId;
            }

            return string.Empty;
        }
    }
}