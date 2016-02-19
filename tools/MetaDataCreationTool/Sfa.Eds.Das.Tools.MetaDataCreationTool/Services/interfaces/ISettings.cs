namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces
{
    public interface ISettings
    {
        string StandardsUrl { get; }

        string JsonFilesDestination { get; }

        string LarsZipFileUrl { get; }
        string WorkingFolder { get;  }
        string CsvFile { get; }

        int MaxStandards { get; }
    }
}
