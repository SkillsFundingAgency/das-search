using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Exceptions;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.UnitTests.Application.ApplicationServices
{
    [TestFixture]
    public sealed class ProviderSearchServiceTests
    {
        private readonly Pagination _pageZeroWithTenItems = new Pagination { Page = 0, Take = 10 };
        private readonly CoordinateResponse _testPostCodeCoordinate = new CoordinateResponse { Coordinate = new Coordinate { Lat = 52.1234, Lon = 1.3445 }, ResponseCode = LocationLookupResponse.Ok };

        [TestCase("")]
        [TestCase(null)]
        public async Task SearchByStandardPostCodeShouldIndicateIfANullOrEmptyPostCodeIsPassed(string postcode)
        {
            var service = new ProviderSearchService(null, null, null, null, null, null);

            var result = await service.SearchStandardProviders(123, postcode, _pageZeroWithTenItems, null, false, false);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByStandardPostCodeShouldAlwaysReturnTheStandardId(string postcode)
        {
            const int testStandardId = 123;
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };
            ProviderSearchService service = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var result = await service.SearchStandardProviders(testStandardId, postcode, _pageZeroWithTenItems, null, false, false);

            Assert.That(result.StandardId, Is.EqualTo(testStandardId));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const int testStandardId = 123;
            const string testPostCode = "AS3 4AA";
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            var builder = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var service = builder.Build();

            var result = await service.SearchStandardProviders(testStandardId, testPostCode, _pageZeroWithTenItems, null, false, false);

            Assert.That(result.Hits, Is.EqualTo(stubSearchResults));

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldReturnZeroResultsIfPostCodeIsIncorrect()
        {
            const int testStandardId = 123;
            const string testPostCode = "AS3 4AA";
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            var builder = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var service = builder.Build();

            var result = await service.SearchStandardProviders(testStandardId, testPostCode, _pageZeroWithTenItems, null, false, false);

            result.TotalResults.Should().Be(0);
            result.StandardId.Should().Be(123);

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = null, Total = 5 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate));

            var result = await service.SearchStandardProviders(123, "AS2 3SS", _pageZeroWithTenItems, null, false, false);

            Assert.That(result.TotalResults, Is.EqualTo(testTotalResults));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIncludeStandardName()
        {
            const string testTitle = "Test Title";
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
               .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupStandardRepository(x => x.GetStandardById(It.IsAny<int>()), new Standard { Title = testTitle });

            var result = await service.SearchStandardProviders(123, "AS3 4AS", _pageZeroWithTenItems, null, false, false);

            Assert.That(result.StandardName, Is.EqualTo(testTitle));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIndicateThereWasAnErrorIfSearchThrowsAnException()
        {
            ProviderSearchService service = new ProviderSearchServiceBuilder()
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupLocationSearchProviderException<SearchException>(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()));

            var result = await service.SearchStandardProviders(-1, "AS3 4AS", _pageZeroWithTenItems, null, false, false);

            result.StandardResponseCode.Should().Be(ServerLookupResponse.InternalServerError);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task SearchByFrameworkPostCodeShouldIndicateIfANullOrEmptyPostCodeIsPassed(string postcode)
        {
            var service = new ProviderSearchServiceBuilder().Build();

            var result = await service.SearchFrameworkProviders(123, postcode, _pageZeroWithTenItems, null, false, false);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByFrameworkPostCodeShouldAlwaysReturnTheFrameworkId(string postcode)
        {
            const int testFrameworkId = 123;
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var result = await service.SearchFrameworkProviders(testFrameworkId, postcode, _pageZeroWithTenItems, null, false, false);

            result.FrameworkId.Should().Be(testFrameworkId);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const int testFrameworkId = 123;
            const string testPostCode = "AS3 4AA";
            var stubSearchResults = new List<FrameworkProviderSearchResultsItem> { new FrameworkProviderSearchResultsItem(), new FrameworkProviderSearchResultsItem() };
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            var framework = new Framework
            {
                FrameworkId = 123,
                FrameworkName = "Test framework name"
            };

            ProviderSearchServiceBuilder builder = new ProviderSearchServiceBuilder()
               .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<int>()), framework)
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var service = builder.Build();

            var result = await service.SearchFrameworkProviders(testFrameworkId, testPostCode, _pageZeroWithTenItems, null, false, false);

            result.Hits.Should().BeSameAs(stubSearchResults);

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldReturnZeroResultsIfPostCodeIsIncorrect()
        {
            const int testFrameworkId = 123;
            const string testPostCode = "AS3 4AA";
            var stubSearchResults = new List<FrameworkProviderSearchResultsItem> { new FrameworkProviderSearchResultsItem(), new FrameworkProviderSearchResultsItem() };
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            ProviderSearchServiceBuilder builder = new ProviderSearchServiceBuilder()
                    .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                    .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var service = builder.Build();
            var result = await service.SearchFrameworkProviders(testFrameworkId, testPostCode, _pageZeroWithTenItems, null, false, false);

            result.TotalResults.Should().Be(0);
            result.FrameworkId.Should().Be(123);

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var framework = new Framework
            {
                FrameworkId = 123,
                FrameworkName = "Test framework name"
            };
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = null, Total = testTotalResults };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                    .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<int>()), framework)
                    .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                    .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var result = await service.SearchFrameworkProviders(123, "AS2 3SS", _pageZeroWithTenItems, null, false, false);

            result.TotalResults.Should().Be(testTotalResults);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIncludeStandardName()
        {
            const string frameworkName = "Test Title";
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                   .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                   .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
                   .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<int>()), new Framework { FrameworkName = frameworkName });

            var result = await service.SearchFrameworkProviders(123, "AS3 4AS", _pageZeroWithTenItems, null, false, false);

            result.FrameworkName.Should().Be(frameworkName);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIndicateThereWasAnErrorIfSearchThrowsAnException()
        {
            ProviderSearchService service = new ProviderSearchServiceBuilder()
                   .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                   .SetupLocationSearchProviderException<SearchException>(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()));

            var result = await service.SearchFrameworkProviders(-1, "AS3 4AS", _pageZeroWithTenItems, null, false, false);

            result.FrameworkResponseCode.Should().Be(ServerLookupResponse.InternalServerError);
        }
    }
}