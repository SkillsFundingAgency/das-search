using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ControllerBuilders;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    [TestFixture]
    public class ProviderControllerTests
    {
        [Test]
        public async Task SearchResultsShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            var searchResults = new ProviderStandardSearchResults
            {
                HasError = false,
                Hits = new List<StandardProviderSearchResultsItem>()
            };
            var stubViewModel = new ProviderStandardSearchResultViewModel();

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupProviderService(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Task.FromResult(searchResults))
                                .SetupMappingService(x => x.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(It.IsAny<ProviderStandardSearchResults>()), stubViewModel);

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = "AB3 1SD" };

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;
            viewResult.Model.Should().Be(stubViewModel);
        }

        [Test]
        public async Task SearchResultsShouldReturnBadRequestStatusCodeIfApprenticeshipIdIsInvalid()
        {
            const int InvalidApprenticeshipId = 0;

            ProviderController controller = new ProviderControllerBuilder();

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = InvalidApprenticeshipId, PostCode = "AB3 1SD" };

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<HttpStatusCodeResult>();

            var viewResult = (HttpStatusCodeResult)result;

            viewResult.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task SearchResultsShouldRedirectToSearchEntryPageIfPostCodeIsNotSet()
        {
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Action("SearchForProviders", "Apprenticeship", It.IsAny<object>())).Returns("someurl");

            ProviderController controller = new ProviderControllerBuilder()
                .WithUrl(mockUrlHelper.Object);

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 2, PostCode = string.Empty };

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<RedirectResult>();

            var viewResult = (RedirectResult)result;

            viewResult.Url.Should().Be("someurl");
        }

        [Test]
        public async Task SearchResultsShouldReturnViewResultWhenFrameworkSearchIsSuccessful()
        {
            var searchResults = new ProviderFrameworkSearchResults
            {
                HasError = false,
                Hits = new List<FrameworkProviderSearchResultsItem>()
            };
            var stubViewModel = new ProviderFrameworkSearchResultViewModel();

            ProviderController controller = new ProviderControllerBuilder()
                .SetupProviderService(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Task.FromResult(searchResults))
                .SetupMappingService(x => x.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(It.IsAny<ProviderFrameworkSearchResults>()), stubViewModel);

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = 123, PostCode = "AB3 1SD" };

            var result = await controller.FrameworkResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;
            viewResult.Model.Should().Be(stubViewModel);
        }

        [Test]
        public void DetailShouldReturnNotFoundResultIfViewModelFromCriteriaIsNull()
        {
            var searchCriteria = new ProviderLocationSearchCriteria();

            ProviderController controller = new ProviderControllerBuilder()
                .SetupViewModelFactory(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()), null);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<HttpNotFoundResult>();

            var responseResult = (HttpNotFoundResult)result;

            responseResult.StatusCode.Should().Be(404);
            responseResult.StatusDescription.Should().StartWith("Cannot find provider:");
        }

        [Test]
        public void DetailShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            var searchCriteria = new ProviderLocationSearchCriteria
            {
                StandardCode = "1",
                LocationId = "2",
                ProviderId = "3"
            };

            var stubProviderViewModel = new ApprenticeshipDetailsViewModel
            {
                Training = ApprenticeshipTrainingType.Standard
            };

            var httpContextMock = new Mock<HttpContextBase>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.Setup(m => m.UrlReferrer).Returns(new Uri("http://www.helloworld.com"));
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);

            ProviderController controller = new ProviderControllerBuilder()
                .SetupViewModelFactory(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()), stubProviderViewModel)
                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), new List<ShortlistedApprenticeship>())
                .WithControllerHttpContext(httpContextMock.Object)
                .WithUrl(urlHelperMock.Object);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            viewResult.Model.Should().Be(stubProviderViewModel);
        }

        [Test]
        public void DetailShouldReturnViewResultWhenFrameworkSearchIsSuccessful()
        {
            var searchCriteria = new ProviderLocationSearchCriteria
            {
                FrameworkId = "1",
                LocationId = "2",
                ProviderId = "3"
            };

            var stubProviderViewModel = new ApprenticeshipDetailsViewModel
            {
                Training = ApprenticeshipTrainingType.Framework
            };

            var httpContextMock = new Mock<HttpContextBase>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.Setup(m => m.UrlReferrer).Returns(new Uri("http://www.helloworld.com"));
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);

            ProviderController controller = new ProviderControllerBuilder()
                .SetupViewModelFactory(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()), stubProviderViewModel)
                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), new List<ShortlistedApprenticeship>())
                .WithUrl(urlHelperMock.Object)
                .WithControllerHttpContext(httpContextMock.Object);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            viewResult.Model.Should().Be(stubProviderViewModel);
        }
    }
}