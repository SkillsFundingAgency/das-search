namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.GovLearn;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public sealed class LarsDataService : ILarsDataService
    {
        private readonly IReadStandardsFromCsv _csvService;

        private readonly IUnzipStream _fileExtractor;

        private readonly IHttpGet _httpGet;

        private readonly IHttpGetFile _httpGetFile;

        private readonly ILog _logger;

        private readonly IAppServiceSettings _appServiceSettings;

        public LarsDataService(
            IAppServiceSettings appServiceSettings,
            IReadStandardsFromCsv csvService,
            IHttpGetFile httpGetFile,
            IUnzipStream fileExtractor,
            ILog logger,
            IHttpGet httpGet)
        {
            _appServiceSettings = appServiceSettings;
            _csvService = csvService;
            _httpGetFile = httpGetFile;
            _fileExtractor = fileExtractor;
            _logger = logger;
            _httpGet = httpGet;
        }

        public IEnumerable<Standard> GetListOfCurrentStandards()
        {
            var zipFilePath = GetZipFilePath();
            _logger.Debug($"Zip file path: {zipFilePath}");

            var zipStream = _httpGetFile.GetFile(zipFilePath);
            _logger.Debug($"Zip file downloaded");

            string fileContent = _fileExtractor.ExtractFileFromStream(zipStream, _appServiceSettings.CsvFileName);
            _logger.Debug($"Extracted contrent. Length: {fileContent.Length}");

            var standards = _csvService.ReadStandardsFromStream(fileContent);
            _logger.Debug($"Read: {standards.Count} standards from file.");

            return standards;
        }

        private string GetZipFilePath()
        {
            var json = _httpGet.Get(_appServiceSettings.GovLearningUrl, null, null);
            var govLearnResponse = JsonConvert.DeserializeObject<GovLearnResponse>(json);

            if (govLearnResponse == null)
            {
                return string.Empty;
            }

            var govLearnResource = govLearnResponse.Resources.FirstOrDefault(m => m.Description.StartsWith("Current download"));

            return govLearnResource?.Url ?? string.Empty;
        }
    }
}