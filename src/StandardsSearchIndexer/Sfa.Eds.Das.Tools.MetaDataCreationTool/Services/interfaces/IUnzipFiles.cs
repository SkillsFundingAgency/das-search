namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    public interface IUnzipFiles
    {
        string ExtractFileFromZip(string pathToZipFile, string fileToExtract);
    }
}
