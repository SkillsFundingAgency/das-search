namespace Sfa.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;

    public interface IInfrastructureSettings
    {
        string WorkerRolePauseTime { get; }
        string ApprenticeshipIndexAlias { get; }
        string StorageAccountName { get; }
        string StorageAccountKey { get; }
        string ConnectionString { get; }
        string PauseTime { get; }
        string StandardJsonContainer { get; }
        string StandardPdfContainer { get; }
        string StandardContentType { get; }
        string CourseDirectoryUri { get; }

        IEnumerable<Uri> ElasticServerUrls { get; }
    }
}