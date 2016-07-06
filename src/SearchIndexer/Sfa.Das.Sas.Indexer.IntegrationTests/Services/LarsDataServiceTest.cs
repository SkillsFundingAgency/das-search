using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Infrastructure.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;
using CsvService = Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.CsvService;

namespace Sfa.Das.Sas.Indexer.IntegrationTests.Services
{
    [TestFixture]
    public class LarsDataServiceTest
    {
        private LarsDataService _sut;
        private IReadMetaDataFromCsv _readMetaDataFromCsv;
        private IHttpGetFile _httpGetFile;
        private IUnzipStream _fileExtractor;
        private IAngleSharpService _angleSharpService;
        private IHttpGet _httpGet;

        private Mock<IAppServiceSettings> _appServiceSettings;
        private Mock<ILog> _mockLogger;

        [SetUp]
        public void Init()
        {
            _appServiceSettings = new Mock<IAppServiceSettings>();
            _mockLogger = new Mock<ILog>();

            var metaDataFactories = new List<IMetaDataFactory>
            {
                new FrameworkMetaDataFactory(),
                new FrameworkAimMetaDataFactory(),
                new FrameworkComponentTypeMetaDataFactory(),
                new LearningDeliveryMetaDataFactory()
            };

            //_readMetaDataFromCsv = new CsvService(new LarsMetaDataFactory(metaDataFactories));
            _readMetaDataFromCsv = new CsvService(new LarsMetaDataFactory());
            _httpGet = new HttpService(_mockLogger.Object);
            _fileExtractor = new ZipFileExtractor();
            _angleSharpService = new AngleSharpService(_httpGet);
            _httpGetFile = new HttpService(_mockLogger.Object);

            _sut = new LarsDataService(
                _appServiceSettings.Object,
                _readMetaDataFromCsv,
                _httpGetFile,
                _fileExtractor,
                _angleSharpService,
                _mockLogger.Object, 
                _httpGet);
        }

        [Test]
        [Ignore]
        public void ShouldGetFrameworks()
        {
            _mockLogger.Setup(x => x.Info(It.IsAny<object>())).Callback<object>(Console.WriteLine);

            _appServiceSettings.Setup(x => x.ImServiceBaseUrl).Returns("https://hub.imservices.org.uk");
            _appServiceSettings.Setup(x => x.ImServiceUrl).Returns("Learning%20Aims/Downloads/Pages/default.aspx");
            _appServiceSettings.Setup(x => x.CsvFileNameFrameworks).Returns("CSV/Framework.csv");
            _appServiceSettings.Setup(x => x.CsvFileNameFrameworksAim).Returns("CSV/FrameworkAims.csv");
            _appServiceSettings.Setup(x => x.CsvFileNameFrameworkComponentType).Returns("CSV/FrameworkComponentType.csv");
            _appServiceSettings.Setup(x => x.CsvFileNameLearningDelivery).Returns("CSV/LearningDelivery.csv");

            var frameworks = _sut.GetListOfCurrentFrameworks();

            frameworks.Should().NotBeNull();
            frameworks.Should().NotBeEmpty();
        }
    }
}