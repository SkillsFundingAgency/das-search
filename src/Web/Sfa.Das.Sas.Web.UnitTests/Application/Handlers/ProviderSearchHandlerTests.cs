using System.Collections.Generic;
using System.Threading;
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

        private ProviderSearchHandler _handler;
        private Mock<IPostcodeIoService> _mockPostcodeIoService;

        [SetUp]
        public void Setup()
        {
            _mockSearchService = new Mock<IProviderSearchService>();
            _mockPostcodeIoService = new Mock<IPostcodeIoService>();
            _mockLogger = new Mock<ILog>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();

            var providerSearchResults = new ProviderSearchResults
            {
                ResponseCode = LocationLookupResponse.Ok
            };
            _mockSearchService.Setup(x => x.SearchProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerSearchResults));

            _handler = new ProviderSearchHandler(
                new ProviderSearchQueryValidator(new Validation()),
                _mockSearchService.Object,
                _mockPaginationSettings.Object,
                _mockPostcodeIoService.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task ShouldReturnSuccessWhenSearchIsSuccessful()
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "AB23 0BB" };

            var response = await _handler.Handle(message,default(CancellationToken));

            response.Success.Should().BeTrue();
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsNull()
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = null };
            _mockPostcodeIoService.Setup(x => x.GetPostcodeStatus(It.IsAny<string>()))
                .ReturnsAsync("Error");
            var response = await _handler.Handle(message, default(CancellationToken));

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsEmpty()
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = string.Empty };
            _mockPostcodeIoService.Setup(x => x.GetPostcodeStatus(It.IsAny<string>()))
                .ReturnsAsync("Error");
            var response = await _handler.Handle(message, default(CancellationToken));

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.PostCodeInvalidFormat);
        }

        [Test]
        public async Task ShouldSignalFailureWhenPostCodeIsInvalidFormat()
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "gfsgfdgds" };

            var response = await _handler.Handle(message, default(CancellationToken));

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
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "nw67xt" };

            _mockPostcodeIoService.Setup(m => m.GetPostcodeStatus(It.IsAny<string>()))
                .ReturnsAsync(returnCode);

            var response = await _handler.Handle(message, default(CancellationToken));

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public async Task ShouldSignalFailureWhenApprenticeshipIsNotFound()
        {
            var providerStandardSearchResults = new ProviderSearchResults { ResponseCode = ProviderSearchResponseCodes.ApprenticeshipNotFound.ToString() };
            _mockSearchService.Setup(x => x.SearchProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.Success.Should().BeFalse();
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.ApprenticeshipNotFound);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task ShouldReturnPageNumberOfOneIfPageNumberInRequestIsLessThanOne(int page)
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = page };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.CurrentPage.Should().Be(1);
        }

        [TestCase(1)]
        [TestCase(42)]
        public async Task ShouldReturnPageNumberFromRequestIfPageNumberInRequestIsGreaterThanZero(int page)
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = page };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.CurrentPage.Should().Be(page);
        }

        [Test]
        public async Task ShouldReturnSearchTerms()
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Keywords = "abba 42" };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.SearchTerms.Should().Be("abba 42");
        }

        [Test]
        public async Task ShouldReturnShowAllProvidersFlag()
        {
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", ShowAll = true };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.ShowAllProviders.Should().BeTrue();
        }

        [Test]
        public async Task ShouldReturnResultOfSearch()
        {
            var providerStandardSearchResults = new ProviderSearchResults();
            _mockSearchService.Setup(x => x.SearchProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));
            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 0 };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.Results.Should().BeSameAs(providerStandardSearchResults);
        }

        [Test]
        public async Task ShouldReturnLastPageIfCurrentPageExtendsUpperBound()
        {
            _mockPaginationSettings.Setup(x => x.DefaultResultsAmount).Returns(10);
            var providerStandardSearchResults = new ProviderSearchResults() { TotalResults = 42, Hits = new List<ProviderSearchResultItem>() };
            _mockSearchService.Setup(x => x.SearchProviders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(providerStandardSearchResults));

            var message = new ProviderSearchQuery { ApprenticeshipId = "1", PostCode = "GU21 6DB", Page = 8 };

            var response = await _handler.Handle(message, default(CancellationToken));

            response.CurrentPage.Should().Be(5);
            response.StatusCode.ShouldBeEquivalentTo(ProviderSearchResponseCodes.PageNumberOutOfUpperBound);
        }
    }
}
