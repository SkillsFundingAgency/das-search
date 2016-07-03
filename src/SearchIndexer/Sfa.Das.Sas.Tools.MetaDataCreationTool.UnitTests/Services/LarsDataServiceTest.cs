using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Services
{
    [TestFixture]
    public class LarsDataServiceTest
    {
        private LarsDataService _sut;
        private Mock<IReadMetaDataFromCsv> _mockCsvService;
        private Mock<IUnzipStream> _mockFileExtractor;
        private Mock<IAngleSharpService> _mockAngleSharpService;
        private Mock<IHttpGet> _mockHttpGet;
        private Mock<IHttpGetFile> _mockHttpGetFile;
        private Mock<ILog> _mockLogger;
        private Mock<IAppServiceSettings> _mockAppServiceSettings;

        [SetUp]
        public void Init()
        {
            _mockAngleSharpService = new Mock<IAngleSharpService>();
            _mockAppServiceSettings = new Mock<IAppServiceSettings>();
            _mockCsvService = new Mock<IReadMetaDataFromCsv>();
            _mockFileExtractor = new Mock<IUnzipStream>();
            _mockHttpGet = new Mock<IHttpGet>();
            _mockHttpGetFile = new Mock<IHttpGetFile>();
            _mockLogger = new Mock<ILog>();

            _sut = new LarsDataService(
                _mockAppServiceSettings.Object,
                _mockCsvService.Object,
                _mockHttpGetFile.Object,
                _mockFileExtractor.Object,
                _mockAngleSharpService.Object,
                _mockLogger.Object,
                _mockHttpGet.Object);
        }

        [Test]
        public void TestMethod1()
        {
        }
    }
}
