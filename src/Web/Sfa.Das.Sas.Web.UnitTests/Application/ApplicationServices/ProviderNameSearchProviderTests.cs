namespace Sfa.Das.Sas.Web.UnitTests.Application.ApplicationServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Core.Domain.Model;
    using Elasticsearch.Net;
    using FluentAssertions;
    using Moq;
    using Nest;
    using NUnit.Framework;
    using Sas.ApplicationServices.Models;
    using Sas.ApplicationServices.Responses;
    using Sas.ApplicationServices.Services;
    using Sas.Infrastructure.Elasticsearch;
    using Sas.Infrastructure.Mapping;
    using SFA.DAS.NLog.Logger;

    [TestFixture]
    public class ProviderNameSearchProviderTests
    {
        private Mock<ILog> _mockLogger;
        private Mock<IProviderNameSearchMapping> _mockProviderNameSearchMapping;
        private Mock<IPaginationOrientationService> _mockPaginationOrientationService;
        private Mock<IProviderNameSearchProviderQuery> _mockProviderQuery;
        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILog>();
            _mockProviderNameSearchMapping = new Mock<IProviderNameSearchMapping>();
            _mockPaginationOrientationService = new Mock<IPaginationOrientationService>();
            _mockProviderQuery = new Mock<IProviderNameSearchProviderQuery>();
        }

        [Test]
        public void ShouldCallProviderWithSomeResultsFoundAndCorrectLogging()
        {
            const int totalHits = 2;
            const int pageNumber = 1;
            const int take = 20;
            const int lastPage = 2;
            const int skip = 0;
            const string searchTerm = "college{";
            const string formattedTerm = "college";
            const string wildcardTerm = "*college*";
            var searchResponse = new Mock<ISearchResponse<ProviderNameSearchResult>>();
            var apiCall = new Mock<IApiCallDetails>();
            apiCall.SetupGet(x => x.HttpStatusCode).Returns((int)HttpStatusCode.OK);
            searchResponse.SetupGet(x => x.ApiCall).Returns(apiCall.Object);

            var returnResults = new List<ProviderNameSearchResult>
            {
                new ProviderNameSearchResult { UkPrn = 11111111, ProviderName = "xyz college", Aliases = new List<string>{ "abc college" } },
                new ProviderNameSearchResult { UkPrn = 33334444, ProviderName = "Life College" }
            };

            searchResponse.SetupGet(x => x.Documents).Returns(returnResults);

            var paginationOrientationDetails = new PaginationOrientationDetails { CurrentPage = pageNumber, LastPage = lastPage, Skip = skip };
            _mockPaginationOrientationService.Setup(x => x.GeneratePaginationDetails(pageNumber, take, totalHits)).Returns(paginationOrientationDetails);
            _mockProviderQuery.Setup(x => x.GetTotalMatches(It.IsAny<string>())).Returns(totalHits);
            _mockProviderQuery.Setup(x => x.GetResults(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PaginationOrientationDetails>())).Returns(searchResponse.Object);
            _mockProviderNameSearchMapping.Setup(x => x.FilterNonMatchingAliases(It.IsAny<string>(), It.IsAny<IEnumerable<ProviderNameSearchResult>>())).Returns(returnResults);

            var provider = new ProviderNameSearchProvider(
                _mockLogger.Object,
                _mockProviderNameSearchMapping.Object,
                _mockPaginationOrientationService.Object,
                _mockProviderQuery.Object
                );

            var result = provider.SearchByTerm(searchTerm, pageNumber, take);
            var logInfo1Expected = $"Provider Name Search provider formatting query: SearchTerm: [{searchTerm}] formatted to: [{formattedTerm}]";

            _mockLogger.Verify(x => x.Info(logInfo1Expected));

            _mockProviderQuery.Verify(x => x.GetTotalMatches(It.IsAny<string>()), Times.Once);
            var logInfo3Expected = $"Provider Name Search provider total hits retrieved: [{totalHits}]";
            _mockLogger.Verify(x => x.Info(logInfo3Expected));

            _mockProviderQuery.Verify(x => x.GetResults(wildcardTerm, take, paginationOrientationDetails), Times.Once);

            var logDebug1Expected = $"Provider Name Search provider getting results based on pagination details: take: [{take}] skip:[{skip}], current page [{pageNumber}], last page [{lastPage}] ";
            _mockLogger.Verify(x => x.Debug(logDebug1Expected));

            var logDebug3Expected = $"Provider Name Search provider retrieved results mapped to returned format";
            _mockLogger.Verify(x => x.Debug(logDebug3Expected));

            Assert.AreEqual(ProviderNameSearchResponseCodes.Success, result.Result.ResponseCode);
            Assert.AreEqual(pageNumber, result.Result.ActualPage);
            Assert.AreEqual(lastPage, result.Result.LastPage);
            Assert.IsFalse(result.Result.HasError);
            Assert.AreEqual(formattedTerm, result.Result.SearchTerm);
            Assert.AreEqual(2, result.Result.ResultsToTake);
            Assert.AreEqual(2, result.Result.Results.Count());

            result.Result.Results.ShouldBeEquivalentTo(returnResults);
        }

        [Test]
        public void ShouldCallProviderWithNoResultsFoundAndCorrectLogging()
        {
            const int totalHits = 0;
            const int pageNumber = 1;
            const int take = 20;
            const int lastPage = 2;
            const int skip = 0;
            const string searchTerm = "college{";
            const string formattedTerm = "college";
            const string wildcardTerm = "*college*";
            var searchResponse = new Mock<ISearchResponse<ProviderNameSearchResult>>();
            var apiCall = new Mock<IApiCallDetails>();
            apiCall.SetupGet(x => x.HttpStatusCode).Returns((int)HttpStatusCode.OK);
            searchResponse.SetupGet(x => x.ApiCall).Returns(apiCall.Object);
            searchResponse.SetupGet(x => x.Documents).Returns(new List<ProviderNameSearchResult>());

            var paginationOrientationDetails = new PaginationOrientationDetails { CurrentPage = pageNumber, LastPage = lastPage, Skip = skip };
            _mockPaginationOrientationService.Setup(x => x.GeneratePaginationDetails(pageNumber, take, totalHits)).Returns(paginationOrientationDetails);
            _mockProviderQuery.Setup(x => x.GetTotalMatches(It.IsAny<string>())).Returns(totalHits);
            _mockProviderQuery.Setup(x => x.GetResults(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PaginationOrientationDetails>())).Returns(searchResponse.Object);

            var provider = new ProviderNameSearchProvider(
                _mockLogger.Object,
                _mockProviderNameSearchMapping.Object,
                _mockPaginationOrientationService.Object,
                _mockProviderQuery.Object
                );

            var result = provider.SearchByTerm(searchTerm, pageNumber, take);
            var logInfo1Expected = $"Provider Name Search provider formatting query: SearchTerm: [{searchTerm}] formatted to: [{formattedTerm}]";

            _mockLogger.Verify(x => x.Info(logInfo1Expected));

            _mockProviderQuery.Verify(x => x.GetTotalMatches(It.IsAny<string>()), Times.Once);
            var logInfo3Expected = $"Provider Name Search provider total hits retrieved: [{totalHits}]";
            _mockLogger.Verify(x => x.Info(logInfo3Expected));

            _mockProviderQuery.Verify(x => x.GetResults(wildcardTerm, take, paginationOrientationDetails), Times.Once);

            var logDebug1Expected = $"Provider Name Search provider getting results based on pagination details: take: [{take}] skip:[{skip}], current page [{pageNumber}], last page [{lastPage}] ";
            _mockLogger.Verify(x => x.Debug(logDebug1Expected));

            var logDebug3Expected = $"Provider Name Search provider retrieved results mapped to returned format";
            _mockLogger.Verify(x => x.Debug(logDebug3Expected));

            Assert.AreEqual(ProviderNameSearchResponseCodes.NoSearchResultsFound, result.Result.ResponseCode);
            Assert.AreEqual(pageNumber, result.Result.ActualPage);
            Assert.AreEqual(lastPage, result.Result.LastPage);
            Assert.IsFalse(result.Result.HasError);
            Assert.AreEqual(formattedTerm, result.Result.SearchTerm);
            Assert.AreEqual(0, result.Result.ResultsToTake);
            Assert.AreEqual(0, result.Result.Results.Count());
        }

        [Test]
        public void ShouldCallProviderWithShortSearchTerm()
        {
            const int pageNumber = 1;
            const int take = 20;
            const string searchTerm = "co";
            const string formattedTerm = "co";

            var provider = new ProviderNameSearchProvider(
                _mockLogger.Object,
                _mockProviderNameSearchMapping.Object,
                _mockPaginationOrientationService.Object,
                _mockProviderQuery.Object
                );

            var result = provider.SearchByTerm(searchTerm, pageNumber, take);
            var logInfo1Expected = $"Formatted search term causing SearchTermTooShort: [{formattedTerm}]";

            _mockLogger.Verify(x => x.Info(logInfo1Expected));

            _mockProviderQuery.Verify(x => x.GetTotalMatches(It.IsAny<string>()), Times.Never);

            _mockProviderQuery.Verify(x => x.GetResults(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PaginationOrientationDetails>()), Times.Never);

            _mockLogger.Verify(x => x.Debug(It.IsAny<string>()), Times.Never);

            var tooShortDetails = new ProviderNameSearchResultsAndPagination
            {
                ActualPage = 1,
                LastPage = 1,
                HasError = false,
                SearchTerm = formattedTerm,
                ResponseCode = ProviderNameSearchResponseCodes.SearchTermTooShort
            };

            result.Result.ShouldBeEquivalentTo(tooShortDetails);
        }
    }
}