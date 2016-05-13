using System.IO;
using System.IO.Compression;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Services
{
    using System.Linq;

    public class ZipFileExtractor : IUnzipStream
    {
        public string ExtractFileFromStream(Stream stream, string fileToExtract)
        {
            using (var zip = new ZipArchive(stream))
            {
                foreach (var entry in zip.Entries.Where(m => m.FullName.EndsWith($"CSV/{fileToExtract}")))
                {
                    using (var reader = new StreamReader(entry.Open()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            return null;
        }
    }
}