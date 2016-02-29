namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;

    public class VstsClient : IVstsClient
    {
        private readonly IHttpHelper _httpHelper;

        private readonly ISettings _settings;

        public VstsClient(ISettings settings, IHttpHelper httpHelper)
        {
            _settings = settings;
            _httpHelper = httpHelper;
        }

        public string GetFileContent(string path)
        {
            var url = string.Format(_settings.VstsGitGetFilesUrlFormat, path);
            return _httpHelper.DownloadString(url, _settings.GitUsername, _settings.GitPassword);
        }
    }
}