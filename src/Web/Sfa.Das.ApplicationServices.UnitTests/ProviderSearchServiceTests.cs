using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.ApplicationServices;

namespace Sfa.Das.ApplicationServices.Tests
{
    [TestFixture]
    public class ProviderSearchServiceTests
    {
        [TestCase("")]
        [TestCase(null)]
        public async Task SearchByPostCodeShouldIdicateIfANullOrEmptyPostCodeIsPassed(string postcode)
        {
            var service = new ProviderSearchService(null, null, null);

            var result = await service.SearchByPostCode(123, postcode);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByPostCodeShouldAlwaysReturnTheStandardId(string postcode)
        {
            const int testStandardId = 123;
            var mockSearchProvider = CreateMockSearchProvider();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockPostCodeLookup.Object);

            var result = await service.SearchByPostCode(testStandardId, postcode);

            Assert.That(result.StandardId, Is.EqualTo(testStandardId));
        }

        [Test]
        public async Task SearchByPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const int testStandardId = 123;
            const string testPostCode = "AS3 4AA";
            var testCoordinates = new Coordinate { Lat = 52.1234, Lon = 1.3445 };
            var stubSearchResults = new List<ProviderSearchResultsItem> { new ProviderSearchResultsItem(), new ProviderSearchResultsItem() };
            var mockSearchProvider = CreateMockSearchProvider(stubSearchResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            mockPostCodeLookup.Setup(x => x.GetLatLongFromPostCode(It.IsAny<string>())).Returns(Task.FromResult(testCoordinates));

            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockPostCodeLookup.Object);

            var result = await service.SearchByPostCode(testStandardId, testPostCode);

            Assert.That(result.Hits, Is.EqualTo(stubSearchResults));

            mockPostCodeLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
            mockSearchProvider.Verify(x => x.SearchByLocation(testStandardId, testCoordinates), Times.Once);
        }

        [Test]
        public async Task SearchByPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var mockSearchProvider = CreateMockSearchProvider(null, testTotalResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();

            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockPostCodeLookup.Object);

            var result = await service.SearchByPostCode(123, "AS2 3SS");

            Assert.That(result.TotalResults, Is.EqualTo(testTotalResults));
        }

        [Test]
        public async Task SearchByPostCodeShouldIncludeStandardName()
        {
            var mockSearchProvider = CreateMockSearchProvider();
            const string testTitle = "Test Title";

            var mockStandardRepository = new Mock<IStandardRepository>();
            mockStandardRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Standard { Title = testTitle });

            var mockPostCodeLookup = CreateMockPostCodeLookup();

            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockPostCodeLookup.Object);

            var result = await service.SearchByPostCode(123, "AS3 4AS");

            Assert.That(result.StandardName, Is.EqualTo(testTitle));
        }

        private static Mock<ISearchProvider> CreateMockSearchProvider(List<ProviderSearchResultsItem> stubSearchResults, long totalHits = 0)
        {
            var searchResults = new SearchResult<ProviderSearchResultsItem> { Hits = stubSearchResults, Total = totalHits };

            var mockSearchProvider = new Mock<ISearchProvider>();
            mockSearchProvider.Setup(x => x.SearchByLocation(It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(searchResults);

            return mockSearchProvider;
        }

        private static Mock<ISearchProvider> CreateMockSearchProvider()
        {
            return CreateMockSearchProvider(null);
        }

        private static Mock<IStandardRepository> CreateMockStandardRepository()
        {
            var mockStandardRepository = new Mock<IStandardRepository>();

            return mockStandardRepository;
        }

        private static Mock<ILookupLocations> CreateMockPostCodeLookup()
        {
            var testCoordinates = new Coordinate { Lat = 52.1234, Lon = 1.3445 };

            var mockPostCodeLookup = new Mock<ILookupLocations>();
            mockPostCodeLookup.Setup(x => x.GetLatLongFromPostCode(It.IsAny<string>())).Returns(Task.FromResult(testCoordinates));

            return mockPostCodeLookup;
        }
    }
}