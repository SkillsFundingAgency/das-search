using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.ProviderIndexer.UnitTests.Helpers
{
    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.Common.Configuration;
    using Sfa.Eds.Das.Indexer.Common.Settings;
    using Sfa.Eds.Das.ProviderIndexer.Clients;
    using Sfa.Eds.Das.ProviderIndexer.Helpers;
    using Sfa.Eds.Das.ProviderIndexer.Models;

    [TestFixture]
    public class ProviderHelperTests
    {
        [Test]
        public void ShouldLoadEntriesFromCourseDirectoryAndFilterOutTheInactiveOnes()
        {
            // Arrange
            var settings = Mock.Of<IIndexSettings<Provider>>();
            var mockElasticSearchClient = new Mock<IElasticsearchClientFactory>();
            var mockCourseDirectoryClient = new Mock<ICourseDirectoryClient>();
            var mockActiveProviderClient = new Mock<IActiveProviderClient>();

            var sut = new ProviderHelper(settings, mockElasticSearchClient.Object, mockCourseDirectoryClient.Object, mockActiveProviderClient.Object);

            mockCourseDirectoryClient.Setup(x => x.GetProviders()).Returns(TwoProvidersTask());
            mockActiveProviderClient.Setup(x => x.GetProviders()).Returns(new List<string>() { "123" });

            // Act
            var entries = sut.LoadEntries();

            Assert.AreEqual("123", entries.First().UkPrn);
            Assert.AreEqual(1, entries.Count);

            mockCourseDirectoryClient.VerifyAll();
            mockActiveProviderClient.VerifyAll();
        }

        private async Task<IEnumerable<Provider>> TwoProvidersTask()
        {
            return TwoProviders();
        }

        private IEnumerable<Provider> TwoProviders()
        {
            yield return new Provider() { UkPrn = "123" };
            yield return new Provider() { UkPrn = "456" };
        } 
    }
}
