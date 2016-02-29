namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.IO;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class Settings : BaseSettings, ISettings
    {
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

        // Private settings
        private string VstsGitBaseUrl => GetSetting("VstsGitBaseUrl");
    }
}