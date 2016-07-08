using System;
using System.Collections.Generic;
using System.Linq;
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
                FworkCode = 500,
                PwayCode = 1,
                ProgType = 2,
                EffectiveFrom = DateTime.Now.AddDays(-4),
                EffectiveTo = DateTime.Now.AddDays(4)
            };

            _frameworkComponentType = new FrameworkComponentTypeMetaData
            {
                FrameworkComponentType = 1,
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
            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Count().Should().Be(1);
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Should().BeEmpty();

            var qualification = framework.CompetencyQualification.First();

            qualification.Should().Be(_learningDelivery.LearnAimRefTitle);
        }

        [Test]
        public void ShouldPopulateFrameworkWithKnowledgeQualifications()
        {
            // Assign
            _frameworkComponentType.FrameworkComponentType = 2;
            _frameworkAim.FrameworkComponentType = 2;

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Should().BeEmpty();
            framework.KnowledgeQualification.Count().Should().Be(1);
            framework.CombinedQualification.Should().BeEmpty();

            var qualification = framework.KnowledgeQualification.First();

            qualification.Should().Be(_learningDelivery.LearnAimRefTitle);
        }

        [Test]
        public void ShouldPopulateFrameworkWithCombinedQualifications()
        {
            // Assign
            _frameworkComponentType.FrameworkComponentType = 3;
            _frameworkAim.FrameworkComponentType = 3;

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Should().BeEmpty();
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Count().Should().Be(1);

            var qualification = framework.CombinedQualification.First();

            qualification.Should().Be(_learningDelivery.LearnAimRefTitle);
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

            framework.CompetencyQualification.Should().BeEmpty();
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Should().BeEmpty();
        }

        [Test]
        public void ShouldReturnUniqueQualifications()
        {
            // Assign
            var newLearnDelivery = new LearningDeliveryMetaData
            {
                LearnAimRef = "new10101",
                LearnAimRefTitle = _learningDelivery.LearnAimRefTitle
            };

            _learningDeliveryList.Add(newLearnDelivery);

            var newAim = new FrameworkAimMetaData
            {
                FworkCode = _framework.FworkCode,
                FrameworkComponentType = _frameworkComponentType.FrameworkComponentType,
                LearnAimRef = newLearnDelivery.LearnAimRef
            };

            _frameworkAimList.Add(newAim);

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Count().Should().Be(1);
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Should().BeEmpty();

            framework.CompetencyQualification.First().Should().Be(_learningDelivery.LearnAimRefTitle);
        }

        [Test]
        public void ShouldReturnUniqueQualificationsEvenIfCaseIsDifferent()
        {
            // Assign
            var newLearnDelivery = new LearningDeliveryMetaData
            {
                LearnAimRef = "new10101",
                LearnAimRefTitle = _learningDelivery.LearnAimRefTitle.ToUpperInvariant()
            };

            _learningDeliveryList.Add(newLearnDelivery);

            var newAim = new FrameworkAimMetaData
            {
                FworkCode = _framework.FworkCode,
                FrameworkComponentType = _frameworkComponentType.FrameworkComponentType,
                LearnAimRef = newLearnDelivery.LearnAimRef
            };

            _frameworkAimList.Add(newAim);

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Count().Should().Be(1);
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Should().BeEmpty();

            framework.CompetencyQualification.First().Should().Be(_learningDelivery.LearnAimRefTitle);
        }

        [Test]
        public void ShouldNotHaveSameQualificationInCombinedAndOtherComponentType()
        {
            // Assign
            const string learnRef = "new10101";
            _learningDeliveryList.Add(new LearningDeliveryMetaData
            {
                LearnAimRef = learnRef,
                LearnAimRefTitle = _learningDelivery.LearnAimRefTitle
            });

            _frameworkAimList.Add(new FrameworkAimMetaData
            {
                FworkCode = _framework.FworkCode,
                FrameworkComponentType = 2,
                LearnAimRef = learnRef
            });

            _frameworkAimList.Add(new FrameworkAimMetaData
            {
                FworkCode = _framework.FworkCode,
                FrameworkComponentType = 3, // Combined Component Type (should not be in any other type)
                LearnAimRef = learnRef
            });

            _frameworkComponentTypeList.Add(new FrameworkComponentTypeMetaData
            {
                FrameworkComponentType = 2
            });

            _frameworkComponentTypeList.Add(new FrameworkComponentTypeMetaData
            {
                FrameworkComponentType = 3
            });

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Should().BeEmpty();
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Count().Should().Be(1);

            // If a qualification is in the combined component type it should be removed from the other two types
            framework.CombinedQualification.First().Should().Be(_learningDelivery.LearnAimRefTitle);
        }

        [Test]
        // The QCF certification for qualiifications is no longer in use to we need to remove it before we check
        // for duplicates
        public void ShouldRemoveOldQCFLabelFromQualificationTitle()
        {
            // Act
            var actualTitle = "This is a (QCFT) test QCF title which should remove from here (QCF)";
            var expectedTitle = "This is a (QCFT) test QCF title which should remove from here";

            _learningDelivery.LearnAimRefTitle = actualTitle;

            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Count().Should().Be(1);
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Should().BeEmpty();

            var qualification = framework.CompetencyQualification.First();

            qualification.Should().Be(expectedTitle);
        }
    }
}
