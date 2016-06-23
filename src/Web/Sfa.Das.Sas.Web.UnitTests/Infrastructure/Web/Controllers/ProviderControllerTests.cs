using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
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
                                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), new List<ShortlistedApprenticeship>())
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
                .SetupMappingService(x => x.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(It.IsAny<ProviderFrameworkSearchResults>()), stubViewModel)
                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), new List<ShortlistedApprenticeship>());

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

        [Test]
        public void ShouldShowViewModelAsShortlisted()
        {
            var searchCriteria = new ProviderLocationSearchCriteria
            {
                FrameworkId = "1",
                LocationId = "2",
                ProviderId = "3"
            };

            var stubProviderViewModel = new ApprenticeshipDetailsViewModel
            {
                Training = ApprenticeshipTrainingType.Framework,
                Apprenticeship = new ApprenticeshipBasic
                {
                    Code = int.Parse(searchCriteria.FrameworkId)
                }
            };

            var httpContextMock = new Mock<HttpContextBase>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.Setup(m => m.UrlReferrer).Returns(new Uri("http://www.helloworld.com"));
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);

            var shortlistedApprenticeships = new List<ShortlistedApprenticeship>
            {
                new ShortlistedApprenticeship
                {
                    ApprenticeshipId = int.Parse(searchCriteria.FrameworkId),
                    ProvidersIdAndLocation = new List<ShortlistedProvider>()
                    {
                        new ShortlistedProvider()
                        {
                            LocationId = int.Parse(searchCriteria.LocationId),
                            ProviderId = searchCriteria.ProviderId
                        }
                    }
                }
            };

            ProviderController controller = new ProviderControllerBuilder()
                .SetupViewModelFactory(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()), stubProviderViewModel)
                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), shortlistedApprenticeships)
                .WithUrl(urlHelperMock.Object)
                .WithControllerHttpContext(httpContextMock.Object);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            var viewmodel = viewResult.Model as ApprenticeshipDetailsViewModel;

            viewmodel.IsShortlisted.Should().BeTrue();
        }

        [Test]
        public void ShouldShowViewModelAsNotShortlistedIfProviderIsWithDifferentApprenticeship()
        {
            var searchCriteria = new ProviderLocationSearchCriteria
            {
                FrameworkId = "1",
                LocationId = "2",
                ProviderId = "3"
            };

            var stubProviderViewModel = new ApprenticeshipDetailsViewModel
            {
                Training = ApprenticeshipTrainingType.Framework,
                Apprenticeship = new ApprenticeshipBasic
                {
                    Code = int.Parse(searchCriteria.FrameworkId)
                }
            };

            var httpContextMock = new Mock<HttpContextBase>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.Setup(m => m.UrlReferrer).Returns(new Uri("http://www.helloworld.com"));
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);

            var shortlistedApprenticeships = new List<ShortlistedApprenticeship>
            {
                new ShortlistedApprenticeship
                {
                    ApprenticeshipId = 28738,
                    ProvidersIdAndLocation = new List<ShortlistedProvider>()
                    {
                        new ShortlistedProvider()
                        {
                            LocationId = int.Parse(searchCriteria.LocationId),
                            ProviderId = searchCriteria.ProviderId
                        }
                    }
                }
            };

            ProviderController controller = new ProviderControllerBuilder()
                .SetupViewModelFactory(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()), stubProviderViewModel)
                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), shortlistedApprenticeships)
                .WithUrl(urlHelperMock.Object)
                .WithControllerHttpContext(httpContextMock.Object);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            var viewmodel = viewResult.Model as ApprenticeshipDetailsViewModel;

            viewmodel.IsShortlisted.Should().BeFalse();
        }

        public void ShouldReturnShortlistedResultsWhenStandardHadddsBeenShortlisted()
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

            var shortlistedApprentices = new List<ShortlistedApprenticeship>()
            {
                new ShortlistedApprenticeship
                {
                    ApprenticeshipId = int.Parse(searchCriteria.StandardCode),
                    ProvidersIdAndLocation = new List<ShortlistedProvider>
                    {
                        new ShortlistedProvider
                        {
                            LocationId = int.Parse(searchCriteria.LocationId),
                            ProviderId = searchCriteria.ProviderId
                        }
                    }
                }
            };

            ProviderController controller = new ProviderControllerBuilder()
                .SetupViewModelFactory(x => x.GenerateDetailsViewModel(It.IsAny<ProviderLocationSearchCriteria>()), stubProviderViewModel)
                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), shortlistedApprentices)
                .WithControllerHttpContext(httpContextMock.Object)
                .WithUrl(urlHelperMock.Object);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            viewResult.Model.Should().Be(stubProviderViewModel);
        }

        [Test]
        public async Task ShouldReturnShortlistedResultsWhenStandardProviderHasBeenShortlisted()
        {
            var searchResultItem = new ProviderResultItemViewModel
            {
                Id = "32",
                LocationId = 2,
                StandardCode = 1
            };

            var searchResults = new ProviderStandardSearchResults
            {
                HasError = false
            };

            var searchResultViewModel = new ProviderStandardSearchResultViewModel()
            {
                Hits = new List<ProviderResultItemViewModel>
                {
                    searchResultItem
                }
            };

            var shortlistedApprentices = new List<ShortlistedApprenticeship>
            {
                new ShortlistedApprenticeship
                {
                    ApprenticeshipId = searchResultItem.StandardCode,
                    ProvidersIdAndLocation = new List<ShortlistedProvider>
                    {
                        new ShortlistedProvider
                        {
                            LocationId = searchResultItem.LocationId,
                            ProviderId = searchResultItem.Id
                        }
                    }
                }
            };

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupProviderService(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Task.FromResult(searchResults))
                                .SetupMappingService(x => x.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(It.IsAny<ProviderStandardSearchResults>()), searchResultViewModel)
                                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), shortlistedApprentices);

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = searchResultItem.StandardCode, PostCode = "AB3 1SD" };

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            var results = viewResult.Model as ProviderStandardSearchResultViewModel;

            results.Should().NotBeNull();

            results.Hits.Should().NotBeNull();

            results.Hits.Count().Should().Be(1);

            results.Hits.Should().Contain(x => x.IsShortlisted);
        }

        [Test]
        public async Task ShouldReturnNoShortlistedResultsWhenStandardProviderHasNotBeenShortlisted()
        {
            var searchResultItem = new ProviderResultItemViewModel
            {
                Id = "32",
                LocationId = 2,
                StandardCode = 1
            };

            var searchResults = new ProviderStandardSearchResults
            {
                HasError = false
            };

            var searchResultViewModel = new ProviderStandardSearchResultViewModel()
            {
                Hits = new List<ProviderResultItemViewModel>
                {
                    searchResultItem
                }
            };

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupProviderService(x => x.SearchStandardProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Task.FromResult(searchResults))
                                .SetupMappingService(x => x.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(It.IsAny<ProviderStandardSearchResults>()), searchResultViewModel)
                                .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), new List<ShortlistedApprenticeship>());

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = searchResultItem.StandardCode, PostCode = "AB3 1SD" };

            var result = await controller.StandardResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            var results = viewResult.Model as ProviderStandardSearchResultViewModel;

            results.Should().NotBeNull();

            results.Hits.Should().NotBeNull();

            results.Hits.Count().Should().Be(1);

            results.Hits.All(x => !x.IsShortlisted).Should().BeTrue();
        }

        [Test]
        public async Task ShouldReturnShortlistedResultsWhenFrameworkProviderHasBeenShortlisted()
        {
            var searchResultItem = new FrameworkProviderResultItemViewModel
            {
                Id = "32",
                LocationId = 2,
                FrameworkCode = 1
            };

            var searchResults = new ProviderFrameworkSearchResults
            {
                HasError = false
            };

            var searchResultViewModel = new ProviderFrameworkSearchResultViewModel
            {
                Hits = new List<FrameworkProviderResultItemViewModel>
                {
                    searchResultItem
                }
            };

            var shortlistedApprentices = new List<ShortlistedApprenticeship>
            {
                new ShortlistedApprenticeship
                {
                    ApprenticeshipId = searchResultItem.FrameworkCode,
                    ProvidersIdAndLocation = new List<ShortlistedProvider>
                    {
                        new ShortlistedProvider
                        {
                            LocationId = searchResultItem.LocationId,
                            ProviderId = searchResultItem.Id
                        }
                    }
                }
            };

            ProviderController controller = new ProviderControllerBuilder()
               .SetupProviderService(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Task.FromResult(searchResults))
               .SetupMappingService(x => x.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(It.IsAny<ProviderFrameworkSearchResults>()), searchResultViewModel)
               .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), shortlistedApprentices);

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = searchResultItem.FrameworkCode, PostCode = "AB3 1SD" };

            var result = await controller.FrameworkResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            var results = viewResult.Model as ProviderFrameworkSearchResultViewModel;

            results.Should().NotBeNull();

            results.Hits.Should().NotBeNull();

            results.Hits.Count().Should().Be(1);

            results.Hits.Should().Contain(x => x.IsShortlisted);
        }

        [Test]
        public async Task ShouldReturnNoShortlistedResultsWhenFrameworkProviderHasNotBeenShortlisted()
        {
            var searchResultItem = new FrameworkProviderResultItemViewModel
            {
                Id = "32",
                LocationId = 2,
                FrameworkCode = 1
            };

            var searchResults = new ProviderFrameworkSearchResults
            {
                HasError = false
            };

            var searchResultViewModel = new ProviderFrameworkSearchResultViewModel
            {
                Hits = new List<FrameworkProviderResultItemViewModel>
                {
                    searchResultItem
                }
            };

            ProviderController controller = new ProviderControllerBuilder()
               .SetupProviderService(x => x.SearchFrameworkProviders(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Pagination>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Task.FromResult(searchResults))
               .SetupMappingService(x => x.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(It.IsAny<ProviderFrameworkSearchResults>()), searchResultViewModel)
               .SetupCookieCollection(x => x.GetAllItems(It.IsAny<string>()), new List<ShortlistedApprenticeship>());

            var searchCriteria = new ProviderSearchCriteria { ApprenticeshipId = searchResultItem.FrameworkCode, PostCode = "AB3 1SD" };

            var result = await controller.FrameworkResults(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            var results = viewResult.Model as ProviderFrameworkSearchResultViewModel;

            results.Should().NotBeNull();

            results.Hits.Should().NotBeNull();

            results.Hits.Count().Should().Be(1);

            results.Hits.All(x => !x.IsShortlisted).Should().BeTrue();
        }
    }
}