using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Services
{
    using Core.Logging;
    using Core.Logging.Metrics;
    using Core.Logging.Models;

    using Newtonsoft.Json;

    using Sfa.Das.Sas.Indexer.Infrastructure.Services.Models;

    public class VstsClient : IVstsClient
    {
        private readonly IHttpGet _httpHelper;

        private readonly IHttpPost _httpPost;

        private readonly ILog _logger;

        private readonly IAppServiceSettings _appServiceSettings;

        public VstsClient(
            IAppServiceSettings appServiceSettings,
            IHttpGet httpHelper,
            IHttpPost httpPost,
            ILog logger)
        {
            _appServiceSettings = appServiceSettings;
            _httpHelper = httpHelper;
            _httpPost = httpPost;
            _logger = logger;
        }

        public string GetFileContent(string path)
        {
            var url = string.Format(_appServiceSettings.VstsGitGetFilesUrlFormat, path);
            return Get(url);
        }

        public string Get(string url)
        {
            var timing = ExecutionTimer.GetTiming(() => _httpHelper.Get(url, _appServiceSettings.GitUsername, _appServiceSettings.GitPassword));

            var logEntry = new DependencyLogEntry
            {
                Identifier = "VstsContent",
                ResponseTime = timing.ElaspedMilliseconds,
                Url = url
            };

            _logger.Debug("VSTS content", logEntry);

            return timing.Result;
        }

        public void Post(string url, string username, string pwd, string body)
        {
            _httpPost.Post(url, body, username, pwd);
        }

        public string GetLatesCommit()
        {
            var commitResponse = Get(_appServiceSettings.VstsGitAllCommitsUrl);
            var gitTree = JsonConvert.DeserializeObject<GitTree>(commitResponse);
            if (gitTree == null)
            {
                return string.Empty;
            }

            return gitTree.Value[0]?.CommitId;
        }
    }
}