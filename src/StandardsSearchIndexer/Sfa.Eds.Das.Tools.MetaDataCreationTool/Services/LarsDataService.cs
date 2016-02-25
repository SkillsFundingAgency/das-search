namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.GovLearn;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public sealed class LarsDataService : ILarsDataService
    {
        private readonly ISettings _settings;
        private readonly IReadStandardsFromCsv _csvService;
        private readonly IHttpHelper _httpHelper;

        public LarsDataService(ISettings settings, IReadStandardsFromCsv csvService, IHttpHelper httpHelper)
        {
            _settings = settings;
            _csvService = csvService;
            _httpHelper = httpHelper;
        }

        public string GetZipFilePath()
        {
            var json = _httpHelper.DownloadString(_settings.GovLearningUrl, null, null);
            var govLearnResponse = JsonConvert.DeserializeObject<GovLearnResponse>(json);
            if (govLearnResponse == null)
            {
                return string.Empty;
            }

            var govLearnResource = govLearnResponse.Resources.FirstOrDefault(m => m.Description.StartsWith("Current download"));
            return govLearnResource?.Url ?? string.Empty;
        }

        public string DownloadZipFile(string zipFilePath)
        {
            var zipFile = _httpHelper.DownloadFile(zipFilePath, _settings.WorkingFolder);
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
            var standards = _csvService.ReadStandardsFromFile(csvFile);
            foreach (var standard in standards.Where(m => !excludeIds.Contains($"{m.Id}")))
            {
                var json = JsonConvert.SerializeObject(standard, Formatting.Indented);
                var standardTitle = Path.GetInvalidFileNameChars().Aggregate(standard.Title, (current, c) => current.Replace(c, '_')).Replace(" ", string.Empty);
                var gitFilePath = $"{_settings.VstsGitFolderPath}/{standard.Id}-{standardTitle}.json";
                retlist.Add(new StandardObject(gitFilePath, json));
            }

            FileHelper.DeleteRecursive(extractedPath);
            return retlist;
        }

        public string UnPackZipFile(string zipFilePath, string file)
        {
            if (string.IsNullOrEmpty(zipFilePath))
            {
                return string.Empty;
            }

            var workingDir = Path.GetDirectoryName(zipFilePath);
            if (string.IsNullOrEmpty(workingDir))
            {
                return string.Empty;
            }

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

            FileHelper.DeleteFile(zipFilePath);
            return extractedPath;
        }
    }
}
