using System;
using System.Collections.Generic;
using System.Linq;
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

            var filteredFrameworkMetaData = FilterFrameworks(frameworksMetaData);

            var frameworkAimsMetaData = _csvService.ReadFromString<FrameworkAimMetaData>(frameworkAimFileContent);
            var frameworkComponentTypesMetaData = _csvService.ReadFromString<FrameworkComponentTypeMetaData>(frameworkContentTypeFileContent);
            var learningDeliveriesMetaData = _csvService.ReadFromString<LearningDeliveryMetaData>(learningDeliveryFileContent);

            foreach (var framework in filteredFrameworkMetaData)
            {
                var frameworkAims = frameworkAimsMetaData.Where(x => x.FworkCode.Equals(framework.FworkCode) &&
                                                                     (x.EffectiveTo >= DateTime.Now || x.EffectiveTo == null)).ToList();
                var qualifications =
                    (from aim in frameworkAims
                    join comp in frameworkComponentTypesMetaData on aim.FrameworkComponentType equals comp.FrameworkComponentType
                    join ld in learningDeliveriesMetaData on aim.LearnAimRef equals ld.LearnAimRef
                    select new FrameworkQualification
                    {
                        Title = ld.LearnAimRefTitle.Replace("(QCF)", string.Empty).Trim(),
                        CompetenceType = comp.FrameworkComponentType,
                        CompetenceDescription = comp.FrameworkComponentTypeDesc
                    }).ToList();

                framework.CombinedQualification = qualifications.Where(x => x.CompetenceType == 3)
                                                                .Select(x => x.Title)
                                                                .GroupBy(x => x.ToLower())
                                                                .Select(group => group.First())
                                                                .ToList();

                framework.CompetencyQualification = qualifications.Where(x => x.CompetenceType == 1)
                                                                  .Select(x => x.Title)
                                                                  .GroupBy(x => x.ToLower())
                                                                  .Select(group => group.First())
                                                                  .Except(framework.CombinedQualification)
                                                                  .ToList();

                framework.KnowledgeQualification = qualifications.Where(x => x.CompetenceType == 2)
                                                                 .Select(x => x.Title)
                                                                 .GroupBy(x => x.ToLower())
                                                                 .Select(group => group.First())
                                                                 .Except(framework.CombinedQualification)
                                                                 .ToList();
            }

            return filteredFrameworkMetaData;
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

        private static List<FrameworkMetaData> FilterFrameworks(IEnumerable<FrameworkMetaData> frameworks)
        {
            var progTypeList = new[] { 2, 3, 20, 21, 22, 23 };

            return frameworks.Where(s => s.FworkCode > 399)
                .Where(s => s.PwayCode > 0)
                .Where(s => !s.EffectiveFrom.Equals(DateTime.MinValue))
                .Where(s => !s.EffectiveTo.HasValue || s.EffectiveTo > DateTime.Now)
                .Where(s => progTypeList.Contains(s.ProgType))
                .ToList();
        }
    }
}