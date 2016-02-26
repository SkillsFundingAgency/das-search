namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.Git;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class VstsService : IVstsService
    {
        private readonly ISettings _settings;
        private readonly IGitDynamicModelGenerator _gitDynamicModelGenerator;
        private readonly IHttpHelper _httpHelper;

        public VstsService(ISettings settings, IGitDynamicModelGenerator gitDynamicModelGenerator, IHttpHelper httpHelper)
        {
            _settings = settings;
            _gitDynamicModelGenerator = gitDynamicModelGenerator;
            _httpHelper = httpHelper;
        }

        public IEnumerable<string> GetExistingStandardIds()
        {
            var blobs = GetAllBlobs();

            return blobs?.Select(m => GetIdFromPath(m.Path)) ?? new List<string>();
        }

        public IEnumerable<string> GetStandards()
        {
            var blobs = GetAllBlobs();

            if (blobs == null)
            {
                return new List<string>();
            }

            var standardsAsJson = new List<string>();
            foreach (var blob in blobs)
            {
                var str = _httpHelper.DownloadString(blob.Url, _settings.GitUsername, _settings.GitPassword);
                standardsAsJson.Add(str);
            }

            return standardsAsJson;
        }

        public void PushCommit(List<StandardObject> items)
        {
            var body = _gitDynamicModelGenerator.GenerateCommitBody(_settings.GitBranch, GetLatesCommit(), items);
            Post(_settings.VstsGitPushUrl, _settings.GitUsername, _settings.GitPassword, body);
        }

        // Helpers
        private IEnumerable<Entity> GetAllBlobs()
        {
            var folderTreeStr = _httpHelper.DownloadString(_settings.VstsGitGetFilesUrl, _settings.GitUsername, _settings.GitPassword);
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
            using (HttpClient client = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{pwd}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
                client.PostAsync(streamUrl, content).Wait();
            }
        }

        private string GetLatesCommit()
        {
            var commitResponse = _httpHelper.DownloadString(_settings.VstsGitAllCommitsUrl, _settings.GitUsername, _settings.GitPassword);
            var gitTree = JsonConvert.DeserializeObject<GitTree>(commitResponse);
            if (gitTree == null)
            {
                return string.Empty;
            }

            return gitTree.Value[0]?.CommitId;
        }
    }
}