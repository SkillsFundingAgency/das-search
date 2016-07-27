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
        private Mock<IHttpGetFile> _mockHttpGetFile;
        private Mock<ILog> _mockLogger;
        private Mock<IAppServiceSettings> _mockAppServiceSettings;

        private List<string> _linkEndPoints;
        private FrameworkMetaData _framework;
        private FrameworkAimMetaData _frameworkAim;
        private FrameworkComponentTypeMetaData _frameworkComponentType;
        private LearningDeliveryMetaData _learningDelivery;
        private FundingMetaData _fundingMetaData;
        private List<FrameworkMetaData> _frameworkList;
        private List<FrameworkAimMetaData> _frameworkAimList;
        private List<FrameworkComponentTypeMetaData> _frameworkComponentTypeList;
        private List<LearningDeliveryMetaData> _learningDeliveryList;
        private List<FundingMetaData> _fundingList;

        [SetUp]
        public void Init()
        {
            _mockAngleSharpService = new Mock<IAngleSharpService>();
            _mockAppServiceSettings = new Mock<IAppServiceSettings>();
            _mockCsvService = new Mock<IReadMetaDataFromCsv>();
            _mockFileExtractor = new Mock<IUnzipStream>();
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

            _frameworkAim = new FrameworkAimMetaData
            {
                FworkCode = _framework.FworkCode,
                PwayCode = _framework.PwayCode,
                ProgType = _framework.ProgType,
                FrameworkComponentType = _frameworkComponentType.FrameworkComponentType,
                LearnAimRef = "5001738X"
            };

            _learningDelivery = new LearningDeliveryMetaData
            {
                LearnAimRef = _frameworkAim.LearnAimRef,
                LearnAimRefTitle = "Test Learning Delivery"
            };

            _fundingMetaData = new FundingMetaData
            {
                LearnAimRef = _frameworkAim.LearnAimRef,
                EffectiveFrom = DateTime.Now.AddYears(-2),
                EffectiveTo = DateTime.Now.AddDays(20),
                FundingCategory = "APP_ACT_COST", // This category is used to determine an apprenticeship funded qualification
                RateWeighted = 150
            };

            _frameworkList = new List<FrameworkMetaData> { _framework };
            _frameworkAimList = new List<FrameworkAimMetaData> { _frameworkAim };
            _frameworkComponentTypeList = new List<FrameworkComponentTypeMetaData> { _frameworkComponentType };
            _learningDeliveryList = new List<LearningDeliveryMetaData> { _learningDelivery };
            _fundingList = new List<FundingMetaData> { _fundingMetaData };

            _mockCsvService.Setup(x => x.ReadFromString<FrameworkMetaData>(It.IsAny<string>())).Returns(_frameworkList);
            _mockCsvService.Setup(x => x.ReadFromString<FrameworkAimMetaData>(It.IsAny<string>())).Returns(_frameworkAimList);
            _mockCsvService.Setup(x => x.ReadFromString<FrameworkComponentTypeMetaData>(It.IsAny<string>())).Returns(_frameworkComponentTypeList);
            _mockCsvService.Setup(x => x.ReadFromString<LearningDeliveryMetaData>(It.IsAny<string>())).Returns(_learningDeliveryList);
            _mockCsvService.Setup(x => x.ReadFromString<FundingMetaData>(It.IsAny<string>())).Returns(_fundingList);

            _mockAngleSharpService.Setup(x => x.GetLinks(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_linkEndPoints);

            _sut = new LarsDataService(
                _mockAppServiceSettings.Object,
                _mockCsvService.Object,
                _mockHttpGetFile.Object,
                _mockFileExtractor.Object,
                _mockAngleSharpService.Object,
                _mockLogger.Object);
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
                PwayCode = _framework.PwayCode,
                ProgType = _framework.ProgType,
                FrameworkComponentType = 2,
                LearnAimRef = learnRef
            });

            _frameworkAimList.Add(new FrameworkAimMetaData
            {
                FworkCode = _framework.FworkCode,
                PwayCode = _framework.PwayCode,
                ProgType = _framework.ProgType,
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

            _fundingList.Add(new FundingMetaData { LearnAimRef = learnRef, RateWeighted = 150, FundingCategory = "APP_ACT_COST" });

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

        // The QCF certification for qualiifications is no longer in use to we need to remove it before we check
        // for duplicates
        [Test]
        public void ShouldRemoveOldQcfLabelFromQualificationTitle()
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

        [Test]
        public void ShouldNotProcessFrameworkThatIsOutOfDate()
        {
            _frameworkList.Add(new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("2015-01-01"),
                EffectiveTo = DateTime.Parse("2015-01-02"), // Date in the past
                FworkCode = 500,
                PwayCode = 1,
                ProgType = 22,
            });

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);
            frameworks.First().FworkCode.Should().Be(_framework.FworkCode);
        }

        [TestCase(399)]
        public void ShouldNotProcessFrameworkThatHasCodeThatIsOutOfBounds(int value)
        {
            _frameworkList.Add(new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("2015-01-01"),
                EffectiveTo = DateTime.Parse("2017-01-02"),
                FworkCode = 399, // we should not process frameworks below 400
                PwayCode = 1,
                ProgType = 22,
            });

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);
            frameworks.First().FworkCode.Should().Be(_framework.FworkCode);
        }

        [Test]
        public void ShouldNotProcessFrameworkThatHasAPathwayOfZero()
        {
            _frameworkList.Add(new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("2015-01-01"),
                EffectiveTo = DateTime.Parse("2017-01-02"),
                FworkCode = 450,
                PwayCode = 0,
                ProgType = 22,
            });

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);
            frameworks.First().FworkCode.Should().Be(_framework.FworkCode);
        }

        [Test]
        public void ShouldNotProcessFrameworkThatHasAUnsupportedPrgType()
        {
            _frameworkList.Add(new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("2015-01-01"),
                EffectiveTo = DateTime.Parse("2017-01-02"),
                FworkCode = 450,
                PwayCode = 1,
                ProgType = 16,
            });

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);
            frameworks.First().FworkCode.Should().Be(_framework.FworkCode);
        }

        [Test]
        public void ShouldNotProcessFrameworkThatHasANotValidEffectiveFromDate()
        {
            _frameworkList.Add(new FrameworkMetaData
            {
                EffectiveFrom = DateTime.MinValue,
                EffectiveTo = DateTime.Parse("2017-01-02"),
                FworkCode = 450,
                PwayCode = 1,
                ProgType = 22,
            });

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);
            frameworks.First().FworkCode.Should().Be(_framework.FworkCode);
        }

        [Test]
        public void ShouldNotAddQualificationWithUnweightedFundingToFramework()
        {
            // Assign
            _fundingMetaData.RateWeighted = 0;

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
        public void ShouldNotAddQualificationWithExpiredFundingToFramework()
        {
            // Assign
            _fundingMetaData.EffectiveTo = DateTime.Now.AddDays(-1);

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
        public void ShouldAddQualificationWithNoEffectiveToDateButWeightedFundingToFramework()
        {
            // Assign
            _fundingMetaData.EffectiveTo = null;

            // Act
            var frameworks = _sut.GetListOfCurrentFrameworks();

            // Assert
            frameworks.Count.Should().Be(1);

            var framework = frameworks.First();

            framework.CompetencyQualification.Count().Should().Be(1);
            framework.KnowledgeQualification.Should().BeEmpty();
            framework.CombinedQualification.Should().BeEmpty();
        }
    }
}
