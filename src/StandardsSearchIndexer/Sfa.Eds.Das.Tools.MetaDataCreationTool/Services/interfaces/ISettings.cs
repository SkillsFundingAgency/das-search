namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    public interface ISettings
    {
        string WorkingFolder { get; }

        string GovLearningUrl { get; }

        string CsvFileName { get; }

        string GitUsername { get; }

        string GitPassword { get; }

        string GitBranch { get; }

        string VstsGitFolderPath { get; }

        string VstsGitGetFilesUrl { get; }

        string VstsGitGetFilesUrlFormat { get; }

        string VstsGitAllCommitsUrl { get; }

        string VstsGitPushUrl { get; }

        string EnvironmentName { get; }
    }
}
