using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    public class VstsService : IVstsService
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly IGitDynamicModelGenerator _gitDynamicModelGenerator;

        private readonly IHttpGet _httpHelper;

        private readonly ILog _logger;

        public VstsService(
            IAppServiceSettings appServiceSettings,
            IGitDynamicModelGenerator gitDynamicModelGenerator,
            IHttpGet httpHelper,
            ILog logger)
        {
            _appServiceSettings = appServiceSettings;
            _gitDynamicModelGenerator = gitDynamicModelGenerator;
            _httpHelper = httpHelper;
            _logger = logger;
        }

        public IEnumerable<string> GetExistingStandardIds()
        {
            var blobs = GetAllBlobs();

            return blobs?.Select(m => GetIdFromPath(m.Path)) ?? new List<string>();
        }

        public IDictionary<string, string> GetStandards()
        {
            return GetAllFileContents();
        }

        public void PushCommit(List<FileContents> items)
        {
            var body = _gitDynamicModelGenerator.GenerateCommitBody(_appServiceSettings.GitBranch, GetLatesCommit(), items);
            Post(_appServiceSettings.VstsGitPushUrl, _appServiceSettings.GitUsername, _appServiceSettings.GitPassword, body);
        }

        public IDictionary<string, string> GetAllFileContents()
        {
            var blobs = GetAllBlobs();

            if (blobs == null)
            {
                return new Dictionary<string, string>(0);
            }

            var standardsAsJson = new Dictionary<string, string>();

            foreach (var blob in blobs)
            {
                var str = _httpHelper.Get(blob.Url, _appServiceSettings.GitUsername, _appServiceSettings.GitPassword);
                standardsAsJson.Add(blob.Path, str);
            }

            return standardsAsJson;
        }

        // Helpers
        private IEnumerable<Entity> GetAllBlobs()
        {
            var folderTreeStr = _httpHelper.Get(
                _appServiceSettings.VstsGitGetFilesUrl,
                _appServiceSettings.GitUsername,
                _appServiceSettings.GitPassword);
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

        private void Post(string streamUrl, string username, string pwd, string body)
        {
            using (var client = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{pwd}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                using (var content = new StringContent(body, Encoding.UTF8, "application/json"))
                {
                    client.PostAsync(streamUrl, content).Wait();
                }
            }
        }

        private string GetLatesCommit()
        {
            var commitResponse = _httpHelper.Get(
                _appServiceSettings.VstsGitAllCommitsUrl,
                _appServiceSettings.GitUsername,
                _appServiceSettings.GitPassword);
            var gitTree = JsonConvert.DeserializeObject<GitTree>(commitResponse);
            if (gitTree == null)
            {
                return string.Empty;
            }

            return gitTree.Value[0]?.CommitId;
        }
    }
}