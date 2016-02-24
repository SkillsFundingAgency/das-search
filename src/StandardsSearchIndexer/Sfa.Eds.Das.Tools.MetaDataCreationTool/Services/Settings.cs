namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.IO;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public class Settings : BaseSettings, ISettings
    {
        private readonly ILog4NetLogger _logger;

        public Settings(ILog4NetLogger logger)
        {
            _logger = logger;
        }

        public string WorkingFolder
        {
            get
            {
                try
                {
                    var localResource = RoleEnvironment.GetLocalResource("WorkingSpace");
                    return Path.Combine(localResource.RootPath, "CourseDirectory");
                }
                catch (Exception exception)
                {
                    _logger.Warn($"Failed to use local resource");
                    // Delete
                }
                _logger.Debug("Trying to use temp path");
                return Path.Combine(Path.GetTempPath(), "CourseDirectory");
            }
        }

        public string GovLearningUrl => GetSetting();

        public int MaxStandards => GetSetting("MaxStandards", int.MaxValue); // Delete

        public string CsvFileName => GetSetting();

        public string VstsGitFolderPath => GetSetting();

        public string VstsGitGetFilesUrl => $"{VstsGitBaseUrl}/items?scopePath={VstsGitFolderPath}&recursionLevel=Full&api-version=2.0";

        public string VstsGitAllCommitsUrl => $"{VstsGitBaseUrl}/commits?api-version=1.0&$top=1";

        public string VstsGitPushUrl => $"{VstsGitBaseUrl}/pushes?api-version=2.0-preview";

        public string GitUsername => GetSetting();
        public string GitPassword => GetSetting();
        public string GitBranch => GetSetting();

        // Private settings

        private string VstsGitBaseUrl => GetSetting();

        // Helpers
    }
}