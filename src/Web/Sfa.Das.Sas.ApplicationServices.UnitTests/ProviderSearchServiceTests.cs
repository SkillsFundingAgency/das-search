using Sfa.Das.Sas.ApplicationServices.Settings;

namespace Sfa.Das.Sas.ApplicationServices.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Domain.Model;
    using Core.Domain.Services;
    using Core.Logging;
    using Exceptions;
    using FluentAssertions;
    using Models;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ProviderSearchServiceTests
    {
        [TestCase("")]
        [TestCase(null)]
        public async Task SearchByStandardPostCodeShouldIndicateIfANullOrEmptyPostCodeIsPassed(string postcode)
        {
            var service = new ProviderSearchService(null, null, null, null, null, null);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchStandardProviders(123, postcode, pagination, null, false);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByStandardPostCodeShouldAlwaysReturnTheStandardId(string postcode)
        {
            const int testStandardId = 123;
            var mockSearchProvider = CreateMockStandardSearchProvider();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchStandardProviders(testStandardId, postcode, pagination, null, false);

            Assert.That(result.StandardId, Is.EqualTo(testStandardId));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const int testStandardId = 123;
            const string testPostCode = "AS3 4AA";
            var testCoordinates = new Coordinate { Lat = 52.1234, Lon = 1.3445 };
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var mockSearchProvider = CreateMockStandardSearchProvider(stubSearchResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            mockPostCodeLookup.Setup(x => x.GetLatLongFromPostCode(It.IsAny<string>())).Returns(Task.FromResult(testCoordinates));

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchStandardProviders(testStandardId, testPostCode, pagination, null, false);

            Assert.That(result.Hits, Is.EqualTo(stubSearchResults));

            mockPostCodeLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
            mockSearchProvider.Verify(x => x.SearchByStandardLocation(testStandardId, testCoordinates, 0, 10, null), Times.Once);
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldReturnZeroResultsIfPostCodeIsIncorrect()
        {
            const int testStandardId = 123;
            const string testPostCode = "AS3 4AA";
            var stubSearchResults = new List<StandardProviderSearchResultsItem> { new StandardProviderSearchResultsItem(), new StandardProviderSearchResultsItem() };
            var mockSearchProvider = CreateMockStandardSearchProvider(stubSearchResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            mockPostCodeLookup.Setup(x => x.GetLatLongFromPostCode(It.IsAny<string>())).Returns(Task.FromResult((Coordinate)null));

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchStandardProviders(testStandardId, testPostCode, pagination, null, false);

            result.TotalResults.Should().Be(0);
            result.StandardId.Should().Be(123);

            mockPostCodeLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var mockSearchProvider = CreateMockStandardSearchProvider(null, testTotalResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchStandardProviders(123, "AS2 3SS", pagination, null, false);

            Assert.That(result.TotalResults, Is.EqualTo(testTotalResults));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIncludeStandardName()
        {
            const string testTitle = "Test Title";
            var mockSearchProvider = CreateMockStandardSearchProvider();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            mockStandardRepository.Setup(x => x.GetStandardById(It.IsAny<int>())).Returns(new Standard { Title = testTitle });
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchStandardProviders(123, "AS3 4AS", pagination, null, false);

            Assert.That(result.StandardName, Is.EqualTo(testTitle));
        }

        [Test]
        public async Task SearchByStandardPostCodeShouldIndicateThereWasAnErrorIfSearchThrowsAnException()
        {
            var mockSearchProvider = CreateMockStandardSearchProvider();
            mockSearchProvider.Setup(x => x.SearchByStandardLocation(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).Throws<SearchException>();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchStandardProviders(123, "AS3 4AS", pagination, null, false);

            Assert.That(result.HasError, Is.True);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task SearchByFrameworkPostCodeShouldIndicateIfANullOrEmptyPostCodeIsPassed(string postcode)
        {
            var service = new ProviderSearchService(null, null, null, null, null, null);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchFrameworkProviders(123, postcode, pagination, null, false);

            Assert.That(result.PostCodeMissing, Is.True);
        }

        [TestCase("")]
        [TestCase("ABC 123")]
        public async Task SearchByFrameworkPostCodeShouldAlwaysReturnTheFrameworkId(string postcode)
        {
            const int testFrameworkId = 123;
            var mockSearchProvider = CreateMockFrameworkSearchProvider();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchFrameworkProviders(testFrameworkId, postcode, pagination, null, false);

            result.FrameworkId.Should().Be(testFrameworkId);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldSearchForProviderByLatLongAndStandardId()
        {
            const int testFrameworkId = 123;
            const string testPostCode = "AS3 4AA";
            var testCoordinates = new Coordinate { Lat = 52.1234, Lon = 1.3445 };
            var stubSearchResults = new List<FrameworkProviderSearchResultsItem> { new FrameworkProviderSearchResultsItem(), new FrameworkProviderSearchResultsItem() };
            var mockSearchProvider = CreateMockFrameworkSearchProvider(stubSearchResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            mockPostCodeLookup.Setup(x => x.GetLatLongFromPostCode(It.IsAny<string>())).Returns(Task.FromResult(testCoordinates));

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchFrameworkProviders(testFrameworkId, testPostCode, pagination, null, false);

            result.Hits.Should().BeSameAs(stubSearchResults);

            mockPostCodeLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
            mockSearchProvider.Verify(x => x.SearchByFrameworkLocation(testFrameworkId, testCoordinates, 0, 10, null), Times.Once);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldReturnZeroResultsIfPostCodeIsIncorrect()
        {
            const int testFrameworkId = 123;
            const string testPostCode = "AS3 4AA";
            var stubSearchResults = new List<FrameworkProviderSearchResultsItem> { new FrameworkProviderSearchResultsItem(), new FrameworkProviderSearchResultsItem() };
            var mockSearchProvider = CreateMockFrameworkSearchProvider(stubSearchResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            mockPostCodeLookup.Setup(x => x.GetLatLongFromPostCode(It.IsAny<string>())).Returns(Task.FromResult((Coordinate)null));

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchFrameworkProviders(testFrameworkId, testPostCode, pagination, null, false);

            result.TotalResults.Should().Be(0);
            result.FrameworkId.Should().Be(123);

            mockPostCodeLookup.Verify(x => x.GetLatLongFromPostCode(testPostCode), Times.Once);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIncludeCountOfResults()
        {
            const long testTotalResults = 5;
            var mockSearchProvider = CreateMockFrameworkSearchProvider(null, testTotalResults);
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchFrameworkProviders(123, "AS2 3SS", pagination, null, false);

            result.TotalResults.Should().Be(testTotalResults);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIncludeStandardName()
        {
            const string frameworkName = "Test Title";
            var mockSearchProvider = CreateMockFrameworkSearchProvider();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            mockFrameworkRepository.Setup(x => x.GetFrameworkById(It.IsAny<int>())).Returns(new Framework { FrameworkName = frameworkName });
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchFrameworkProviders(123, "AS3 4AS", pagination, null, false);

            result.FrameworkName.Should().Be(frameworkName);
        }

        [Test]
        public async Task SearchByFrameworkPostCodeShouldIndicateThereWasAnErrorIfSearchThrowsAnException()
        {
            var mockSearchProvider = CreateMockFrameworkSearchProvider();
            mockSearchProvider.Setup(x => x.SearchByFrameworkLocation(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), null)).Throws<SearchException>();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var mockPostCodeLookup = CreateMockPostCodeLookup();
            var mockPaginationSettings = new Mock<IPaginationSettings>();

            var mockLogger = new Mock<ILog> { DefaultValue = DefaultValue.Mock };
            var service = new ProviderSearchService(mockSearchProvider.Object, mockStandardRepository.Object, mockFrameworkRepository.Object, mockPostCodeLookup.Object, mockLogger.Object, mockPaginationSettings.Object);

            var pagination = new Pagination
            {
                Page = 0,
                Take = 10
            };

            var result = await service.SearchFrameworkProviders(123, "AS3 4AS", pagination, null, false);

            Assert.That(result.HasError, Is.True);
        }

        private static Mock<IProviderLocationSearchProvider> CreateMockStandardSearchProvider(List<StandardProviderSearchResultsItem> stubSearchResults, long totalHits = 0)
        {
            var searchResults = new SearchResult<StandardProviderSearchResultsItem> { Hits = stubSearchResults, Total = totalHits };

            var mockSearchProvider = new Mock<IProviderLocationSearchProvider>();
            mockSearchProvider.Setup(x => x.SearchByStandardLocation(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).Returns(searchResults);

            return mockSearchProvider;
        }

        private static Mock<IProviderLocationSearchProvider> CreateMockStandardSearchProvider()
        {
            return CreateMockStandardSearchProvider(null);
        }

        private static Mock<IProviderLocationSearchProvider> CreateMockFrameworkSearchProvider(List<FrameworkProviderSearchResultsItem> stubSearchResults, long totalHits = 0)
        {
            var searchResults = new SearchResult<FrameworkProviderSearchResultsItem> { Hits = stubSearchResults, Total = totalHits };

            var mockSearchProvider = new Mock<IProviderLocationSearchProvider>();
            mockSearchProvider.Setup(x => x.SearchByFrameworkLocation(It.IsAny<int>(), It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).Returns(searchResults);

            return mockSearchProvider;
        }

        private static Mock<IProviderLocationSearchProvider> CreateMockFrameworkSearchProvider()
        {
            return CreateMockFrameworkSearchProvider(null);
        }

        private static Mock<IGetStandards> CreateMockStandardRepository()
        {
            var mockStandardsRepository = new Mock<IGetStandards>();

            return mockStandardsRepository;
        }

        private static Mock<IGetFrameworks> CreateMockFrameworkRepository()
        {
            var mockFrameworkRepository = new Mock<IGetFrameworks>();

            return mockFrameworkRepository;
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