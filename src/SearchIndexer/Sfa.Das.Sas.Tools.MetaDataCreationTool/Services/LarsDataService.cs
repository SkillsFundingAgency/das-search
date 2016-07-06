using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    public sealed class LarsDataService : ILarsDataService
    {
        private readonly IReadMetaDataFromCsv _csvService;
        private readonly IUnzipStream _fileExtractor;
        private readonly IAngleSharpService _angleSharpService;
        private readonly IHttpGet _httpGet;
        private readonly IHttpGetFile _httpGetFile;
        private readonly ILog _logger;
        private readonly IAppServiceSettings _appServiceSettings;

        public LarsDataService(
            IAppServiceSettings appServiceSettings,
            IReadMetaDataFromCsv csvService,
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

        public IEnumerable<LarsStandard> GetListOfCurrentStandards()
        {
            var fileContent = ReadStandardCsvFile();

            var standards = _csvService.ReadFromString<LarsStandard>(fileContent);
            _logger.Debug($"Read: {standards.Count} standards from file.");

            return standards;
        }

        public ICollection<FrameworkMetaData> GetListOfCurrentFrameworks()
        {
            var zipFilePath = GetZipFilePath();

            string frameworksFileContent;
            string frameworkAimFileContent;
            string frameworkContentTypeFileContent;
            string learningDeliveryFileContent;

            using (var zipStream = _httpGetFile.GetFile(zipFilePath))
            {
                frameworksFileContent = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameFrameworks, true);

                frameworkAimFileContent = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameFrameworksAim, true);

                frameworkContentTypeFileContent = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameFrameworkComponentType, true);

                learningDeliveryFileContent = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameLearningDelivery, true);
            }

            var frameworksMetaData = _csvService.ReadFromString<FrameworkMetaData>(frameworksFileContent);
            var frameworkAimsMetaData = _csvService.ReadFromString<FrameworkAimMetaData>(frameworkAimFileContent);
            var frameworkComponentTypesMetaData = _csvService.ReadFromString<FrameworkComponentTypeMetaData>(frameworkContentTypeFileContent);
            var learningDeliveriesMetaData = _csvService.ReadFromString<LearningDeliveryMetaData>(learningDeliveryFileContent);

            foreach (var framework in frameworksMetaData)
            {
                var frameworkAims = frameworkAimsMetaData.Where(x => x.FworkCode.Equals(framework.FworkCode) &&
                                                                     (x.EffectiveTo >= DateTime.Now || x.EffectiveTo == null)).ToList();
                var qualifications =
                    from aim in frameworkAims
                    join comp in frameworkComponentTypesMetaData on aim.FrameworkComponentType equals comp.FrameworkComponentType
                    join ld in learningDeliveriesMetaData on aim.LearnAimRef equals ld.LearnAimRef
                    select new
                    {
                        Title = ld.LearnAimRefTitle,
                        CompetenceDescription = comp.FrameworkComponentTypeDesc
                    };

                if (qualifications.Any())
                {
                    var lines = qualifications.Select(x => x.Title + " " + x.CompetenceDescription);

                    framework.Qualifications = lines.Aggregate((x1, x2) => x1 + Environment.NewLine + x2);
                }
                else
                {
                    framework.Qualifications = "None specified";
                }
            }

            return frameworksMetaData;
        }

        private string ReadStandardCsvFile()
        {
            var zipFilePath = GetZipFilePath();
            _logger.Debug($"Zip file path: {zipFilePath}");

            var zipStream = _httpGetFile.GetFile(zipFilePath);
            _logger.Debug("Zip file downloaded");

            string fileContent = _fileExtractor.ExtractFileFromStream(zipStream, _appServiceSettings.CsvFileNameStandards);
            _logger.Debug($"Extracted contrent. Length: {fileContent.Length}");

            return fileContent;
        }

        private string GetZipFilePath()
        {
            var url = $"{_appServiceSettings.ImServiceBaseUrl}/{_appServiceSettings.ImServiceUrl}";

            var link = _angleSharpService.GetLinks(url, "li a", "LARS CSV");
            var linkEndpoint = link.FirstOrDefault();
            var fullLink = linkEndpoint != null ? $"{_appServiceSettings.ImServiceBaseUrl}/{linkEndpoint}" : string.Empty;

            if (string.IsNullOrEmpty(fullLink))
            {
                throw new Exception($"Can not find LARS zip file. Url: {url}");
            }

            return fullLink;
        }
    }
}