using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Eds.Das.Web.Controllers;
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
        private Mock<IApprenticeshipProviderRepository> _mockApprenticeshipProviderRepository;

        [Test]
        public async Task SearchResultsShouldReturnViewResultWhenSearchIsSuccessful()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderSearchService = new Mock<IProviderSearchService>();
            _mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            var mockStandardRepository = CreateMockStandardRepository();
            var mockFrameworkRepository = CreateMockFrameworkRepository();
            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = "AB3 1SD" };
            var searchResults = new ProviderStandardSearchResults { HasError = false, Hits = new List<StandardProviderSearchResultsItem>() };
            var stubViewModel = new ProviderStandardSearchResultViewModel();

            _mockProviderSearchService.Setup(x => x.SearchByStandardPostCode(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(searchResults));
            _mockMappingService.Setup(x => x.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(It.IsAny<ProviderStandardSearchResults>())).Returns(stubViewModel);

            var controller = new ProviderController(_mockProviderSearchService.Object, _mockLogger.Object, _mockMappingService.Object, _mockApprenticeshipProviderRepository.Object, mockStandardRepository.Object, mockFrameworkRepository.Object);

            var result = await controller.StandardResults(searchCriteria);

            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(stubViewModel));
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task SearchResultsShouldRedirectToStandardDetailsIfNoPostCodeIsNotSet(string postCode)
        {
            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123 };

            var controller = new ProviderController(null, null, null, null, null, null);

            var result = await controller.StandardResults(searchCriteria);

            Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());

            var redirectResult = (RedirectToRouteResult)result;
            Assert.That(redirectResult?.RouteValues["id"], Is.EqualTo(123));
            Assert.That(redirectResult?.RouteValues["HasError"], Is.EqualTo(true));
            Assert.That(redirectResult?.RouteValues["controller"], Is.EqualTo("Apprenticeship"));
            Assert.That(redirectResult?.RouteValues["action"], Is.EqualTo("Standard"));
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
    }
}