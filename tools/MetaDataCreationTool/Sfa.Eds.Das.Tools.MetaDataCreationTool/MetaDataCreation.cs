namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public interface IMetaDataCreation
    {
        void Run(CmdArgs cmdArgs);
    }

    public class MetaDataCreation : IMetaDataCreation
    {
        private readonly ISettings settings;
        private readonly ICsvService csvService;
        private readonly IFileService fileService;

        public MetaDataCreation(ISettings settings, ICsvService csvService, IFileService fileService)
        {
            this.settings = settings;
            this.csvService = csvService;
            this.fileService = fileService;
        }

        public void Run(CmdArgs cmdArgs)
        {
            string csvFile;
            if (cmdArgs.UseLocalCsvFile)
            {
                csvFile = this.settings.CsvFile;
            }
            else
            {
                var filename = "lars.zip";
                var storageFolder = this.settings.WorkingFolder;
                var zipFile = $"{storageFolder}\\{filename}";
                var extractedPath = $"{storageFolder}\\extracted";
                var csvFileName = "Standard.csv";

                // ---------------

                DownloadFile(this.settings.LarsZipFileUrl, storageFolder, filename);
                UnPackZipFile(zipFile, extractedPath, csvFileName);
                csvFile = Path.Combine(extractedPath, "standard.csv");

                if (!File.Exists(csvFile))
                {
                    Console.WriteLine($"Can't find file {csvFile}");
                    return;
                }
            }

            var standards = csvService.GetAllStandardsFromCsv(csvFile);

            foreach (var standard in standards)
            {
                fileService.CreateJsonFile(standard, settings.JsonFilesDestination, false);
            }
            Console.WriteLine($"Generation Done, {standards.Count} created in directory {settings.JsonFilesDestination}");
        }

        public void UnPackZipFile(string zipFilePath, string extractedPath)
        {
            EnsureDir(extractedPath);
            ZipFile.ExtractToDirectory(zipFilePath, extractedPath);
        }

        public void UnPackZipFile(string zipFilePath, string extractedPath, string file)
        {
            DeleteRecursive(extractedPath);
            EnsureDir(extractedPath);
            using (var zip = ZipFile.OpenRead(zipFilePath))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.FullName.EndsWith(file, StringComparison.OrdinalIgnoreCase))
                    {
                        entry.ExtractToFile(Path.Combine(extractedPath, file));
                    }
                }
            }
            Console.WriteLine($"Files extracted. [{extractedPath}]");
            DeleteFile(zipFilePath);
        }

        private void DeleteFile(string zipFilePath)
        {
            if(File.Exists(zipFilePath))
                File.Delete(zipFilePath);
        }

        public void DownloadFile(string larsZipFileUrl, string storagefolder, string zipFile)
        {
            EnsureDir(storagefolder);
            using (var client = new WebClient())
            {
                client.DownloadFile(larsZipFileUrl, Path.Combine(storagefolder,zipFile));
            }
            Console.WriteLine($"File downloaded [{Path.Combine(storagefolder, zipFile)}]");
        }

        private void EnsureDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void DeleteRecursive(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
        }
    }
}
