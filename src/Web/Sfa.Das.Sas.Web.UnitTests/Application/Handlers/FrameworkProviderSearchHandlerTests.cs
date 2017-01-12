using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Application
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Sfa.Das.Sas.ApplicationServices;
    using Sfa.Das.Sas.ApplicationServices.Handlers;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.ApplicationServices.Queries;
    using Sfa.Das.Sas.ApplicationServices.Settings;
    using Sfa.Das.Sas.ApplicationServices.Validators;
    using Sfa.Das.Sas.Core.Logging;

    [TestFixture]
    public class FrameworkProviderSearchHandlerTests
    {
        private Mock<IProviderSearchService> _mockSearchService;
        private Mock<ILog> _mockLogger;
        private Mock<IPaginationSettings> _mockPaginationSettings;

        private FrameworkProviderSearchHandler _handler;
        private Mock<IPostcodeIoService> _mockPostcodeIoService;

        [SetUp]
        public void Setup()
        {
            _mockSearchService = new Mock<IProviderSearchService>();
            _mockPostcodeIoService = new Mock<IPostcodeIoService>();
            _mockLogger = new Mock<ILog>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();

            var providerFrameworkSearchResults = new ProviderFrameworkSearchResults();
            _mockSearchService.Setup(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerFrameworkSearchResults));

            _handler = new FrameworkProviderSearchHandler(new ProviderSearchQueryValidator(new Validation()), _mockSearchService.Object, _mockPaginationSettings.Object, _mockPostcodeIoService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task ShouldReturnSuccessWhenSearchIsSuccessful()
        {
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = "AB23 0BB" };

            var response = await _handler.Handle(message);

            response.StatusCode.Should().Be(FrameworkProviderSearchResponse.ResponseCodes.Success);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsNull()
        {
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = null };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsEmpty()
        {
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = string.Empty };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsInvalidFormat()
        {
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = "gfsgfdgds" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureOfInvalidApprenticeshipIdAndPostcodeWhenBothInvalid()
        {
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "0", PostCode = "gfsgfdgds" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldReturnSearchTerms()
        {
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Keywords = "abba 42" };

            var response = await _handler.Handle(message);

            response.SearchTerms.Should().Be("abba 42");
        }

        [Test]
        public async Task ShouldReturnShowAllProvidersFlag()
        {
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", ShowAll = true };

            var response = await _handler.Handle(message);

            response.ShowAllProviders.Should().BeTrue();
        }

        [Test]
        public async Task ShouldReturnResultOfSearch()
        {
            var providerFrameworkSearchResults = new ProviderFrameworkSearchResults();
            _mockSearchService.Setup(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerFrameworkSearchResults));
            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.Results.Should().BeSameAs(providerFrameworkSearchResults);
        }

        [Test]
        public async Task ShouldReturnTotalResultForCountryIfNoResultOnPostCodeSearch()
        {
            var providerFrameworkSearchResults = new ProviderFrameworkSearchResults() { TotalResults = 0 };
            var providerFrameworkSearchResultsAllCountry = new ProviderFrameworkSearchResults() { TotalResults = 5 };
            _mockSearchService.Setup(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), false)).Returns(Task.FromResult(providerFrameworkSearchResults));
            _mockSearchService.Setup(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), true)).Returns(Task.FromResult(providerFrameworkSearchResultsAllCountry));

            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.TotalResultsForCountry.Should().Be(5);
        }

        [Test]
        public async Task ShouldReturnLastPageIfCurrentPageExtendsUpperBound()
        {
            _mockPaginationSettings.Setup(x => x.DefaultResultsAmount).Returns(10);
            var providerFrameworkSearchResults = new ProviderFrameworkSearchResults() { TotalResults = 42, Hits = new List<IApprenticeshipProviderSearchResultsItem>() };
            _mockSearchService.Setup(x => x.SearchFrameworkProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), false)).Returns(Task.FromResult(providerFrameworkSearchResults));

            var message = new FrameworkProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 8 };

            var response = await _handler.Handle(message);

            response.CurrentPage.Should().Be(5);
            response.StatusCode.ShouldBeEquivalentTo(FrameworkProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound);
        }
    }
}
