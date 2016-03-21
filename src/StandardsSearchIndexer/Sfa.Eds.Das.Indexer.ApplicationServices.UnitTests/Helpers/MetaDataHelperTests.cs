namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Helpers
{
    using System.Collections.Generic;
    using Core.Services;
    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Standard;

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
    }
}