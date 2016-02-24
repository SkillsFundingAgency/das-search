namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces
{
    public interface ISettings
    {
        string WorkingFolder { get; }
        string GovLearningUrl { get; }
        int MaxStandards { get; }
        string CsvFileName { get; }

        string GitUsername { get; }

        string GitPassword { get; }

        string GitBranch { get; }
        string VstsGitFolderPath { get; }
        string VstsGitGetFilesUrl { get; }

        string VstsGitAllCommitsUrl { get; }

        string VstsGitPushUrl { get; }
    }
}
