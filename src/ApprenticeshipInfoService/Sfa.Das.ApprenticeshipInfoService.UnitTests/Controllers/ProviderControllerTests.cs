using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using FluentAssertions;
using NUnit.Framework.Constraints;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;
using Sfa.Das.ApprenticeshipInfoService.Core.Models.Responses;
using SFA.DAS.Apprenticeships.Api.Types;

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
        public void ShouldReturnProvider()
        {
            var expected = new List<Provider> { new Provider() };

            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();
            var mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();

            mockGetProviders.Setup(
                x =>
                    x.GetProvidersByUkprn(1)).Returns(expected);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object, mockApprenticeshipProviderRepository.Object);
            var actual = _sut.Get(1);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ShouldReturnProvidersNotFound()
        {
            var expected = new List<Provider> { new Provider() };

            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();
            var mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();

            mockGetProviders.Setup(
                x =>
                    x.GetProvidersByUkprn(1)).Returns(expected);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object, mockApprenticeshipProviderRepository.Object);

            ActualValueDelegate<object> test = () => _sut.Get(-2);

            Assert.That(test, Throws.TypeOf<HttpResponseException>());
        }

        [Test]
        public void ShouldReturnValidListOfStandardProviders()
        {
            var element = new StandardProviderSearchResultsItemResponse
            {
                ProviderName = "Test provider name",
                ApprenticeshipInfoUrl = "http://www.abba.co.uk"
            };
            var expected = new List<StandardProviderSearchResultsItemResponse> { element };

            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();
            var mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            
            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int>())).Returns(1);
            mockGetProviders.Setup(
                x =>
                    x.GetByStandardIdAndLocation(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double>(),
                        It.IsAny<int>())).Returns(expected);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object, mockApprenticeshipProviderRepository.Object);

            var response = _sut.GetByStandardIdAndLocation(1, 2, 3, 1);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<StandardProviderSearchResultsItemResponse>>();
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
            var element = new FrameworkProviderSearchResultsItemResponse
            {
                ProviderName = "Test provider name",
                ApprenticeshipInfoUrl = "http://www.abba.co.uk"
            };
            var expected = new List<FrameworkProviderSearchResultsItemResponse> { element };

            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();
            var mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            
            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int>())).Returns(1);
            mockGetProviders.Setup(
                x =>
                    x.GetByFrameworkIdAndLocation(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double>(),
                        It.IsAny<int>())).Returns(expected);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object, mockApprenticeshipProviderRepository.Object);

            var response = _sut.GetByFrameworkIdAndLocation(1, 2, 3, 1);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<FrameworkProviderSearchResultsItemResponse>>();
            response.Should().NotBeEmpty();
            response.Should().BeEquivalentTo(expected);
            response.First().Should().NotBe(null);
            response.First().Should().Be(element);
            response.First().ProviderName.Should().Be(element.ProviderName);
            response.First().ApprenticeshipInfoUrl.Should().Be(element.ApprenticeshipInfoUrl);
        }

        [Test]
        public void ShouldThrowExceptionIfLatLonIsNullSearchingByStandardId()
        {
            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();
            var mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();

            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int>())).Returns(1);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object, mockApprenticeshipProviderRepository.Object);

            ActualValueDelegate<object> test = () => _sut.GetByStandardIdAndLocation(1, null, null, 1);

            Assert.That(test, Throws.TypeOf<HttpResponseException>());
        }

        [Test]
        public void ShouldThrowExceptionIfLatLonIsNullSearchingByFrameworkId()
        {
            var mockGetProviders = new Mock<IGetProviders>();
            var mockControllerHelper = new Mock<IControllerHelper>();
            var mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            
            mockControllerHelper.Setup(x => x.GetActualPage(It.IsAny<int>())).Returns(1);

            var _sut = new ProvidersController(mockGetProviders.Object, mockControllerHelper.Object, mockApprenticeshipProviderRepository.Object);

            ActualValueDelegate<object> test = () => _sut.GetByFrameworkIdAndLocation(1, null, null, 1);

            Assert.That(test, Throws.TypeOf<HttpResponseException>());
        }
    }
}
