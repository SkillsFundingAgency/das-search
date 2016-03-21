namespace Sfa.Infrastructure.Services
{
    using System;
    using System.IO;
    using System.IO.Compression;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;

    public class ZipFileExtractor : IUnzipFiles, IUnzipStream
    {
        public string ExtractFileFromZip(string pathToZipFile, string fileToExtract)
        {
            if (string.IsNullOrEmpty(pathToZipFile))
            {
                return string.Empty;
            }

            var workingDir = Path.GetDirectoryName(pathToZipFile);

            if (string.IsNullOrEmpty(workingDir))
            {
                return string.Empty;
            }

            var extractedPath = Path.Combine(workingDir, "extracted");

            FileHelper.DeleteRecursive(extractedPath);
            FileHelper.EnsureDir(extractedPath);

            using (var zip = ZipFile.OpenRead(pathToZipFile))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.FullName.EndsWith(fileToExtract, StringComparison.OrdinalIgnoreCase))
                    {
                        entry.ExtractToFile(Path.Combine(extractedPath, fileToExtract));
                    }
                }
            }

            FileHelper.DeleteFile(pathToZipFile);

            return extractedPath;
        }

        public string ExtractFileFromStream(Stream stream, string fileToExtract)
        {
            using (var zip = new ZipArchive(stream))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.FullName.EndsWith(fileToExtract, StringComparison.OrdinalIgnoreCase))
                    {
                        using (var reader = new StreamReader(entry.Open()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

            return null;
        }
    }
}