using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Eds.Das.Web.Models;
using Sfa.Eds.Das.Web.Services;
using Sfa.Eds.Das.Web.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sfa.Eds.Das.Core.Domain.Services;

namespace Sfa.Eds.Das.Web.Controllers.Tests
{
    [TestFixture]
    public class ProviderControllerTests
    {
        private Mock<ILog> mockLogger;
        private Mock<IStandardRepository> mockStandardRepository;
        private Mock<IMappingService> mockMappingService;
        private Mock<IProviderSearchService> mockProviderSearchService;
        private Mock<IApprenticeshipProviderRepository> mockApprenticeshipProviderRepository;

        [Test]
        public async Task SearchResultsShouldReturnViewResultWhenSearchIsSuccessful()
        {
            mockLogger = new Mock<ILog>();
            mockMappingService = new Mock<IMappingService>();
            mockProviderSearchService = new Mock<IProviderSearchService>();
            mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            mockStandardRepository = new Mock<IStandardRepository>();
            var searchCriteria = new ProviderSearchCriteria { StandardId = 123, PostCode = "AB3 1SD" };
            var searchResults = new ProviderSearchResults {HasError = false, Hits = new List<ProviderSearchResultsItem>()};
            var stubViewModel = new ProviderSearchResultViewModel();

            mockProviderSearchService.Setup(x => x.SearchByPostCode(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(searchResults));
            mockMappingService.Setup(x => x.Map<ProviderSearchResults, ProviderSearchResultViewModel>(It.IsAny<ProviderSearchResults>())).Returns(stubViewModel);
            try
            {
                var controller = new ProviderController(mockProviderSearchService.Object, mockLogger.Object, mockMappingService.Object, mockApprenticeshipProviderRepository.Object, mockStandardRepository.Object);

                var result = await controller.SearchResults(searchCriteria);

                Assert.That(result, Is.InstanceOf<ViewResult>());

                var viewResult = (ViewResult)result;
                Assert.That(viewResult.Model, Is.EqualTo(stubViewModel));
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task SearchResultsShouldRedirectToStandardDetailsIfNoPostCodeIsNotSet(string postCode)
        {
            var searchCriteria = new ProviderSearchCriteria { StandardId = 123 };

            var controller = new ProviderController(null, null, null, null, null);

            var result = await controller.SearchResults(searchCriteria);

            Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());

            var redirectResult = (RedirectToRouteResult)result;
            Assert.That(redirectResult?.RouteValues["id"], Is.EqualTo(123));
            Assert.That(redirectResult?.RouteValues["HasError"], Is.EqualTo(true));
            Assert.That(redirectResult?.RouteValues["controller"], Is.EqualTo("Standard"));
            Assert.That(redirectResult?.RouteValues["action"], Is.EqualTo("Detail"));
        }
    }
}