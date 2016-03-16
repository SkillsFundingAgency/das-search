namespace Sfa.Infrastructure.Services
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    public class VstsClient : IVstsClient
    {
        private readonly IHttpGet _httpHelper;

        private readonly IAppServiceSettings _appServiceSettings;

        public VstsClient(IAppServiceSettings appServiceSettings, IHttpGet httpHelper)
        {
            _appServiceSettings = appServiceSettings;
            _httpHelper = httpHelper;
        }

        public string GetFileContent(string path)
        {
            var url = string.Format(_appServiceSettings.VstsGitGetFilesUrlFormat, path);
            return _httpHelper.Get(url, _appServiceSettings.GitUsername, _appServiceSettings.GitPassword);
        }
    }
}