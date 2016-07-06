using System.Collections.Generic;
using System.IO;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure
{
    public interface IUnzipStream
    {
        string ExtractFileFromStream(Stream stream, string filePath);

        string ExtractFileFromStream(Stream stream, string filePath, bool leaveStreamOpen);
    }
}