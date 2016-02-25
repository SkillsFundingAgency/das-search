namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface ILarsDataService
    {
        string GetZipFilePath();
        string DownloadZipFile(string zipFilePath);
        List<StandardObject> GenerateJsons(string extractedPath, IEnumerable<string> excludeIds);
    }
}