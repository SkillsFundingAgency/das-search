using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
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

        private List<string> _linkEndPoints;
        private FrameworkMetaData _framework;
        private FrameworkAimMetaData _frameworkAim;
        private FrameworkComponentTypeMetaData _frameworkComponentType;
        private LearningDeliveryMetaData _learningDelivery;
        private List<FrameworkMetaData> _frameworkList;
        private List<FrameworkAimMetaData> _frameworkAimList;
        private List<FrameworkComponentTypeMetaData> _frameworkComponentTypeList;
        private List<LearningDeliveryMetaData> _learningDeliveryList;

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

            _linkEndPoints = new List<string> { "endpoint" };
            _framework = new FrameworkMetaData
            {
                FworkCode = 2
            };

            _frameworkComponentType = new FrameworkComponentTypeMetaData
            {
                FrameworkComponentType = 1,
                FrameworkComponentTypeDesc = "Test framework component type"
            };
            
            _learningDelivery = new LearningDeliveryMetaData
            {
                LearnAimRef = "5001738X",
                LearnAimRefTitle = "Test Learning Delivery"
            };

            _frameworkAim = new FrameworkAimMetaData
            {
                FworkCode = _framework.FworkCode,
                FrameworkComponentType = _frameworkComponentType.FrameworkComponentType,
                LearnAimRef = "5001738X"
            };

            _frameworkList = new List<FrameworkMetaData> { _framework };
            _frameworkAimList = new List<FrameworkAimMetaData>{ _frameworkAim };
            _frameworkComponentTypeList = new List<FrameworkComponentTypeMetaData>{ _frameworkComponentType };
            _learningDeliveryList = new List<LearningDeliveryMetaData> { _learningDelivery };

            _mockCsvService.Setup(x => x.ReadFromString<FrameworkMetaData>(It.IsAny<string>())).Returns(_frameworkList);
            _mockCsvService.Setup(x => x.ReadFromString<FrameworkAimMetaData>(It.IsAny<string>())).Returns(_frameworkAimList);
            _mockCsvService.Setup(x => x.ReadFromString<FrameworkComponentTypeMetaData>(It.IsAny<string>())).Returns(_frameworkComponentTypeList);
            _mockCsvService.Setup(x => x.ReadFromString<LearningDeliveryMetaData>(It.IsAny<string>())).Returns(_learningDeliveryList);

            _mockAngleSharpService.Setup(x => x.GetLinks(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_linkEndPoints);

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
        public void ShouldPopulateFrameworkWithCompetenceQualifications()
        {
            // Assign
            var qualificationBuilder = new StringBuilder();
            qualificationBuilder.AppendLine("<p>");
            qualificationBuilder.AppendLine("Apprentices will achieve a practical (i.e. 'competence') qualification:");
            qualificationBuilder.AppendLine("<br />");
            qualificationBuilder.AppendLine("<ul>");
            qualificationBuilder.AppendLine($"<li>{_learningDelivery.LearnAimRefTitle}</li>");
            qualificationBuilder.AppendLine("</ul>");
            qualificationBuilder.AppendLine("</p>");


            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.Qualifications.Should().Be(qualificationBuilder.ToString());
        }

        [Test]
        public void ShouldPopulateFrameworkWithKnowledgeQualifications()
        {
            // Assign
            _frameworkComponentType.FrameworkComponentType = 2;
            _frameworkAim.FrameworkComponentType = 2;

            var qualificationBuilder = new StringBuilder();
            qualificationBuilder.AppendLine("<p>");
            qualificationBuilder.AppendLine("Apprentices will also achieve a theory-based (i.e. 'knowledge') qualification:");
            qualificationBuilder.AppendLine("<br />");
            qualificationBuilder.AppendLine("<ul>");
            qualificationBuilder.AppendLine($"<li>{_learningDelivery.LearnAimRefTitle}</li>");
            qualificationBuilder.AppendLine("</ul>");
            qualificationBuilder.AppendLine("</p>");

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.Qualifications.Should().Be(qualificationBuilder.ToString());
        }

        [Test]
        public void ShouldPopulateFrameworkWithCombinedQualifications()
        {
            // Assign
            _frameworkComponentType.FrameworkComponentType = 3;
            _frameworkAim.FrameworkComponentType = 3;

            var qualificationBuilder = new StringBuilder();
            qualificationBuilder.AppendLine("<p>");
            qualificationBuilder.AppendLine("Apprentices will achieve a practical and theory-based (i.e. 'combined') qualification:");
            qualificationBuilder.AppendLine("<br />");
            qualificationBuilder.AppendLine("<ul>");
            qualificationBuilder.AppendLine($"<li>{_learningDelivery.LearnAimRefTitle}</li>");
            qualificationBuilder.AppendLine("</ul>");
            qualificationBuilder.AppendLine("</p>");


            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.Qualifications.Should().Be(qualificationBuilder.ToString());
        }

        [Test]
        public void ShouldNotPopulateFrameworkWithQualificationsThatHaveExpired()
        {
            // Assign
            _frameworkAim.EffectiveTo = DateTime.Now.AddDays(-1);

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.Qualifications.Should().Be("None specified");
        }
    }
}
