using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Web.UnitTests.Application
{
    using Sfa.Das.Sas.ApplicationServices.Settings;

    [TestFixture]
    public sealed class ProviderSearchHandlerTests
    {
        private Mock<IProviderSearchService> _mockSearchService;
        private Mock<ILog> _mockLogger;
        private Mock<IShortlistCollection<int>> _mockShortlistCollection;
        private Mock<IPaginationSettings> _mockPaginationSettings;

        private StandardProviderSearchHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockSearchService = new Mock<IProviderSearchService>();
            _mockLogger = new Mock<ILog>();
            _mockShortlistCollection = new Mock<IShortlistCollection<int>>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();

            var providerStandardSearchResults = new ProviderStandardSearchResults();
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));

            _handler = new StandardProviderSearchHandler(new ProviderSearchQueryValidator(new Validation()), _mockSearchService.Object, _mockShortlistCollection.Object, _mockPaginationSettings.Object, _mockLogger.Object);
        }

        [Test]
        public async Task ShouldReturnSuccessWhenSearchIsSuccessful()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "AB23 0BB" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeTrue();
        }

        [TestCase(0)]
        public async Task ShouldSignalFailureWhenApprenticeshipIdIdIsInvalid(int apprenticeshipId)
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = apprenticeshipId, PostCode = "AB23 0BB" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(StandardProviderSearchResponse.ResponseCodes.InvalidApprenticeshipId);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsNull()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = null };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsEmpty()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = string.Empty };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsInvalidFormat()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "gfsgfdgds" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureOfInvalidApprenticeshipIdAndPostcodeWhenBothInvalid()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 0, PostCode = "gfsgfdgds" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenApprenticeshipIsNotFound()
        {
            var providerStandardSearchResults = new ProviderStandardSearchResults { StandardNotFound = true };
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(StandardProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task ShouldReturnPageNumberOfOneIfPageNumberInRequestIsLessThanOne(int page)
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Page = page };

            var response = await _handler.Handle(message);

            response.CurrentPage.Should().Be(1);
        }

        [TestCase(1)]
        [TestCase(42)]
        public async Task ShouldReturnPageNumberFromRequestIfPageNumberInRequestIsGreaterThanZero(int page)
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Page = page };

            var response = await _handler.Handle(message);

            response.CurrentPage.Should().Be(page);
        }

        [Test]
        public async Task ShouldReturnSearchTerms()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Keywords = "abba 42" };

            var response = await _handler.Handle(message);

            response.SearchTerms.Should().Be("abba 42");
        }

        [Test]
        public async Task ShouldReturnShowAllProvidersFlag()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", ShowAll = true };

            var response = await _handler.Handle(message);

            response.ShowAllProviders.Should().BeTrue();
        }

        [Test]
        public async Task ShouldReturnResultOfSearch()
        {
            var providerStandardSearchResults = new ProviderStandardSearchResults();
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.Results.Should().BeSameAs(providerStandardSearchResults);
        }

        [Test]
        public async Task ShouldReturnShortlistedProviderLocationsFilterByApprenticeship()
        {
            var stubShortlist = new List<ShortlistedApprenticeship>()
            {
                new ShortlistedApprenticeship { ApprenticeshipId = 3 },
                new ShortlistedApprenticeship { ApprenticeshipId = 42 },
                new ShortlistedApprenticeship { ApprenticeshipId = 1 }
            };

            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(stubShortlist);
            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.Shortlist.ApprenticeshipId.Should().Be(1);
        }

        [Test]
        public async Task ShouldReturnTotalResultForCountryIfNoResultOnPostCodeSearch()
        {
            var providerStandardSearchResults = new ProviderStandardSearchResults() { TotalResults = 0 };
            var providerStandardSearchResultsAllCountry = new ProviderStandardSearchResults() { TotalResults = 5 };
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), false)).Returns(Task.FromResult(providerStandardSearchResults));
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), true)).Returns(Task.FromResult(providerStandardSearchResultsAllCountry));

            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.TotalResultsForCountry.Should().Be(5);
        }

        [Test]
        public async Task ShouldReturnLastPageIfCurrentPageExtendsUpperBound()
        {
            _mockPaginationSettings.Setup(x => x.DefaultResultsAmount).Returns(10);
            var providerStandardSearchResults = new ProviderStandardSearchResults() { TotalResults = 42, Hits = new List<IApprenticeshipProviderSearchResultsItem>() };
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), false)).Returns(Task.FromResult(providerStandardSearchResults));

            var message = new StandardProviderSearchQuery { ApprenticeshipId = 1, PostCode = "GU21 6DB", Page = 8 };

            var response = await _handler.Handle(message);

            response.CurrentPage.Should().Be(5);
            response.StatusCode.ShouldBeEquivalentTo(StandardProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound);
        }
    }
}
