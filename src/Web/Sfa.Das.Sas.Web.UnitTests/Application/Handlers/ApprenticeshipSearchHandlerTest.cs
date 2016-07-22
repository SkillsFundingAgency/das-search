using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public sealed class ApprenticeshipSearchHandlerTest
    {
        private ApprenticeshipSearchHandler _sut;
        private Mock<IApprenticeshipSearchService> _mockApprenticeshipSearchService;
        private Mock<IShortlistCollection<int>> _mockShortlistCollection;

        [SetUp]
        public void Init()
        {
            _mockApprenticeshipSearchService = new Mock<IApprenticeshipSearchService>();
            _mockShortlistCollection = new Mock<IShortlistCollection<int>>();

            _mockApprenticeshipSearchService.Setup(x => x.SearchByKeyword(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(new ApprenticeshipSearchResults
                {
                    Results = new List<ApprenticeshipSearchResultsItem>(),
                    TotalResults = 20
                });

            _sut = new ApprenticeshipSearchHandler(_mockApprenticeshipSearchService.Object, _mockShortlistCollection.Object);
        }

        [Test]
        public void ShouldReturnSearchPageLimitExceededIfPageIsGreaterThanLastPage()
        {
            _mockApprenticeshipSearchService.Setup(x => x.SearchByKeyword(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(new ApprenticeshipSearchResults
                {
                    LastPage = 3,
                    TotalResults = 10,
                    Results = new List<ApprenticeshipSearchResultsItem>()
                });

            var response = _sut.Handle(new ApprenticeshipSearchQuery());

            response.StatusCode.Should().Be(ApprenticeshipSearchResponse.ResponseCodes.PageNumberOutOfUpperBound);
        }

        [Test]
        public void ShouldReturnSearchValues()
        {
            var searchResults = new ApprenticeshipSearchResults
            {
                LastPage = 3,
                TotalResults = 10,
                Results = new List<ApprenticeshipSearchResultsItem> { new ApprenticeshipSearchResultsItem() },
                ActualPage = 2,
                LevelAggregation = new Dictionary<int, long?> { { 1, 2 } },
                HasError = false,
                ResultsToTake = 4,
                SortOrder = "5",
                SearchTerm = "test term",
                SelectedLevels = new[] { 1, 2 }
            };

            _mockApprenticeshipSearchService.Setup(x => x.SearchByKeyword(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(searchResults);

            var query = new ApprenticeshipSearchQuery
            {
                Keywords = "Test term",
                Order = 2
            };

            var response = _sut.Handle(query);

            response.LastPage.Should().Be(searchResults.LastPage);
            response.TotalResults.Should().Be(searchResults.TotalResults);
            response.Results.Should().BeEquivalentTo(searchResults.Results);
            response.ActualPage.Should().Be(query.Page);
            response.AggregationLevel.ShouldBeEquivalentTo(searchResults.LevelAggregation);
            response.HasError.Should().Be(searchResults.HasError);
            response.ResultsToTake.Should().Be(searchResults.ResultsToTake);
            response.SelectedLevels.ShouldBeEquivalentTo(searchResults.SelectedLevels);

            response.SearchTerm.Should().Be(query.Keywords);
            response.SortOrder.Should().Be(query.Order.ToString());
        }

        [Test]
        public void ShouldReturnDefaultShortListOrderIfOrderIsZero()
        {
            _mockApprenticeshipSearchService.Setup(x => x.SearchByKeyword(
               It.IsAny<string>(), 1, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
               .Returns(new ApprenticeshipSearchResults
               {
                   Results = new List<ApprenticeshipSearchResultsItem>
                   {
                       new ApprenticeshipSearchResultsItem()
                   },
                   TotalResults = 20
               });

            var response = _sut.Handle(new ApprenticeshipSearchQuery
            {
                Order = 0
            });

            response.SortOrder.Should().Be("1");
        }

        [Test]
        public void ShouldEnforceMinimumPageToBeOne()
        {
            _mockApprenticeshipSearchService.Setup(x => x.SearchByKeyword(
                It.IsAny<string>(), 1, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(new ApprenticeshipSearchResults
                {
                    Results = new List<ApprenticeshipSearchResultsItem>(),
                    TotalResults = 20
                });

            var response = _sut.Handle(new ApprenticeshipSearchQuery { Page = 0 });

            _mockApprenticeshipSearchService.Verify(x => x.SearchByKeyword(
                It.IsAny<string>(), 1, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()));
        }
    }
}
