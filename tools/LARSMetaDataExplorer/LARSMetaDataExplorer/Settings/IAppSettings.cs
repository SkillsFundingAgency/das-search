namespace LARSMetaDataExplorer.Settings
{
    public interface IAppSettings
    {
        string ImServiceBaseUrl { get; }
        string ImServiceUrl { get; }
        string CsvFileNameStandards { get; }
    }
}