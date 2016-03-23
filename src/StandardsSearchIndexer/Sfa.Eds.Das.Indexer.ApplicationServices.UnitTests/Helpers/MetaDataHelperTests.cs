namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using Core.Models.Framework;
    using Core.Services;
    using MetaData;
    using Moq;
    using NUnit.Framework;
    using Standard;
    using Tools.MetaDataCreationTool;
    using Tools.MetaDataCreationTool.Services.Interfaces;

    [TestFixture]
    public class MetaDataHelperTests
    {
        [Test]
        public void GetAllStandardsMetaDataShouldLogErrorIfDataIsBadForStandard()
        {
            var stubMetaData = new Dictionary<string, string>();
            stubMetaData.Add("path1", "{ 'id': 1 }");
            stubMetaData.Add("path2", "{ 'id: 2 }"); // Has formatting error
            stubMetaData.Add("path3", "{ 'id': 3 }");

            var metaDataReader = new Mock<IGetStandardMetaData>();
            var metaDataGenerator = new Mock<IGenerateStandardMetaData>();
            var metaDataReaderFramework = new Mock<IGetFrameworkMetaData>();
            var logger = Mock.Of<ILog>();
            metaDataReader.Setup(x => x.GetAllAsJson()).Returns(stubMetaData);

            var helper = new MetaDataHelper(metaDataReader.Object, metaDataGenerator.Object, logger, metaDataReaderFramework.Object);

            var metaDataList = helper.GetAllStandardsMetaData();

            Assert.That(metaDataList.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetAllFrameworks()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(new List<FrameworkMetaData>
            {
                new FrameworkMetaData
                            {
                                EffectiveFrom = DateTime.Parse("2015-01-01"),
                                EffectiveTo = DateTime.MinValue,
                                FworkCode = 500,
                                PwayCode = 1,
                                ProgType = 2
                            }
            });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(1, frameworks.Count);
        }

        [Test]
        public void EffectiveToDateValid()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(new List<FrameworkMetaData>
            {
                new FrameworkMetaData
                               {
                                   EffectiveFrom = DateTime.Parse("2015-01-01"),
                                   EffectiveTo = DateTime.Parse("2015-01-02"), // Date in the past
                                   FworkCode = 500,
                                   PwayCode = 1,
                                   ProgType = 22,
                               }
            });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count, "Effective to date can not be in the past");
        }

        [Test]
        public void GetFrameworkWithAllValidData()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(new List<FrameworkMetaData>
            {
                new FrameworkMetaData
                               {
                                   EffectiveFrom = DateTime.Parse("2015-01-01"),
                                   EffectiveTo = DateTime.MinValue,
                                   FworkCode = 500,
                                   PwayCode = 1,
                                   ProgType = 2
                               }
            });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(1, frameworks.Count, "Should find one framework");
        }

        [TestCase(399)]
        public void FrameworkCodeMustBeOverValue(int value)
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(new List<FrameworkMetaData>
            {
                new FrameworkMetaData
                               {
                                   EffectiveFrom = DateTime.Parse("2015-01-01"),
                                   EffectiveTo = DateTime.MinValue,
                                   FworkCode = 399,
                                   PwayCode = 1,
                                   ProgType = 3
                               }
            });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count, $"Framework code must be over {value}");
        }

        [Test]
        public void PathwayCodeMustBeOver0()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(new List<FrameworkMetaData>
            {
                new FrameworkMetaData
                               {
                                   EffectiveFrom = DateTime.Parse("2015-01-01"),
                                   EffectiveTo = DateTime.MinValue,
                                   FworkCode = 500,
                                   PwayCode = 0,
                                   ProgType = 20
                               }
            });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count, "Pathway codemust be over 0");
        }

        [Test]
        public void FrameworkanOnlyHaveCertainValues()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(new List<FrameworkMetaData>
            {
                new FrameworkMetaData
                               {
                                   EffectiveFrom = DateTime.Parse("2015-01-01"),
                                   EffectiveTo = DateTime.MinValue,
                                   FworkCode = 500,
                                   PwayCode = 1,
                                   ProgType = 16
                               }
            });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count);
        }

        [Test]
        public void FrameworkanEffectiveFromCantBeNull()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(new List<FrameworkMetaData>
            {
                new FrameworkMetaData
                               {
                                   EffectiveFrom = DateTime.MinValue, // Not valid
                                   EffectiveTo = DateTime.MinValue,
                                   FworkCode = 500,
                                   PwayCode = 1,
                                   ProgType = 21
                               }
            });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count);
        }
    }
}