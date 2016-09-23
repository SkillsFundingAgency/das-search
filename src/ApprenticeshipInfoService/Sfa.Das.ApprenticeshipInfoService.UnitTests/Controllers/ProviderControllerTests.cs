using System.Linq;
using FluentAssertions;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace Sfa.Das.ApprenticeshipInfoService.UnitTests.Controllers
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Sfa.Das.ApprenticeshipInfoService.Api.Controllers;
    using Sfa.Das.ApprenticeshipInfoService.Core.Helpers;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;

    public class ProviderControllerTests
    {
        [Test]
        public void ShouldReturnValidListOfStandardProviders()
        {
            var element = new StandardProviderSearchResultsItem
            {
                ProviderName = "Test provider name",
                ApprenticeshipInfoUrl = "http://www.abba.co.uk"
            };
            var expected = new List<StandardProviderSearchResultsItem> { element };

            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();

            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int?>())).Returns(1);
            mockGetProviders.Setup(
                x =>
                    x.GetByStandardIdAndLocation(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double>(),
                        It.IsAny<int>())).Returns(expected);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object);

            var response = _sut.GetByStandardIdAndLocation(1, 2, 3, 1);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<StandardProviderSearchResultsItem>>();
            response.Should().NotBeEmpty();
            response.Should().BeEquivalentTo(expected);
            response.First().Should().NotBe(null);
            response.First().Should().Be(element);
            response.First().ProviderName.Should().Be(element.ProviderName);
            response.First().ApprenticeshipInfoUrl.Should().Be(element.ApprenticeshipInfoUrl);
        }

        [Test]
        public void ShouldReturnValidListOfFrameworkProviders()
        {
            var element = new FrameworkProviderSearchResultsItem
            {
                ProviderName = "Test provider name",
                ApprenticeshipInfoUrl = "http://www.abba.co.uk"
            };
            var expected = new List<FrameworkProviderSearchResultsItem> { element };

            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();

            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int?>())).Returns(1);
            mockGetProviders.Setup(
                x =>
                    x.GetByFrameworkIdAndLocation(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double>(),
                        It.IsAny<int>())).Returns(expected);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object);

            var response = _sut.GetByFrameworkIdAndLocation(1, 2, 3, 1);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<FrameworkProviderSearchResultsItem>>();
            response.Should().NotBeEmpty();
            response.Should().BeEquivalentTo(expected);
            response.First().Should().NotBe(null);
            response.First().Should().Be(element);
            response.First().ProviderName.Should().Be(element.ProviderName);
            response.First().ApprenticeshipInfoUrl.Should().Be(element.ApprenticeshipInfoUrl);
        }

        [Test]
        public void ShouldReturnEmptyListOfStandardProvidersIfLatLonIsNull()
        {
            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();

            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int?>())).Returns(1);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object);

            var response = _sut.GetByStandardIdAndLocation(1, null, null, 1);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<StandardProviderSearchResultsItem>>();
            response.Should().BeEmpty();
        }

        [Test]
        public void ShouldReturnEmptyListOfFrameworkProvidersIfLatLonIsNull()
        {
            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();

            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int?>())).Returns(1);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object);

            var response = _sut.GetByFrameworkIdAndLocation(1, null, null, 1);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<FrameworkProviderSearchResultsItem>>();
            response.Should().BeEmpty();
        }
    }
}
