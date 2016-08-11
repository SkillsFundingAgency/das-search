using System;

namespace Sfa.Das.Sas.ApplicationServices.Settings
{
    public interface IAppServiceSettings
    {
        string GitUsername { get; }

        string GitPassword { get; }

        string VstsGitStandardsFolderPath { get; }

        string VstsGitGetFilesUrl { get; }

        string VstsGitGetFrameworkFilesUrl { get; }
    }
}
