using System;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Settings
{
    public interface IAppServiceSettings
    {
        string CsvFileNameStandards { get; }

        string CsvFileNameFrameworks { get; }

        string CsvFileNameFrameworksAim { get; }

        string CsvFileNameFrameworkComponentType { get; }

        string CsvFileNameLearningDelivery { get; }

        string GitUsername { get; }

        string GitPassword { get; }

        string GitBranch { get; }

        string VstsGitStandardsFolderPath { get; }

        string VstsGitGetFilesUrl { get; }

        string VstsGitGetFrameworkFilesUrl { get; }

        string VstsGitGetFilesUrlFormat { get; }

        string VstsGitAllCommitsUrl { get; }

        string VstsGitPushUrl { get; }

        string EnvironmentName { get; }

        string ConnectionString { get; }

        string ImServiceBaseUrl { get; }

        string ImServiceUrl { get; }

        string GovWebsiteUrl { get; }

        string QueueName(Type type);

        string MetadataApiUri { get; }
    }
}
