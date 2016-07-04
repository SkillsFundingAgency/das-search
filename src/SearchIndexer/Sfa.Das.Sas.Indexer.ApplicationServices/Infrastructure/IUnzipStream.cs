using System.Collections.Generic;
using System.IO;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure
{
    public interface IUnzipStream
    {
        string ExtractFileFromStream(Stream stream, string filePath);
        Dictionary<string, string> ExtractFilesFromStream(Stream stream, IEnumerable<string> filePaths);
    }
}