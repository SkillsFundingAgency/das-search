namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ApplicationServices.Services;
    using Core.Models;
    using Core.Services;
    using Moq;
    using NUnit.Framework;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Standard;

    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    [TestFixture]
    public sealed class StandardIndexerTests
    {
        private StandardIndexer _sut;
        private Mock<IIndexSettings<MetaDataItem>> _mockSettings;
        private Mock<IMaintainSearchIndexes<MetaDataItem>> _mockIndexMaintainer;
        private Mock<IMetaDataHelper> _mockMetaDataHelper;

        [SetUp]
        public void Setup()
        {
            _mockSettings = new Mock<IIndexSettings<MetaDataItem>>();
            _mockIndexMaintainer = new Mock<IMaintainSearchIndexes<MetaDataItem>>();
            _mockMetaDataHelper = new Mock<IMetaDataHelper>();
            var mockLogger = Mock.Of<ILog>();

            _sut = new StandardIndexer(_mockSettings.Object, _mockIndexMaintainer.Object, _mockMetaDataHelper.Object, mockLogger);
        }

        [Test]
        public void ShouldCreateIndexIfOneDoesNotAlreadyExist()
        {
            _sut.CreateIndex("tetsindex");

            _mockIndexMaintainer.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShouldCreateIndexWithTheCorrectName()
        {
            const string testAliasName = "TestAlias";
            _mockSettings.SetupGet(x => x.IndexesAlias).Returns(testAliasName);
            _sut.CreateIndex("testalias-2016-05-10-14");

            _mockIndexMaintainer.Verify(x => x.CreateIndex(It.Is<string>(y => y == $"testalias-2016-05-10-14")), Times.Once);
        }

        [Test]
        public void CreatIndexShouldDeleteAnyExistingIndexWithTheSameName()
        {
            _mockIndexMaintainer.Setup(x => x.IndexExists(It.IsAny<string>())).Returns(true);
            _sut.CreateIndex("testindex");

            _mockIndexMaintainer.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SwapIndexesShouldSwapTheIndexUsedToTheProvidedOne()
        {
            const string testAliasName = "TestAlias";
            _mockSettings.SetupGet(x => x.IndexesAlias).Returns(testAliasName);
            _sut.SwapIndexes("testalias-2016-05-10-14");

            _mockIndexMaintainer.Verify(x => x.SwapAliasIndex(It.Is<string>(y => y == "TestAlias"), It.Is<string>(z => z == "testalias-2016-05-10-14")), Times.Once);
        }

        [Test]
        public void SwapIndexesShouldCreateAliasIfItDoesNotExist()
        {
            const string testAliasName = "TestAlias";
            _mockSettings.SetupGet(x => x.IndexesAlias).Returns(testAliasName);
            _mockIndexMaintainer.Setup(x => x.AliasExists(It.IsAny<string>())).Returns(false);
            _sut.SwapIndexes("testalias-2016-05-10-14");

            _mockIndexMaintainer.Verify(x => x.CreateIndexAlias(It.Is<string>(z => z == "TestAlias"), It.Is<string>(y => y == "testalias-2016-05-10-14")), Times.Once);
        }

        [Test]
        public void DeleteIndexesShouldDeleteIndexesOfLastTwoDays()
        {
            Func<string, bool> matcher = null;
            var testDate = new DateTime(2016, 5, 10, 14, 33, 30, DateTimeKind.Utc);
            const string testAliasName = "TestAlias";
            _mockSettings.SetupGet(x => x.IndexesAlias).Returns(testAliasName);
            _mockIndexMaintainer.Setup(x => x.DeleteIndexes(It.IsAny<Func<string, bool>>())).Callback<Func<string, bool>>(y => matcher = y);

            _sut.DeleteOldIndexes(testDate);

            _mockIndexMaintainer.Verify(x => x.DeleteIndexes(It.IsAny<Func<string, bool>>()), Times.Once);
            var match1 = matcher.Invoke("testalias-2016-05-09");
            var match2 = matcher.Invoke("testalias-2016-05-08");
            var match3 = matcher.Invoke("testalias-2016-05-07");

            Assert.That(match1, Is.True);
            Assert.That(match2, Is.True);
            Assert.That(match3, Is.False);
        }
    }
}
