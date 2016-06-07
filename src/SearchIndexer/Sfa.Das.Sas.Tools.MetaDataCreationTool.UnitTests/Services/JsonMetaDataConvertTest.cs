namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Services
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Sfa.Das.Sas.Indexer.Core.Services;
    using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services;

    [TestFixture]
    public class JsonMetaDataConvertTest
    {
        [Test]
        public void WhenConvertingJsons()
        {
            var stubMetaData = new Dictionary<string, string>
            {
                { "path1", "{ 'id': 1 }" },
                { "path2", "{ 'id': 2 }" },
                { "path3", "{ 'id': 3 }" }
            };
            var mockLogger = new Mock<ILog>();

            var converter = new JsonMetaDataConvert(mockLogger.Object);

            var result = converter.DeserializeObject<TestObject>(stubMetaData);
            mockLogger.Verify(m => m.Warn(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            result.Count.ShouldBeEquivalentTo(3);
        }

        [Test]
        public void WhenOneRecordHaveFormatingError()
        {
            var stubMetaData = new Dictionary<string, string>
            {
                { "path1", "{ 'id': 1 }" },
                { "path2", "{ 'id: 2 }" },
                { "path3", "{ 'id': 3 }" }
            };
            var mockLogger = new Mock<ILog>();

            var converter = new JsonMetaDataConvert(mockLogger.Object);

            var result = converter.DeserializeObject<TestObject>(stubMetaData);

            mockLogger.Verify(m => m.Warn(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            result.Count.ShouldBeEquivalentTo(2);
        }
    }

    internal class TestObject
    {
        public int Id { get; set; }
    }
}
