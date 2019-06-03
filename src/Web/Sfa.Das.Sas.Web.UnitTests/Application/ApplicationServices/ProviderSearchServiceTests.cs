using System.Collections.Generic;
using System.Linq;
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
            var service = new ProviderSearchService(null, null, null, null, null, null, null);

            var result = await service.SearchStandardProviders("123", postcode, _pageZeroWithTenItems, null, false, false, false);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByStandardPostCodeShouldAlwaysReturnTheStandardId(string postcode)
        {
            const string TestStandardId = "123";
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };
            ProviderSearchService service = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
                .SetupStandardRepository(x => x.GetStandardById(It.IsAny<string>()), new Standard { Level = 2 });

            var result = await service.SearchStandardProviders(TestStandardId, postcode, _pageZeroWithTenItems, null, false, false, false);

            Assert.That(result.StandardId, Is.EqualTo(TestStandardId));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const string TestStandardId = "123";
            const string testPostCode = "AS3 4AA";
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            var builder = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
                .SetupStandardRepository(x => x.GetStandardById(It.IsAny<string>()), new Standard { Level = 2 });

            var service = builder.Build();

            var result = await service.SearchStandardProviders(TestStandardId, testPostCode, _pageZeroWithTenItems, null, false, false, false);

            Assert.That(result.Hits, Is.EqualTo(stubSearchResults));

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldReturnZeroResultsIfPostCodeIsIncorrect()
        {
            const string TestStandardId = "123";
            const string TestPostCode = "AS3 4AA";
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            var builder = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
                .SetupStandardRepository(x => x.GetStandardById(It.IsAny<string>()), new Standard { Level = 2 });

            var service = builder.Build();

            var result = await service.SearchStandardProviders(TestStandardId, TestPostCode, _pageZeroWithTenItems, null, false, false, false);

            result.TotalResults.Should().Be(0);
            result.StandardId.Should().Be("123");

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(TestPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = null, Total = 5 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupStandardRepository(x => x.GetStandardById(It.IsAny<string>()), new Standard { Level = 2 });

            var result = await service.SearchStandardProviders("123", "AS2 3SS", _pageZeroWithTenItems, null, false, false, false);

            Assert.That(result.TotalResults, Is.EqualTo(testTotalResults));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIncludeStandardName()
        {
            const string testTitle = "Test Title";
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
               .SetupLocationSearchProvider(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupStandardRepository(x => x.GetStandardById(It.IsAny<string>()), new Standard { Title = testTitle });

            var result = await service.SearchStandardProviders("123", "AS3 4AS", _pageZeroWithTenItems, null, false, false, false);

            Assert.That(result.StandardName, Is.EqualTo(testTitle));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIndicateThereWasAnErrorIfSearchThrowsAnException()
        {
            ProviderSearchService service = new ProviderSearchServiceBuilder()
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupLocationSearchProviderException<SearchException>(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()));

            var result = await service.SearchStandardProviders("-1", "AS3 4AS", _pageZeroWithTenItems, null, false, false, false);

            result.StandardResponseCode.Should().Be(ServerLookupResponse.InternalServerError);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task SearchByFrameworkPostCodeShouldIndicateIfANullOrEmptyPostCodeIsPassed(string postcode)
        {
            var service = new ProviderSearchServiceBuilder().Build();

            var result = await service.SearchFrameworkProviders("123", postcode, _pageZeroWithTenItems, null, false, false, false);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByFrameworkPostCodeShouldAlwaysReturnTheFrameworkId(string postcode)
        {
            const string TestFrameworkId = "123";
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var result = await service.SearchFrameworkProviders(TestFrameworkId, postcode, _pageZeroWithTenItems, null, false, false, false);

            result.FrameworkId.Should().Be(TestFrameworkId);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const string TestFrameworkId = "123";
            const string TestPostCode = "AS3 4AA";
            var stubSearchResults = new List<FrameworkProviderSearchResultsItem> { new FrameworkProviderSearchResultsItem(), new FrameworkProviderSearchResultsItem() };
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            var framework = new Framework
            {
                FrameworkId = "123",
                FrameworkName = "Test framework name"
            };

            ProviderSearchServiceBuilder builder = new ProviderSearchServiceBuilder()
               .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<string>()), framework)
               .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
               .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var service = builder.Build();

            var result = await service.SearchFrameworkProviders(TestFrameworkId, TestPostCode, _pageZeroWithTenItems, null, false, false, false);

            result.Hits.Should().BeSameAs(stubSearchResults);

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(TestPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldReturnZeroResultsIfPostCodeIsIncorrect()
        {
            const string TestFrameworkId = "123";
            const string TestPostCode = "AS3 4AA";
            var stubSearchResults = new List<FrameworkProviderSearchResultsItem> { new FrameworkProviderSearchResultsItem(), new FrameworkProviderSearchResultsItem() };
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = stubSearchResults, Total = 0 };

            ProviderSearchServiceBuilder builder = new ProviderSearchServiceBuilder()
                    .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                    .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var service = builder.Build();
            var result = await service.SearchFrameworkProviders(TestFrameworkId, TestPostCode, _pageZeroWithTenItems, null, false, false, false);

            result.TotalResults.Should().Be(0);
            result.FrameworkId.Should().Be("123");

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(TestPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var framework = new Framework
            {
                FrameworkId = "123",
                FrameworkName = "Test framework name"
            };
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = null, Total = testTotalResults };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                    .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<string>()), framework)
                    .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                    .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults);

            var result = await service.SearchFrameworkProviders("123", "AS2 3SS", _pageZeroWithTenItems, null, false, false, false);

            result.TotalResults.Should().Be(testTotalResults);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIncludeStandardName()
        {
            const string FrameworkName = "Test Title";
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                   .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                   .SetupLocationSearchProvider(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), searchResults)
                   .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<string>()), new Framework { FrameworkName = FrameworkName });

            var result = await service.SearchFrameworkProviders("123", "AS3 4AS", _pageZeroWithTenItems, null, false, false, false);

            result.FrameworkName.Should().Be(FrameworkName);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIndicateThereWasAnErrorIfSearchThrowsAnException()
        {
            ProviderSearchService service = new ProviderSearchServiceBuilder()
                    .SetupFrameworkRepository(m => m.GetFrameworkById(It.IsAny<string>()), new Framework())
                   .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                   .SetupLocationSearchProviderException<SearchException>(x => x.
                        SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()));

            var result = await service.SearchFrameworkProviders("-1", "AS3 4AS", _pageZeroWithTenItems, null, false, false, false);

            result.FrameworkResponseCode.Should().Be(ServerLookupResponse.InternalServerError);
        }



        [TestCase("")]
        [TestCase(null)]
        public async Task SearchByPostCodeShouldIndicateIfANullOrEmptyPostCodeIsPassed(string postcode)
        {
            var service = new ProviderSearchServiceBuilder().Build();

            var result = await service.SearchProviders("123-2-1", postcode, _pageZeroWithTenItems, null, false);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByPostCodeShouldAlwaysReturnTheFrameworkId(string postcode)
        {
            const string TestApprenticeshipId = "123-2-1";
            var searchResults = new SearchResult<ProviderSearchResultItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupProviderSearchProvider(x => x.SearchProvidersByLocation(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), Task.FromResult(searchResults));

            var result = await service.SearchProviders(TestApprenticeshipId, postcode, _pageZeroWithTenItems, null, false);

            result.ApprenticeshipId.Should().Be(TestApprenticeshipId);
        }

        [Test]
        public async Task SearchByPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const string TestApprenticeshipId = "123-2-1";
            const string TestPostCode = "AS3 4AA";
            var stubSearchResults = (new List<ProviderSearchResultItem> { new ProviderSearchResultItem(), new ProviderSearchResultItem() }).AsEnumerable();
            var searchResults = new SearchResult<ProviderSearchResultItem> { Hits = stubSearchResults, Total = 0 };

            var framework = new Framework
            {
                FrameworkId = "123",
                FrameworkName = "Test framework name"
            };

            ProviderSearchServiceBuilder builder = new ProviderSearchServiceBuilder()
                .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<string>()), framework)
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupProviderSearchProvider(x => x.SearchProvidersByLocation(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), Task.FromResult(searchResults));
            var service = builder.Build();

            var result = await service.SearchProviders(TestApprenticeshipId, TestPostCode, _pageZeroWithTenItems, null, false);

            result.Hits.Should().BeSameAs(searchResults.Hits);

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(TestPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByPostCodeShouldReturnZeroResultsIfPostCodeIsIncorrect()
        {
            const string TestApprenticeshipId = "123-2-1";
            const string TestPostCode = "AS3 4AA";
            var stubSearchResults = new List<ProviderSearchResultItem> { new ProviderSearchResultItem(), new ProviderSearchResultItem() };
            var searchResults = new SearchResult<ProviderSearchResultItem> { Hits = stubSearchResults, Total = 0 };

            var framework = new Framework
            {
                FrameworkId = TestApprenticeshipId,
                FrameworkName = "Test framework name"
            };

            ProviderSearchServiceBuilder builder = new ProviderSearchServiceBuilder()
                .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<string>()), framework)

                    .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                    .SetupProviderSearchProvider(x => x.SearchProvidersByLocation(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), Task.FromResult(searchResults));


            var service = builder.Build();
            var result = await service.SearchProviders(TestApprenticeshipId, TestPostCode, _pageZeroWithTenItems, null, false);

            result.TotalResults.Should().Be(0);
            result.ApprenticeshipId.Should().Be(TestApprenticeshipId);

            builder.LocationLookup.Verify(x => x.GetLatLongFromPostCode(TestPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var frameworkId = "123-1-2";
            var framework = new Framework
            {
                FrameworkId = "123-1-2",
                FrameworkName = "Test framework name"
            };
            var searchResults = new SearchResult<ProviderSearchResultItem> { Hits = null, Total = testTotalResults };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                    .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<string>()), framework)
                    .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                    .SetupProviderSearchProvider(x => x.SearchProvidersByLocation(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), Task.FromResult(searchResults));


            var result = await service.SearchProviders(frameworkId, "AS2 3SS", _pageZeroWithTenItems, null, false);

            result.TotalResults.Should().Be(testTotalResults);
        }

        [Test]
        public async Task SearchByPostCodeShouldIncludeStandardTitle()
        {
            const string TestApprenticeshipTitle = "Test Title";
            var searchResults = new SearchResult<ProviderSearchResultItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                   .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                   .SetupStandardRepository(x => x.GetStandardById(It.IsAny<string>()), new Standard() { Title = TestApprenticeshipTitle })
                .SetupProviderSearchProvider(x => x.SearchProvidersByLocation(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), Task.FromResult(searchResults));


            var result = await service.SearchProviders("123", "AS3 4AS", _pageZeroWithTenItems, null, false);

            result.Title.Should().Be(TestApprenticeshipTitle);
        }

        [Test]
        public async Task SearchByPostCodeShouldIncludeFrameworkTitle()
        {
            const string TestApprenticeshipTitle = "Test Title";
            var searchResults = new SearchResult<ProviderSearchResultItem> { Hits = null, Total = 0 };

            ProviderSearchService service = new ProviderSearchServiceBuilder()
                .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                .SetupFrameworkRepository(x => x.GetFrameworkById(It.IsAny<string>()), new Framework() { Title = TestApprenticeshipTitle })
                .SetupProviderSearchProvider(x => x.SearchProvidersByLocation(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()), Task.FromResult(searchResults));


            var result = await service.SearchProviders("123-2-1", "AS3 4AS", _pageZeroWithTenItems, null, false);

            result.Title.Should().Be(TestApprenticeshipTitle);
        }

        [Test]
        public async Task SearchByPostCodeShouldIndicateThereWasAnErrorIfSearchThrowsAnException()
        {
            ProviderSearchService service = new ProviderSearchServiceBuilder()
                    .SetupFrameworkRepository(m => m.GetFrameworkById(It.IsAny<string>()), new Framework())
                   .SetupPostCodeLookup(x => x.GetLatLongFromPostCode(It.IsAny<string>()), Task.FromResult(_testPostCodeCoordinate))
                   .SetupProviderSearchProviderException<SearchException>(x => x.
                        SearchProvidersByLocation(It.IsAny<string>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProviderSearchFilter>()));

            var result = await service.SearchProviders("-1", "AS3 4AS", _pageZeroWithTenItems, null, false);

            result.ResponseCode.Should().Be(LocationLookupResponse.ApprenticeshipNotFound);
        }
    }
}