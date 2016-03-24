namespace Sfa.Infrastructure.Settings
{
    public interface IInfrastructureSettings
    {
        string SearchHost { get; }
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
    }
}