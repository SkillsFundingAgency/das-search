namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.ApplicationServices.Handlers;
    using Sas.ApplicationServices.Interfaces;
    using Sas.ApplicationServices.Models;
    using Sas.ApplicationServices.Queries;
    using Sas.ApplicationServices.Responses;
    using Sas.ApplicationServices.Settings;
    using SFA.DAS.NLog.Logger;

    [TestFixture]
    public class ProviderNameSearchHandlerTests
    {
        private Mock<IProviderNameSearchService> _mockProviderNameSearchService;
        private Mock<ILog> _mockLogger;
        private Mock<IPaginationSettings> _mockPaginationSettings;
        private ProviderNameSearchHandler _handler;
        private int _actualPage;
        private int _lastPage;
        private ProviderNameSearchResponseCodes _responseCode;
        private int _resultsToTake;
        private string _searchTerm;
        private int _totalResults;

        [SetUp]
        public void Setup()
        {
             _mockProviderNameSearchService = new Mock<IProviderNameSearchService>();
            _mockLogger = new Mock<ILog>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();

            _actualPage = 1;
            _lastPage = 2;
            _resultsToTake = 20;
            _searchTerm = "coventry";
            _totalResults = 21;

            _responseCode = ProviderNameSearchResponseCodes.Success;

            var providerNameSearchResults = new ProviderNameSearchResults
            {
               ActualPage = _actualPage,
                HasError = false,
                LastPage = _lastPage,
                ResponseCode = _responseCode,
                Results = null,
                ResultsToTake = _resultsToTake,
                TotalResults = _totalResults
            };

            _mockProviderNameSearchService.Setup(
                    x => x.SearchProviderNameAndAliases(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(providerNameSearchResults));

             _handler = new ProviderNameSearchHandler(
                 _mockProviderNameSearchService.Object,
                 _mockPaginationSettings.Object,
                 _mockLogger.Object);
        }


        [Test]
        public async Task ShouldReturnSuccessWhenSearchIsSuccessful()
        {
            var message = new ProviderNameSearchQuery { SearchTerm = "test", Page = 1 };

            var response = await _handler.Handle(message);

            response.StatusCode.Should().Be(ProviderNameSearchResponseCodes.Success);
        }

        [Test]
        public async Task ShouldReturnExpectedResultsWhenSearchIsComplete()
        {
            var message = new ProviderNameSearchQuery { SearchTerm = _searchTerm, Page = 1 };

            var response = await _handler.Handle(message);

            response.ActualPage.Should().Be(_actualPage);
            response.HasError.Should().BeFalse();
            response.LastPage.Should().Be(_lastPage);
            response.ResultsToTake.Should().Be(_resultsToTake);
            response.SearchTerm.Should().Be(_searchTerm);
            response.TotalResults.Should().Be(_totalResults);
        }
    }
}
