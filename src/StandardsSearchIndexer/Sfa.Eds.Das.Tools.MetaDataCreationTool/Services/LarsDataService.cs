namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Indexer.Core.Services;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public sealed class LarsDataService : ILarsDataService
    {
        private readonly IReadStandardsFromCsv _csvService;

        private readonly IUnzipStream _fileExtractor;

        private readonly IAngleSharpService _angleSharpService;

        private readonly IHttpGet _httpGet;

        private readonly IHttpGetFile _httpGetFile;

        private readonly ILog _logger;

        private readonly IAppServiceSettings _appServiceSettings;

        public LarsDataService(
            IAppServiceSettings appServiceSettings,
            IReadStandardsFromCsv csvService,
            IHttpGetFile httpGetFile,
            IUnzipStream fileExtractor,
            IAngleSharpService angleSharpService,
            ILog logger,
            IHttpGet httpGet)
        {
            _appServiceSettings = appServiceSettings;
            _csvService = csvService;
            _httpGetFile = httpGetFile;
            _fileExtractor = fileExtractor;
            _angleSharpService = angleSharpService;
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

        public List<FrameworkMetaData> GetListOfCurrentFrameworks()
        {
            var zipFilePath = GetZipFilePath();

            var zipStream = _httpGetFile.GetFile(zipFilePath);

            string fileContent = _fileExtractor.ExtractFileFromStream(zipStream, "Framework.csv");

            var standards = _csvService.ReadFrameworksFromStream(fileContent);

            return standards;
        }

        private string GetZipFilePath()
        {
            var url = $"{_appServiceSettings.ImServiceBaseUrl}/{_appServiceSettings.ImServiceUrl}";

            var link = _angleSharpService.GetLinks(url, "li a", "LARS CSV");
            var linkEndpoint = link.FirstOrDefault();
            var fullLink = linkEndpoint != null ? $"{_appServiceSettings.ImServiceBaseUrl}/{linkEndpoint}" : string.Empty;

            if (string.IsNullOrEmpty(fullLink))
            {
                _logger.Error($"Can not find LARS zip file. Url: {url}");
            }

            return fullLink;
        }
    }
}