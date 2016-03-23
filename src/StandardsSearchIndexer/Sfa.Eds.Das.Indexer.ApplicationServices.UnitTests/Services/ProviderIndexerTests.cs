namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Services;

    [TestFixture]
    public sealed class ProviderIndexerTests
    {
        private ProviderIndexer _sut;

        private Mock<IIndexSettings<IMaintainProviderIndex>> _mockSettings;
        private Mock<IMaintainProviderIndex> _mockIndexMaintainer;
        private Mock<IGetApprenticeshipProviders> _mockProviderRepository;

        private Mock<IGetActiveProviders> _mockActiveProviderClient;

        private Mock<IProviderFeatures> _mockFeatures;

        [SetUp]
        public void Setup()
        {
            _mockSettings = new Mock<IIndexSettings<IMaintainProviderIndex>>();
            _mockIndexMaintainer = new Mock<IMaintainProviderIndex>();
            _mockProviderRepository = new Mock<IGetApprenticeshipProviders>();
            _mockActiveProviderClient = new Mock<IGetActiveProviders>();
            _mockFeatures = new Mock<IProviderFeatures>();
            var mockLogger = Mock.Of<ILog>();

            _sut = new ProviderIndexer(_mockSettings.Object, _mockIndexMaintainer.Object, _mockFeatures.Object, _mockProviderRepository.Object, _mockActiveProviderClient.Object, mockLogger);
        }

        [Test]
        public void ShouldCreateIndexIfOneDoesNotAlreadyExist()
        {
            _sut.CreateIndex("testindex");

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
            _mockIndexMaintainer.Setup(x => x.AliasExists(It.IsAny<string>())).Returns(true);
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

        [Test]
        public void ShouldFilterProvidersIfTheFeatureIsEnabled()
        {
            _mockFeatures.Setup(x => x.FilterInactiveProviders).Returns(true);
            _mockActiveProviderClient.Setup(x => x.GetActiveProviders()).Returns(new[] { 123 });
            _mockProviderRepository.Setup(x => x.GetApprenticeshipProvidersAsync()).Returns(TwoProvidersTask());

            var result = _sut.LoadEntries().Result;

            Assert.AreEqual(1, result.Count);

            _mockActiveProviderClient.VerifyAll();
            _mockProviderRepository.VerifyAll();
        }

        [Test]
        public void ShouldntFilterProvidersIfTheFeatureIsDisabled()
        {
            _mockFeatures.Setup(x => x.FilterInactiveProviders).Returns(false);
            _mockProviderRepository.Setup(x => x.GetApprenticeshipProvidersAsync()).Returns(TwoProvidersTask());

            var result = _sut.LoadEntries().Result;

            Assert.AreEqual(2, result.Count);

            _mockProviderRepository.VerifyAll();
        }

        private Task<IEnumerable<Core.Models.Provider.Provider>> TwoProvidersTask()
        {
            return Task.FromResult(TwoProviders());
        }

        private IEnumerable<Core.Models.Provider.Provider> TwoProviders()
        {
            yield return new Core.Models.Provider.Provider() { Ukprn = 123 };
            yield return new Core.Models.Provider.Provider() { Ukprn = 456 };
        }
    }
}