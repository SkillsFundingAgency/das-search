using System;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Settings
{
    public class AppServiceSettings : IAppServiceSettings
    {
        private readonly IProvideSettings _settings;

        public AppServiceSettings(IProvideSettings settingsProvider)
        {
            _settings = settingsProvider;
        }

        public string VstsGitStandardsFolderPath => _settings.GetSetting("VstsGitStandardsFolderPath");

        public string CsvFileNameStandards => _settings.GetSetting("CsvFileNameStandards");

        public string CsvFileNameFrameworks => _settings.GetSetting("CsvFileNameFrameworks");

        public string EnvironmentName => _settings.GetSetting("EnvironmentName");

        public string VstsGitGetFilesUrl => $"{VstsGitBaseUrl}/items?scopePath={VstsGitStandardsFolderPath}&recursionLevel=Full&api-version=2.0";

        public string VstsGitGetFrameworkFilesUrl => $"{VstsGitBaseUrl}/items?scopePath={VstsGitFrameworksFolderPath}&recursionLevel=Full&api-version=2.0";

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
        private string VstsGitFrameworksFolderPath => _settings.GetSetting("VstsGitFrameworksFolderPath");

        public string QueueName(Type type)
        {
            var name = type.Name.Replace("IMaintainApprenticeshipIndex", "Apprenticeship").Replace("IMaintainProviderIndex", "Provider") + ".QueueName";
            return _settings.GetSetting(name);
        }
    }
}