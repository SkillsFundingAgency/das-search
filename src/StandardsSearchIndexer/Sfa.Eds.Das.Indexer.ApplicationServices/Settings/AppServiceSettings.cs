namespace Sfa.Eds.Das.Indexer.ApplicationServices.Settings
{
    using System;
    using Core.Services;
    public class AppServiceSettings : IAppServiceSettings
    {
        private readonly IProvideSettings _settings;

        public AppServiceSettings(IProvideSettings settingsProvider)
        {
            _settings = settingsProvider;
        }

        public string CsvFileName => _settings.GetSetting("CsvFileName");

        public string VstsGitFolderPath => _settings.GetSetting("VstsGitFolderPath");

        public string EnvironmentName => _settings.GetSetting("EnvironmentName");

        public string VstsGitGetFilesUrl => $"{VstsGitBaseUrl}/items?scopePath={VstsGitFolderPath}&recursionLevel=Full&api-version=2.0";

        public string VstsGitGetFilesUrlFormat => VstsGitBaseUrl + "/items?scopePath={0}&recursionLevel=Full&api-version=2.0";

        public string VstsGitAllCommitsUrl => $"{VstsGitBaseUrl}/commits?api-version=1.0&$top=1";

        public string VstsGitPushUrl => $"{VstsGitBaseUrl}/pushes?api-version=2.0-preview";

        public string GitUsername => _settings.GetSetting("GitUsername");

        public string GitPassword => _settings.GetSetting("GitPassword");

        public string GitBranch => _settings.GetSetting("GitBranch");

        public string ConnectionString => _settings.GetSetting("StorageConnectionString");

        public string ImServiceBaseUrl => _settings.GetSetting("ImServiceBaseUrl");

        public string ImServiceUrl => _settings.GetSetting("ImServiceUrl");

        // Private appServiceSettings
        private string VstsGitBaseUrl => _settings.GetSetting("VstsGitBaseUrl");

        public string QueueName(Type type)
        {
            var name = type.Name.Replace("IMaintainApprenticeshipIndex", "Apprenticeship").Replace("IMaintainProviderIndex", "Provider") + ".QueueName";
            return _settings.GetSetting(name);
        }
    }
}