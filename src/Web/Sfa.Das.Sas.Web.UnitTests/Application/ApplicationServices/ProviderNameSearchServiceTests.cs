using System;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.UnitTests.Application.ApplicationServices
{
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;
    using Sas.ApplicationServices;
    using Sas.ApplicationServices.Interfaces;
    using Sas.ApplicationServices.Settings;
    using SFA.DAS.NLog.Logger;

    [TestFixture]
    public sealed class ProviderNameSearchServiceTests
    {
        private Mock<IPaginationSettings> _mockPaginationSettings;
        private Mock<IProviderNameSearchProvider> _mockNameSearchProvider;
        private Mock<ILog> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILog>();
            _mockNameSearchProvider = new Mock<IProviderNameSearchProvider>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();
        }

        [Test]
        public async Task ShouldCallProviderWithExpectedParameters()
        {
            const int numberOfItemsToReturn = 10;
            const int pageNumber = 1;

            _mockNameSearchProvider.Setup(m => m.SearchByTerm(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            _mockPaginationSettings.Setup(p => p.DefaultResultsAmount).Returns(numberOfItemsToReturn);

            var providerNameSearchService = new ProviderNameSearchService(_mockPaginationSettings.Object, _mockNameSearchProvider.Object, _mockLogger.Object);

            await providerNameSearchService.SearchProviderNameAndAliases("test", pageNumber);

            _mockNameSearchProvider.Verify(x => x.SearchByTerm("test", pageNumber, numberOfItemsToReturn));
        }

        [Test]
        public async Task ShouldLogInitialDetailsWithExpectedParameters()
        {
            const int numberOfItemsToReturn = 10;
            const int pageNumber = 1;
            const string searchTerm = "test";
            _mockNameSearchProvider.Setup(m => m.SearchByTerm(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            _mockPaginationSettings.Setup(p => p.DefaultResultsAmount).Returns(numberOfItemsToReturn);

            var providerNameSearchService = new ProviderNameSearchService(_mockPaginationSettings.Object, _mockNameSearchProvider.Object, _mockLogger.Object);

            await providerNameSearchService.SearchProviderNameAndAliases(searchTerm, pageNumber);

            var expected = $"Provider Name Search started: SearchTerm: [{searchTerm}], Page: [{pageNumber}], Page Size: [{numberOfItemsToReturn}]";

            _mockLogger.Verify(x => x.Info(expected));
        }

        [Test]
        public async Task ShouldLogErrorWithFailedCall()
        {
            const int numberOfItemsToReturn = 10;
            const int pageNumber = 1;
            const string searchTerm = "test";
            _mockNameSearchProvider.Setup(m => m.SearchByTerm(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            _mockPaginationSettings.Setup(p => p.DefaultResultsAmount).Returns(numberOfItemsToReturn);

            var providerNameSearchService = new ProviderNameSearchService(_mockPaginationSettings.Object, _mockNameSearchProvider.Object, _mockLogger.Object);

            var result = await providerNameSearchService.SearchProviderNameAndAliases(searchTerm, pageNumber);

            var expected = $"Provider Name Search error: SearchTerm: [{searchTerm}], Page: [{pageNumber}], Page Size: [{numberOfItemsToReturn}]";

            _mockLogger.Verify(x => x.Info(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(x => x.Error(It.IsAny<Exception>(), expected));
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(ProviderNameSearchResponseCodes.SearchFailed, result.ResponseCode);
        }

        [Test]
        public async Task ShouldReturnSuccessCodeAndNotError()
        {
            const int numberOfItemsToReturn = 10;
            const int pageNumber = 1;
            const string searchTerm = "test";
            var returnedResults = new ProviderNameSearchResultsAndPagination { ActualPage = 1, HasError = false, TotalResults = 1 };
            _mockNameSearchProvider.Setup(m => m.SearchByTerm(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(returnedResults));
            _mockPaginationSettings.Setup(p => p.DefaultResultsAmount).Returns(numberOfItemsToReturn);

            var providerNameSearchService = new ProviderNameSearchService(_mockPaginationSettings.Object, _mockNameSearchProvider.Object, _mockLogger.Object);

            var result = await providerNameSearchService.SearchProviderNameAndAliases(searchTerm, pageNumber);

            var expected = $"Provider Name Search complete: SearchTerm: [{searchTerm}], Page: [{result.ActualPage}], Page Size: [{numberOfItemsToReturn}], Total Results: [{result.TotalResults}]";

            _mockLogger.Verify(x => x.Info(expected));
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(ProviderNameSearchResponseCodes.Success, result.ResponseCode);

        }

    }
}
