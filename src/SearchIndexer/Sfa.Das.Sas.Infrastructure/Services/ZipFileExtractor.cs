using System.IO;
using System.IO.Compression;
using System.Linq;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Services
{
    public class ZipFileExtractor : IUnzipStream
    {
        public string ExtractFileFromStream(Stream stream, string filePath)
        {
            return ExtractFileFromStream(stream, filePath, false);
        }

        public string ExtractFileFromStream(Stream stream, string filePath, bool leaveStreamOpen)
        {
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveStreamOpen))
            {
                var entry = zip.Entries.FirstOrDefault(m => m.FullName.EndsWith(filePath));

                if (entry == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(entry.Open()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}