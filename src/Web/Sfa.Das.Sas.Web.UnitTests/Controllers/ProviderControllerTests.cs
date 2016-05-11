using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Controllers
{
    using Sfa.Das.Sas.Core.Configuration;

    [TestFixture]
    public class ProviderControllerTests
    {
        private Mock<ILog> _mockLogger;

        private Mock<IMappingService> _mockMappingService;

        private Mock<IProviderSearchService> _mockProviderSearchService;

        private Mock<IProviderViewModelFactory> _mockViewModelFactory;

        private readonly IConfigurationSettings _configurationSettings = Mock.Of<IConfigurationSettings>(x => x.SurveyUrl == new Uri("http://test.com"));

        [TestCase(null, "null-id")]
        [TestCase("", "empty-id")]
        public async Task SearchResultsShouldRedirectToStandardDetailsIfPostCodeIsNotSet(string postCode, string inputId)
        {
            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = postCode, InputId = inputId };

            var controller = new ProviderController(null, null, null, null, _configurationSettings);

            var moqUrlHelper = new Mock<UrlHelper>();
            moqUrlHelper.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>())).Returns("http://url.co.uk");
            controller.Url = moqUrlHelper.Object;

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<RedirectResult>();

            var redirectResult = (RedirectResult)result;

            redirectResult?.Url.Should().Be($"http://url.co.uk#{inputId}");
        }

        [Test]
        public async Task SearchResultsShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderSearchService = new Mock<IProviderSearchService>();
            _mockViewModelFactory = new Mock<IProviderViewModelFactory>();

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = "AB3 1SD" };
            var searchResults = new ProviderStandardSearchResults
                                    {
                                        HasError = false,
                                        Hits = new List<StandardProviderSearchResultsItem>()
                                    };
            var stubViewModel = new ProviderStandardSearchResultViewModel();

            _mockProviderSearchService.Setup(x => x.SearchByStandardPostCode(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(searchResults));
            _mockMappingService.Setup(
                x =>
                x.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(
                    It.IsAny<ProviderStandardSearchResults>())).Returns(stubViewModel);

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object,
                _configurationSettings);

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;
            viewResult.Model.Should().Be(stubViewModel);
        }

        [TestCase(null, "null-id")]
        [TestCase("", "empty-id")]
        public async Task SearchResultsShouldRedirectToFrameworkDetailsIfPostCodeIsNotSet(string postCode, string inputId)
        {
            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = postCode, InputId = inputId };

            var controller = new ProviderController(null, null, null, null, _configurationSettings);

            var moqUrlHelper = new Mock<UrlHelper>();
            moqUrlHelper.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>())).Returns("http://url.co.uk");
            controller.Url = moqUrlHelper.Object;

            var result = await controller.FrameworkResults(searchCriteria);

            result.Should().BeOfType<RedirectResult>();

            var redirectResult = (RedirectResult)result;

            redirectResult?.Url.Should().Be($"http://url.co.uk#{inputId}");
        }

        [Test]
        public async Task SearchResultsShouldReturnViewResultWhenFrameworkSearchIsSuccessful()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderSearchService = new Mock<IProviderSearchService>();
            _mockViewModelFactory = new Mock<IProviderViewModelFactory>();

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = "AB3 1SD" };
            var searchResults = new ProviderFrameworkSearchResults
            {
                HasError = false,
                Hits = new List<FrameworkProviderSearchResultsItem>()
            };
            var stubViewModel = new ProviderFrameworkSearchResultViewModel();

            _mockProviderSearchService.Setup(x => x.SearchByFrameworkPostCode(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(searchResults));
            _mockMappingService.Setup(
                x =>
                x.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(
                    It.IsAny<ProviderFrameworkSearchResults>())).Returns(stubViewModel);

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object,
                _configurationSettings);

            var result = await controller.FrameworkResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;
            viewResult.Model.Should().Be(stubViewModel);
        }

        [Test]
        public void DetailShouldReturnNotFoundResultIfViewModelFromCriteriaIsNull()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderSearchService = new Mock<IProviderSearchService>();
            _mockViewModelFactory = new Mock<IProviderViewModelFactory>();

            var searchCriteria = new ProviderLocationSearchCriteria();

            _mockViewModelFactory.Setup(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()))
                .Returns((ProviderViewModel)null);

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object,
                _configurationSettings);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<HttpNotFoundResult>();

            var responseResult = (HttpNotFoundResult)result;

            responseResult.StatusCode.Should().Be(404);
            responseResult.StatusDescription.Should().StartWith("Cannot find provider:");
        }

        [Test]
        public void DetailShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderSearchService = new Mock<IProviderSearchService>();
            _mockViewModelFactory = new Mock<IProviderViewModelFactory>();

            var searchCriteria = new ProviderLocationSearchCriteria
            {
                StandardCode = "1",
                LocationId = "2",
                ProviderId = "3"
            };

            var stubProviderViewModel = new ProviderViewModel
            {
                SearchResultLink = new LinkViewModel
                {
                    Title = "Back to search page",
                    Url = string.Empty
                },
                Training = ApprenticeshipTrainingType.Standard
            };

            _mockViewModelFactory.Setup(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()))
                .Returns(stubProviderViewModel);

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object,
                _configurationSettings);

            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.Setup(m => m.UrlReferrer).Returns(new Uri("http://www.helloworld.com"));
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            controller.Url = urlHelperMock.Object;
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            viewResult.Model.Should().Be(stubProviderViewModel);
        }

        [Test]
        public void DetailShouldReturnViewResultWhenFrameworkSearchIsSuccessful()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderSearchService = new Mock<IProviderSearchService>();
            _mockViewModelFactory = new Mock<IProviderViewModelFactory>();

            var searchCriteria = new ProviderLocationSearchCriteria
            {
                FrameworkId = "1",
                LocationId = "2",
                ProviderId = "3"
            };

            var stubProviderViewModel = new ProviderViewModel
            {
                SearchResultLink = new LinkViewModel
                {
                    Title = "Back to search page",
                    Url = string.Empty
                },
                Training = ApprenticeshipTrainingType.Framework
            };

            _mockViewModelFactory.Setup(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()))
                .Returns(stubProviderViewModel);

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object,
                _configurationSettings);

            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.Setup(m => m.UrlReferrer).Returns(new Uri("http://www.helloworld.com"));
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            controller.Url = urlHelperMock.Object;
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            viewResult.Model.Should().Be(stubProviderViewModel);
        }
    }
}