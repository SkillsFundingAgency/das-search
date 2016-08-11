using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Settings
{
    public class AppServiceSettings : IAppServiceSettings
    {
        private readonly IProvideSettings _settings;

        public AppServiceSettings(IProvideSettings settingsProvider)
        {
            _settings = settingsProvider;
        }

        public string VstsGitStandardsFolderPath => _settings.GetSetting("VstsGitStandardsFolderPath");

        public string VstsGitGetFilesUrl => $"{VstsGitBaseUrl}/items?scopePath={VstsGitStandardsFolderPath}&recursionLevel=Full&api-version=2.0";

        public string VstsGitGetFrameworkFilesUrl => $"{VstsGitBaseUrl}/items?scopePath={VstsGitFrameworksFolderPath}&recursionLevel=Full&api-version=2.0";
        
        public string GitUsername => _settings.GetSetting("GitUsername");

        public string GitPassword => _settings.GetSetting("GitPassword");

        // Private appServiceSettings
        private string VstsGitBaseUrl => _settings.GetSetting("VstsGitBaseUrl");
        private string VstsGitFrameworksFolderPath => _settings.GetSetting("VstsGitFrameworksFolderPath");
    }
}
