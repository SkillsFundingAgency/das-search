namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;

    [TestFixture]
    public class ProviderHelperTests
    {
        [Test]
        public void ShouldLoadEntriesFromCourseDirectoryAndFilterOutTheInactiveOnes()
        {
            // Arrange
            var settings = Mock.Of<IIndexSettings<ProviderOld>>();
            var mockElasticSearchClient = new Mock<IElasticsearchClientFactory>();
            var mockCourseDirectoryClient = new Mock<IGetProviders>();
            var mockActiveProviderClient = new Mock<IGetActiveProviders>();
            var indexServiceMock = new Mock<IIndexMaintenanceService>();
            var sut = new ProviderHelper(settings, mockElasticSearchClient.Object, mockCourseDirectoryClient.Object, mockActiveProviderClient.Object, indexServiceMock.Object, Mock.Of<ILog>());

            mockCourseDirectoryClient.Setup(x => x.GetProviders()).Returns(TwoProvidersTask());
            mockActiveProviderClient.Setup(x => x.GetActiveProviders()).Returns(new List<string>() { "123" });

            // Act
            var entries = sut.LoadEntries();

            Assert.AreEqual("123", entries.First().UkPrn);
            Assert.AreEqual(1, entries.Count);

            mockCourseDirectoryClient.VerifyAll();
            mockActiveProviderClient.VerifyAll();
        }

        private async Task<IEnumerable<ProviderOld>> TwoProvidersTask()
        {
            return TwoProviders();
        }

        private IEnumerable<ProviderOld> TwoProviders()
        {
            yield return new ProviderOld() { UkPrn = "123" };
            yield return new ProviderOld() { UkPrn = "456" };
        } 
    }
}
