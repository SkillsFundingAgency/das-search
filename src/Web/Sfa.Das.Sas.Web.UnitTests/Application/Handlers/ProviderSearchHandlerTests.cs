using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;
using Moq;
using NUnit.Framework;

using SFA.DAS.NLog.Logger;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public sealed class ProviderSearchHandlerTests
    {
        private Mock<IProviderSearchService> _mockSearchService;
        private Mock<ILog> _mockLogger;
        private Mock<IPaginationSettings> _mockPaginationSettings;

        private StandardProviderSearchHandler _handler;
        private Mock<IPostcodeIoService> _mockPostcodeIoService;

        [SetUp]
        public void Setup()
        {
            _mockSearchService = new Mock<IProviderSearchService>();
            _mockPostcodeIoService = new Mock<IPostcodeIoService>();
            _mockLogger = new Mock<ILog>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();

            var providerStandardSearchResults = new ProviderStandardSearchResults
            {
                StandardResponseCode = LocationLookupResponse.Ok
            };
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));

            _handler = new StandardProviderSearchHandler(
                new ProviderSearchQueryValidator(new Validation()),
                _mockSearchService.Object,
                _mockPaginationSettings.Object,
                _mockPostcodeIoService.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task ShouldReturnSuccessWhenSearchIsSuccessful()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "AB23 0BB" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeTrue();
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsNull()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = null };
            _mockPostcodeIoService.Setup(x => x.GetPostcodeStatus(It.IsAny<string>()))
                .ReturnsAsync("Error");
            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsEmpty()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = string.Empty };
            _mockPostcodeIoService.Setup(x => x.GetPostcodeStatus(It.IsAny<string>()))
                .ReturnsAsync("Error");
            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsInvalidFormat()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "gfsgfdgds" };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.PostCodeInvalidFormat);
        }

        [TestCase("Northern Ireland", ProviderSearchResponseCodes.NorthernIrelandPostcode)]
        [TestCase("Scotland", ProviderSearchResponseCodes.ScotlandPostcode)]
        [TestCase("Wales", ProviderSearchResponseCodes.WalesPostcode)]
        [TestCase("Error", ProviderSearchResponseCodes.PostCodeInvalidFormat)]
        [TestCase("Terminated", ProviderSearchResponseCodes.PostCodeTerminated)]
        public async Task ShouldNotValidateThePostCode(string returnCode, ProviderSearchResponseCodes expected)
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "nw67xt" };

            _mockPostcodeIoService.Setup(m => m.GetPostcodeStatus(It.IsAny<string>()))
                .ReturnsAsync(returnCode);

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public async Task ShouldSignalFailureWhenApprenticeshipIsNotFound()
        {
            var providerStandardSearchResults = new ProviderStandardSearchResults { StandardResponseCode = ProviderSearchResponseCodes.ApprenticeshipNotFound.ToString() };
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.ApprenticeshipNotFound);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task ShouldReturnPageNumberOfOneIfPageNumberInRequestIsLessThanOne(int page)
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = page };

            var response = await _handler.Handle(message);

            response.CurrentPage.Should().Be(1);
        }

        [TestCase(1)]
        [TestCase(42)]
        public async Task ShouldReturnPageNumberFromRequestIfPageNumberInRequestIsGreaterThanZero(int page)
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = page };

            var response = await _handler.Handle(message);

            response.CurrentPage.Should().Be(page);
        }

        [Test]
        public async Task ShouldReturnSearchTerms()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Keywords = "abba 42" };

            var response = await _handler.Handle(message);

            response.SearchTerms.Should().Be("abba 42");
        }

        [Test]
        public async Task ShouldReturnShowAllProvidersFlag()
        {
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", ShowAll = true };

            var response = await _handler.Handle(message);

            response.ShowAllProviders.Should().BeTrue();
        }

        [Test]
        public async Task ShouldReturnResultOfSearch()
        {
            var providerStandardSearchResults = new ProviderStandardSearchResults();
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));
            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.Results.Should().BeSameAs(providerStandardSearchResults);
        }

        [Test]
        public async Task ShouldReturnTotalResultForCountryIfNoResultOnPostCodeSearch()
        {
            var providerStandardSearchResults = new ProviderStandardSearchResults() { TotalResults = 0 };
            var providerStandardSearchResultsAllCountry = new ProviderStandardSearchResults() { TotalResults = 5 };
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), false, It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), true, It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResultsAllCountry));

            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message);

            response.TotalResultsForCountry.Should().Be(5);
        }

        [Test]
        public async Task ShouldReturnLastPageIfCurrentPageExtendsUpperBound()
        {
            _mockPaginationSettings.Setup(x => x.DefaultResultsAmount).Returns(10);
            var providerStandardSearchResults = new ProviderStandardSearchResults() { TotalResults = 42, Hits = new List<IApprenticeshipProviderSearchResultsItem>() };
            _mockSearchService.Setup(x => x.SearchStandardProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), false, It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));

            var message = new StandardProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 8 };

            var response = await _handler.Handle(message);

            response.CurrentPage.Should().Be(5);
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.PageNumberOutOfUpperBound);
        }
    }
}
