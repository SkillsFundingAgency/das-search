using System.Collections.Generic;
using System.IO;

namespace LARSMetaDataExplorer.Zip
{
    public interface IUnzipStream
    {
        string ExtractFileFromStream(Stream stream, string filePath);

        string ExtractFileFromStream(Stream stream, string filePath, bool leaveStreamOpen);

        Dictionary<string, string> UnzipFilesFromStream(Stream stream, IEnumerable<string> filePaths);
    }
}