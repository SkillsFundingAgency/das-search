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
        private readonly IHttpGetFile _httpGetFile;
        private readonly ILog _logger;
        private readonly IAppServiceSettings _appServiceSettings;

        public LarsDataService(
            IAppServiceSettings appServiceSettings,
            IReadMetaDataFromCsv csvService,
            IHttpGetFile httpGetFile,
            IUnzipStream fileExtractor,
            IAngleSharpService angleSharpService,
            ILog logger)
        {
            _appServiceSettings = appServiceSettings;
            _csvService = csvService;
            _httpGetFile = httpGetFile;
            _fileExtractor = fileExtractor;
            _angleSharpService = angleSharpService;
            _logger = logger;
        }

        public IEnumerable<LarsStandard> GetListOfCurrentStandards()
        {
            var zipFilePath = GetZipFilePath();
            _logger.Debug($"Zip file path: {zipFilePath}");

            var zipStream = _httpGetFile.GetFile(zipFilePath);
            _logger.Debug("Zip file downloaded");

            var fileContent = _fileExtractor.ExtractFileFromStream(zipStream, _appServiceSettings.CsvFileNameStandards);
            _logger.Debug($"Extracted contrent. Length: {fileContent.Length}");

            var standards = _csvService.ReadFromString<LarsStandard>(fileContent);
            _logger.Debug($"Read: {standards.Count} standards from file.");

            return standards;
        }

        public ICollection<FrameworkMetaData> GetListOfCurrentFrameworks()
        {
            var zipFilePath = GetZipFilePath();
            _logger.Debug($"Zip file path: {zipFilePath}");

            var csvData = GetLarsCsvData(zipFilePath);

            var larsMetaData = GetLarsMetaData(csvData);

            AddQualificationsToFrameworks(larsMetaData);

            return larsMetaData.Frameworks;
        }

        private static void AddQualificationsToFrameworks(LarsMetaData metaData)
        {
            foreach (var framework in metaData.Frameworks)
            {
                var frameworkAims = metaData.FrameworkAims.Where(x => x.FworkCode.Equals(framework.FworkCode) &&
                                                                      (x.EffectiveTo >= DateTime.Now || x.EffectiveTo == null)).ToList();

                var qualifications =
                    from aim in frameworkAims
                    join comp in metaData.FrameworkContentTypes on aim.FrameworkComponentType equals comp.FrameworkComponentType
                    join ld in metaData.LearningDeliveries on aim.LearnAimRef equals ld.LearnAimRef
                    select new FrameworkQualification
                    {
                        Title = ld.LearnAimRefTitle.Replace("(QCF)", string.Empty).Trim(),
                        CompetenceType = comp.FrameworkComponentType,
                        CompetenceDescription = comp.FrameworkComponentTypeDesc
                    };

                var categorisedQualifications = GetCategorisedQualifications(qualifications.ToList());

                framework.CompetencyQualification = categorisedQualifications.Competency;
                framework.KnowledgeQualification = categorisedQualifications.Knowledge;
                framework.CombinedQualification = categorisedQualifications.Combined;
            }
        }

        private static CategorisedQualifications GetCategorisedQualifications(ICollection<FrameworkQualification> qualifications)
        {
            var qualificationSet = default(CategorisedQualifications);

            qualificationSet.Combined = qualifications.Where(x => x.CompetenceType == 3)
                .Select(x => x.Title)
                .GroupBy(x => x.ToUpperInvariant())
                .Select(group => @group.First())
                .ToList();

            qualificationSet.Competency = qualifications.Where(x => x.CompetenceType == 1)
                .Select(x => x.Title)
                .GroupBy(x => x.ToUpperInvariant())
                .Select(group => @group.First())
                .Except(qualificationSet.Combined)
                .ToList();

            qualificationSet.Knowledge = qualifications.Where(x => x.CompetenceType == 2)
                .Select(x => x.Title)
                .GroupBy(x => x.ToUpperInvariant())
                .Select(group => @group.First())
                .Except(qualificationSet.Combined)
                .ToList();

            return qualificationSet;
        }

        private static ICollection<FrameworkMetaData> FilterFrameworks(IEnumerable<FrameworkMetaData> frameworks)
        {
            var progTypeList = new[] { 2, 3, 20, 21, 22, 23 };

            return frameworks.Where(s => s.FworkCode > 399)
                .Where(s => s.PwayCode > 0)
                .Where(s => !s.EffectiveFrom.Equals(DateTime.MinValue))
                .Where(s => !s.EffectiveTo.HasValue || s.EffectiveTo > DateTime.Now)
                .Where(s => progTypeList.Contains(s.ProgType))
                .ToList();
        }

        private LarsMetaData GetLarsMetaData(LarsCsvData larsCsvData)
        {
            var metaData = default(LarsMetaData);

            var frameworksMetaData = _csvService.ReadFromString<FrameworkMetaData>(larsCsvData.Framework);
            _logger.Debug($"Read {frameworksMetaData.Count} frameworks from file.");

            metaData.Frameworks = FilterFrameworks(frameworksMetaData);
            _logger.Debug($"There are {metaData.Frameworks.Count} frameworks after filtering.");

            metaData.FrameworkAims = _csvService.ReadFromString<FrameworkAimMetaData>(larsCsvData.FrameworkAim);
            metaData.FrameworkContentTypes = _csvService.ReadFromString<FrameworkComponentTypeMetaData>(larsCsvData.FrameworkContentType);
            metaData.LearningDeliveries = _csvService.ReadFromString<LearningDeliveryMetaData>(larsCsvData.LearningDelivery);

            return metaData;
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

        private LarsCsvData GetLarsCsvData(string zipFilePath)
        {
            var csvData = default(LarsCsvData);

            using (var zipStream = _httpGetFile.GetFile(zipFilePath))
            {
                _logger.Debug("Zip file downloaded");

                csvData.Framework = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameFrameworks, true);

                csvData.FrameworkAim = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameFrameworksAim, true);

                csvData.FrameworkContentType = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameFrameworkComponentType, true);

                csvData.LearningDelivery = _fileExtractor.ExtractFileFromStream(
                    zipStream, _appServiceSettings.CsvFileNameLearningDelivery, true);
            }

            return csvData;
        }

        private struct LarsCsvData
        {
            public string Framework { get; set; }
            public string FrameworkAim { get; set; }
            public string FrameworkContentType { get; set; }
            public string LearningDelivery { get; set; }
        }

        private struct LarsMetaData
        {
            public ICollection<FrameworkMetaData> Frameworks { get; set; }
            public ICollection<FrameworkAimMetaData> FrameworkAims { get; set; }
            public ICollection<FrameworkComponentTypeMetaData> FrameworkContentTypes { get; set; }
            public ICollection<LearningDeliveryMetaData> LearningDeliveries { get; set; }
        }

        private struct CategorisedQualifications
        {
            public ICollection<string> Competency { get; set; }
            public ICollection<string> Knowledge { get; set; }
            public ICollection<string> Combined { get; set; }
        }
    }
}