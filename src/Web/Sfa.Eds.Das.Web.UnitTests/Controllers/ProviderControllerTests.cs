using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Das.Web.Controllers;
using Sfa.Das.Web.Services;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Eds.Das.Web.Models;
using Sfa.Eds.Das.Web.Services;
using Sfa.Eds.Das.Web.ViewModels;

namespace Sfa.Eds.Das.Web.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderControllerTests
    {
        private Mock<ILog> _mockLogger;

        private Mock<IMappingService> _mockMappingService;

        private Mock<IProviderSearchService> _mockProviderSearchService;

        private Mock<IProviderViewModelFactory> _mockViewModelFactory;

        [TestCase(null)]
        [TestCase("")]
        public async Task SearchResultsShouldRedirectToStandardDetailsIfPostCodeIsNotSet(string postCode)
        {
            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = postCode };

            var controller = new ProviderController(null, null, null, null);

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<RedirectToRouteResult>();

            var redirectResult = (RedirectToRouteResult)result;

            redirectResult?.RouteValues["id"].Should().Be(123);
            redirectResult?.RouteValues["HasError"].Should().Be(true);
            redirectResult?.RouteValues["controller"].Should().Be("Apprenticeship");
            redirectResult?.RouteValues["action"].Should().Be("Standard");
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

            _mockProviderSearchService.Setup(x => x.SearchByStandardPostCode(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(searchResults));
            _mockMappingService.Setup(
                x =>
                x.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(
                    It.IsAny<ProviderStandardSearchResults>())).Returns(stubViewModel);

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object);

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;
            viewResult.Model.Should().Be(stubViewModel);
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task SearchResultsShouldRedirectToFrameworkDetailsIfPostCodeIsNotSet(string postCode)
        {
            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = postCode };

            var controller = new ProviderController(null, null, null, null);

            var result = await controller.FrameworkResults(searchCriteria);

            result.Should().BeOfType<RedirectToRouteResult>();

            var redirectResult = (RedirectToRouteResult)result;

            redirectResult?.RouteValues["id"].Should().Be(123);
            redirectResult?.RouteValues["HasError"].Should().Be(true);
            redirectResult?.RouteValues["controller"].Should().Be("Apprenticeship");
            redirectResult?.RouteValues["action"].Should().Be("Framework");
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

            _mockProviderSearchService.Setup(x => x.SearchByFrameworkPostCode(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(searchResults));
            _mockMappingService.Setup(
                x =>
                x.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(
                    It.IsAny<ProviderFrameworkSearchResults>())).Returns(stubViewModel);

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object);

            var result = await controller.FrameworkResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;
            viewResult.Model.Should().Be(stubViewModel);
        }

        [Test]
        [Ignore("problem with the Request being used in the controller")]
        public void ShouldFindTheDetailsPageForAProviderAndFramework()
        {
            // Arrange
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderSearchService = new Mock<IProviderSearchService>();
            _mockViewModelFactory = new Mock<IProviderViewModelFactory>();

            var searchCriteria = new ProviderLocationSearchCriteria
                                     {
                                         FrameworkId = "123",
                                         LocationId = "123",
                                         ProviderId = "123"
                                     };
            var stubViewModel = new ProviderViewModel();

            _mockViewModelFactory.Setup(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()))
                .Returns(new ProviderViewModel { Training = ApprenticeshipTrainingType.Framework });

            var controller = new ProviderController(
                _mockProviderSearchService.Object,
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockViewModelFactory.Object);

            // Act
            var result = controller.Detail(searchCriteria);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(stubViewModel));
        }
    }
}