namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Services;
    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core;

    [TestFixture]
    public class ProviderHelperTests
    {
        [Test]
        public async Task ShouldLoadEntriesFromCourseDirectoryAndFilterOutTheInactiveOnes()
        {
            // Arrange
            var settings = Mock.Of<IIndexSettings<Core.Models.Provider.Provider>>();
            var mockElasticSearchClient = new Mock<IElasticsearchClientFactory>();
            var mockApprenticeshipProviderRepository = new Mock<IGetApprenticeshipProviders>();
            var mockActiveProviderClient = new Mock<IGetActiveProviders>();
            var indexServiceMock = new Mock<IIndexMaintenanceService>();
            var sut = new ProviderHelper(settings, mockElasticSearchClient.Object, mockApprenticeshipProviderRepository.Object, mockActiveProviderClient.Object, indexServiceMock.Object, Mock.Of<ILog>());

            mockApprenticeshipProviderRepository.Setup(x => x.GetApprenticeshipProvidersAsync()).Returns(Task.FromResult(TwoProvidersTask()));
            mockActiveProviderClient.Setup(x => x.GetActiveProviders()).Returns(new List<int>() { 123 });

            // Act
            var entries = await sut.LoadEntries();

            Assert.AreEqual(123, entries.First().Ukprn);
            Assert.AreEqual(1, entries.Count);

            mockApprenticeshipProviderRepository.VerifyAll();
            mockActiveProviderClient.VerifyAll();
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
