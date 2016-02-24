namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.GovLearn;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public interface IMetaDataCreation
    {
        string GetZipFilePath();
        string DownloadZipFile(string zipFilePath);
        List<StandardObject> GenerateJsons(string extractedPath, IEnumerable<string> excludeIds);
    }

    public class MetaDataCreation : IMetaDataCreation
    {
        private readonly ISettings _settings;
        private readonly ICsvService _csvService;

        public MetaDataCreation(ISettings settings, ICsvService csvService)
        {
            _settings = settings;
            _csvService = csvService;
        }

        public string GetZipFilePath()
        {
            var json = HttpHelper.DownloadString(_settings.GovLearningUrl, null, null);
            var govLearnResponse = JsonConvert.DeserializeObject<GovLearnResponse>(json);
            var govLearnResource = govLearnResponse.Resources.FirstOrDefault(m => m.Description.StartsWith("Current download"));
            return govLearnResource?.Url ?? string.Empty;
        }

        public string DownloadZipFile(string zipFilePath)
        {
            var zipFile = DownloadFile(zipFilePath, _settings.WorkingFolder);
            var extractedPath = UnPackZipFile(zipFile, _settings.CsvFileName);
            return extractedPath;
        }

        public List<StandardObject> GenerateJsons(string extractedPath, IEnumerable<string> excludeIds)
        {
            var csvFile = Path.Combine(extractedPath, _settings.CsvFileName);
            if (!File.Exists(csvFile))
            {
                Console.WriteLine($"Can't find file {csvFile}");
                return new List<StandardObject>();
            }
            var retlist = new List<StandardObject>();
            var standards = _csvService.GetAllStandardsFromCsv(csvFile);
            foreach (var standard in standards.Where(m => !excludeIds.Contains($"{m.Id}")))
            {
                var json = JsonConvert.SerializeObject(standard, Formatting.Indented);
                var standardTitle = Path.GetInvalidFileNameChars().Aggregate(standard.Title, (current, c) => current.Replace(c, '_')).Replace(" ", "");
                var gitFilePath = $"{_settings.VstsGitFolderPath}/{standard.Id}-{standardTitle}.json";
                retlist.Add(new StandardObject(gitFilePath, json));
            }
            FileHelper.DeleteRecursive(extractedPath);
            return retlist;
        }

        public string UnPackZipFile(string zipFilePath, string file)
        {
            var workingDir = Path.GetDirectoryName(zipFilePath);
            if (string.IsNullOrEmpty(workingDir)) return string.Empty;
            var extractedPath = Path.Combine(workingDir, "extracted");
            
            FileHelper.DeleteRecursive(extractedPath);
            FileHelper.EnsureDir(extractedPath);
            
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
            FileHelper.DeleteFile(zipFilePath);
            return extractedPath;
        }

        private string DownloadFile(string larsZipFileUrl, string workingfolder)
        {
            FileHelper.EnsureDir(workingfolder);
            var zipFile = Path.Combine(workingfolder, "lars.zip");
            using (var client = new WebClient())
            {
                client.DownloadFile(larsZipFileUrl, zipFile);
            }
            Console.WriteLine($"File downloaded [{zipFile}]");
            return zipFile;
        }
    }
}
