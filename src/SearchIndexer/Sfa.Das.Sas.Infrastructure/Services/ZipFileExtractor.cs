using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Services
{
    using System.Linq;

    public class ZipFileExtractor : IUnzipStream
    {
        public string ExtractFileFromStream(Stream stream, string filePath)
        {
            using (var zip = new ZipArchive(stream))
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

        public Dictionary<string, string> ExtractFilesFromStream(Stream stream, IEnumerable<string> filePaths)
        {
            var extractedFiles = new Dictionary<string, string>();

            using (var zip = new ZipArchive(stream))
            {
                foreach (var filePath in filePaths)
                {
                    var entry = zip.Entries.FirstOrDefault(m => m.FullName.EndsWith(filePath));

                    if (entry == null)
                    {
                        continue;
                    }

                    using (var reader = new StreamReader(entry.Open()))
                    {
                        extractedFiles.Add(filePath, reader.ReadToEnd());
                    }
                }
            }

            return extractedFiles;
        }
    }
}