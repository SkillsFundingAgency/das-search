namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Services;
    using Moq;
    using NUnit.Framework;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core;
    using ApplicationServices.Services;

    [TestFixture]
    public sealed class ProviderIndexerTests
    {
        private ProviderIndexer _sut;
        private Mock<IIndexSettings<Core.Models.Provider.Provider>> _mockSettings;
        private Mock<IMaintainSearchIndexes<Core.Models.Provider.Provider>> _mockIndexMaintainer;
        private Mock<IGetApprenticeshipProviders> _mockProviderRepository;
        private Mock<IGetActiveProviders> _mockActiveProviderClient;

        [SetUp]
        public void Setup()
        {
            _mockSettings = new Mock<IIndexSettings<Core.Models.Provider.Provider>>();
            _mockIndexMaintainer = new Mock<IMaintainSearchIndexes<Core.Models.Provider.Provider>>();
            _mockProviderRepository = new Mock<IGetApprenticeshipProviders>();
            _mockActiveProviderClient = new Mock<IGetActiveProviders>();
            var mockLogger = Mock.Of<ILog>();

            _sut = new ProviderIndexer(_mockSettings.Object, _mockIndexMaintainer.Object, _mockProviderRepository.Object, _mockActiveProviderClient.Object, mockLogger);
        }

        [Test]
        public async Task ShouldLoadEntriesFromCourseDirectoryAndFilterOutTheInactiveOnes()
        {
            // Arrange
            _mockProviderRepository.Setup(x => x.GetApprenticeshipProvidersAsync()).Returns(Task.FromResult(TwoProvidersTask()));
            _mockActiveProviderClient.Setup(x => x.GetActiveProviders()).Returns(new List<int>() { 123 });

            // Act
            var entries = await _sut.LoadEntries();

            Assert.AreEqual(123, entries.First().Ukprn);
            Assert.AreEqual(1, entries.Count);

            _mockProviderRepository.VerifyAll();
            _mockActiveProviderClient.VerifyAll();
        }

        [Test]
        public void ShouldCreateIndexIfOneDoesNotAlreadyExist()
        {
            var testDate = new DateTime(2016, 5, 10);
            _sut.CreateIndex(testDate);

            _mockIndexMaintainer.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShouldCreateIndexWithTheCorrectName()
        {
            var testDate = new DateTime(2016, 5, 10, 14, 33, 30, DateTimeKind.Utc);
            const string testAliasName = "TestAlias";
            _mockSettings.SetupGet(x => x.IndexesAlias).Returns(testAliasName);
            _sut.CreateIndex(testDate);

            _mockIndexMaintainer.Verify(x => x.CreateIndex(It.Is<string>(y => y == $"testalias-2016-05-10-14")), Times.Once);
        }

        [Test]
        public void CreatIndexShouldDeleteAnyExistingIndexWithTheSameName()
        {
            var testDate = new DateTime(2016, 5, 10);
            _mockIndexMaintainer.Setup(x => x.IndexExists(It.IsAny<string>())).Returns(true);
            _sut.CreateIndex(testDate);

            _mockIndexMaintainer.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SwapIndexesShouldSwapTheIndexUsedToTheProvidedOne()
        {
            var testDate = new DateTime(2016, 5, 10, 14, 33, 30, DateTimeKind.Utc);
            const string testAliasName = "TestAlias";
            _mockSettings.SetupGet(x => x.IndexesAlias).Returns(testAliasName);
            _mockIndexMaintainer.Setup(x => x.AliasExists(It.IsAny<string>())).Returns(true);
            _sut.SwapIndexes(testDate);

            _mockIndexMaintainer.Verify(x => x.SwapAliasIndex(It.Is<string>(y => y == "TestAlias"), It.Is<string>(z => z == "testalias-2016-05-10-14")), Times.Once);
        }

        [Test]
        public void SwapIndexesShouldCreateAliasIfItDoesNotExist()
        {
            var testDate = new DateTime(2016, 5, 10, 14, 33, 30, DateTimeKind.Utc);
            const string testAliasName = "TestAlias";
            _mockSettings.SetupGet(x => x.IndexesAlias).Returns(testAliasName);
            _mockIndexMaintainer.Setup(x => x.AliasExists(It.IsAny<string>())).Returns(false);
            _sut.SwapIndexes(testDate);

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

        private IEnumerable<Core.Models.Provider.Provider> TwoProvidersTask()
        {
            return TwoProviders();
        }

        private IEnumerable<Core.Models.Provider.Provider> TwoProviders()
        {
            yield return new Core.Models.Provider.Provider() { Ukprn = 123 };
            yield return new Core.Models.Provider.Provider() { Ukprn = 456 };
        }
    }
}
