using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace LARSMetaDataExplorer.Zip
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

        public Dictionary<string, string> UnzipFilesFromStream(Stream stream, IEnumerable<string> filePaths)
        {
            var unzippedFiles = new Dictionary<string, string>();

            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var path in filePaths)
                {
                    var entry = zip.Entries.FirstOrDefault(m => m.FullName.EndsWith(path));

                    if (entry == null) continue;

                    using (var reader = new StreamReader(entry.Open()))
                    {
                        var content = reader.ReadToEnd();

                        unzippedFiles.Add(path, content);
                    }
                }
            }

            return unzippedFiles;
        }
    }
}