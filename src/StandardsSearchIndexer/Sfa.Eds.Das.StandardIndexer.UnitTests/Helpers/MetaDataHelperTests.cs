using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.StandardIndexer.Helpers;
using Sfa.Eds.Das.Tools.MetaDataCreationTool;
using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;
using System.Collections.Generic;

namespace Sfa.Eds.Das.StandardIndexer.UnitTests.Helpers
{
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
            metaDataReader.Setup(x => x.GetAllAsJson()).Returns(stubMetaData);

            var helper = new MetaDataHelper(metaDataReader.Object, metaDataGenerator.Object);

            var metaDataList = helper.GetAllStandardsMetaData();

            Assert.That(metaDataList.Count, Is.EqualTo(2));
        }
    }
}
