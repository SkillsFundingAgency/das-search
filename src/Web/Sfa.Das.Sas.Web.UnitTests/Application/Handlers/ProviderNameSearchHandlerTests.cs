using System.Collections.Generic;
using System.Threading;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Model;

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

    [TestFixture]
    public class ProviderNameSearchHandlerTests
    {
        private Mock<IProviderSearchProvider> _mockProviderNameSearchService;
        private Mock<IPaginationSettings> _mockPaginationSettings;
        private ProviderNameSearchHandler _handler;
        private int _actualPage;
        private int _lastPage;
        private ProviderNameSearchResponseCodes _responseCode;
        private int _resultsToTake;
        private string _searchTerm;
        private int _totalResults;
        private int _defaultPageSize;
        private List<ProviderNameSearchResult> _searchResults;

        [SetUp]
        public void Setup()
        {
            _mockProviderNameSearchService = new Mock<IProviderSearchProvider>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();
            _actualPage = 1;
            _lastPage = 2;
            _resultsToTake = 20;
            _searchTerm = "coventry";
            _totalResults = 21;
            _defaultPageSize = 10;

            _responseCode = ProviderNameSearchResponseCodes.Success;

            _searchResults = new List<ProviderNameSearchResult>
            {
                new ProviderNameSearchResult {UkPrn = 12345678, ProviderName = "abc", Aliases = null },
                new ProviderNameSearchResult {UkPrn = 87654321, ProviderName = "ab stuff", Aliases = new List<string> {"ab1", "ab2" } },
                new ProviderNameSearchResult {UkPrn = 12341234, ProviderName = "Smiths Learning", Aliases = null }
            };

            var providerNameSearchResults = new ProviderNameSearchResultsAndPagination
            {
                ActualPage = _actualPage,
                HasError = false,
                LastPage = _lastPage,
                ResponseCode = _responseCode,
                Results = _searchResults,
                ResultsToTake = _resultsToTake,
                TotalResults = _totalResults
            };


            var providerNameFirstPageSearchResults = new ProviderNameSearchResultsAndPagination
            {
                ActualPage = 1,
                HasError = false,
                LastPage = _lastPage,
                ResponseCode = _responseCode,
                Results = _searchResults,
                ResultsToTake = _resultsToTake,
                TotalResults = _totalResults
            };


            var providerNameDefaultPageSizeSearchResults = new ProviderNameSearchResultsAndPagination
            {
                ActualPage = _actualPage,
                HasError = false,
                LastPage = _lastPage,
                ResponseCode = _responseCode,
                Results = _searchResults,
                ResultsToTake = _defaultPageSize,
                TotalResults = _totalResults
            };

            _mockProviderNameSearchService.Setup(
                    x => x.SearchProviderNameAndAliases(It.IsAny<string>(), It.IsAny<int>(),It.IsAny<int>()))
                .Returns(Task.FromResult(providerNameSearchResults));

            _mockProviderNameSearchService.Setup(
                    x => x.SearchProviderNameAndAliases(It.IsAny<string>(), 1, It.IsAny<int>()))
                .Returns(Task.FromResult(providerNameFirstPageSearchResults));

            _mockProviderNameSearchService.Setup(
                    x => x.SearchProviderNameAndAliases(It.IsAny<string>(), It.IsAny<int>(), _defaultPageSize))
                .Returns(Task.FromResult(providerNameDefaultPageSizeSearchResults));

            _mockPaginationSettings.Setup(x => x.DefaultResultsAmount).Returns(_defaultPageSize);

            _handler = new ProviderNameSearchHandler(
                _mockProviderNameSearchService.Object, _mockPaginationSettings.Object);
        }

        [Test]
        public async Task ShouldReturnSuccessWhenSearchIsSuccessful()
        {
            var message = new ProviderNameSearchQuery { SearchTerm = "test", Page = 1 };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.StatusCode.Should().Be(ProviderNameSearchResponseCodes.Success);
        }

        [Test]
        public async Task ShouldReturnExpectedResultsWhenSearchIsComplete()
        {
            var message = new ProviderNameSearchQuery { SearchTerm = _searchTerm, Page = 1, PageSize = _resultsToTake};

            var response = await _handler.Handle(message, default(CancellationToken));

            response.ActualPage.Should().Be(_actualPage);
            response.HasError.Should().BeFalse();
            response.LastPage.Should().Be(_lastPage);
            response.ResultsToTake.Should().Be(_resultsToTake);
            response.SearchTerm.Should().Be(_searchTerm);
            response.TotalResults.Should().Be(_totalResults);
            response.Results.Should().BeSameAs(_searchResults);
        }

        [Test]
        public async Task ShouldReturnDefaultPageSizeWhenPageSizeZero()
        {
            var message = new ProviderNameSearchQuery { SearchTerm = _searchTerm, PageSize = 0 };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.ActualPage.Should().Be(_actualPage);
            response.HasError.Should().BeFalse();
            response.LastPage.Should().Be(_lastPage);
            response.ResultsToTake.Should().Be(_defaultPageSize);
            response.SearchTerm.Should().Be(_searchTerm);
            response.TotalResults.Should().Be(_totalResults);
            response.Results.Should().BeSameAs(_searchResults);
        }

        [Test]
        public async Task ShouldReturnFirstPageWhenPageZero()
        {
            var message = new ProviderNameSearchQuery { SearchTerm = _searchTerm, Page = 0, PageSize = _resultsToTake};

            var response = await _handler.Handle(message, default(CancellationToken));

            response.ActualPage.Should().Be(1);
            response.HasError.Should().BeFalse();
            response.LastPage.Should().Be(_lastPage);
            response.ResultsToTake.Should().Be(_resultsToTake);
            response.SearchTerm.Should().Be(_searchTerm);
            response.TotalResults.Should().Be(_totalResults);
            response.Results.Should().BeSameAs(_searchResults);
        }
    }
}
