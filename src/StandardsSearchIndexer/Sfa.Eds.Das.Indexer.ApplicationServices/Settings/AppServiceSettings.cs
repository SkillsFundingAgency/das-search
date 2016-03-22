namespace Sfa.Eds.Das.Indexer.ApplicationServices.Settings
{
    using System;
    using System.Configuration;
    using System.IO;

    public class AppServiceSettings : BaseSettings, IAppServiceSettings
    {
        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string WorkingFolder => Path.Combine(Path.GetTempPath(), "WorkingSpace");

        public string GovLearningUrl => GetSetting("GovLearningUrl");

        public string CsvFileName => GetSetting("CsvFileName");

        public string VstsGitFolderPath => GetSetting("VstsGitFolderPath");

        public string EnvironmentName => GetSetting("EnvironmentName");

        public string VstsGitGetFilesUrl => $"{VstsGitBaseUrl}/items?scopePath={VstsGitFolderPath}&recursionLevel=Full&api-version=2.0";

        public string VstsGitGetFilesUrlFormat => VstsGitBaseUrl + "/items?scopePath={0}&recursionLevel=Full&api-version=2.0";

        public string VstsGitAllCommitsUrl => $"{VstsGitBaseUrl}/commits?api-version=1.0&$top=1";

        public string VstsGitPushUrl => $"{VstsGitBaseUrl}/pushes?api-version=2.0-preview";

        public string GitUsername => GetSetting("GitUsername");

        public string GitPassword => GetSetting("GitPassword");

        public string GitBranch => GetSetting("GitBranch");

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string ImServiceBaseUrl => GetSetting("ImServiceBaseUrl");

        public string ImServiceUrl => GetSetting("ImServiceUrl");

        // Private appServiceSettings
        private string VstsGitBaseUrl => GetSetting("VstsGitBaseUrl");

        public string QueueName(Type type)
        {
            var name = type.Name.Replace("IMaintainStandardIndex", "Standard").Replace("IMaintainProviderIndex", "Provider") + ".QueueName";
            var setting = ConfigurationManager.AppSettings[name];
            if (setting != null)
            {
                return setting;
            }

            throw new ArgumentException("setting '" + name + "' not found");
        }
    }
}