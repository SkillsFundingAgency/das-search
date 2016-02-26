using System;
using System.IO;
using System.IO.Compression;
using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    public class ZipFileExtractor : IUnzipFiles
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
    }
}
