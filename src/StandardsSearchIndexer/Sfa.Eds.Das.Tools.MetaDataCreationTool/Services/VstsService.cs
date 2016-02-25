namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.Git;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public class VstsService : IVstsService
    {
        private readonly ISettings settings;

        public VstsService(ISettings settings)
        {
            this.settings = settings;
        }

        public IEnumerable<string> GetStandardObjectsIds()
        {
            var folderTreeStr = HttpHelper.DownloadString(settings.VstsGitGetFilesUrl, settings.GitUsername, settings.GitPassword);
            var tree = JsonConvert.DeserializeObject<GitTree>(folderTreeStr);
            if (tree == null)
            {
                return new List<string>();
            }

            return tree.Value.Where(x => x.IsBlob).Select(m => GetIdFromPath(m.Path));
        }

        public IEnumerable<string> GetStandards()
        {
            var folderTreeStr = HttpHelper.DownloadString(settings.VstsGitGetFilesUrl, settings.GitUsername, settings.GitPassword);
            var tree = JsonConvert.DeserializeObject<GitTree>(folderTreeStr);

            var standardsAsJson = new List<string>();
            if (tree == null)
            {
                return standardsAsJson;
            }

            foreach (var blob in tree.Value.Where(x => x.IsBlob))
            {
                var str = HttpHelper.DownloadString(blob.Url, settings.GitUsername, settings.GitPassword);
                standardsAsJson.Add(str);
            }

            return standardsAsJson;
        }

        public string GetLatesCommit()
        {
            var commitResponse = HttpHelper.DownloadString(settings.VstsGitAllCommitsUrl, settings.GitUsername, settings.GitPassword);
            var gitTree = JsonConvert.DeserializeObject<GitTree>(commitResponse);
            if (gitTree == null)
            {
                return string.Empty;
            }

            return gitTree.Value[0]?.CommitId;
        }

        public void PushCommit(string body)
        {
            Post(settings.VstsGitPushUrl, settings.GitUsername, settings.GitPassword, body);
        }

        // Helpers
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
                var result = client.PostAsync(streamUrl, content).Result;
            }
        }
    }
}