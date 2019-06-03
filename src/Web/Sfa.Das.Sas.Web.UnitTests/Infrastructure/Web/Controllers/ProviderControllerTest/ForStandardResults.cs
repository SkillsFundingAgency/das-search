using System.Threading;
using System.Web.Routing;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ProviderControllerTest
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ApplicationServices.Queries;
    using ApplicationServices.Responses;
    using AutoMapper;
    using ControllerBuilders;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Controllers;
    using ViewModels;

    [TestFixture]
    public class ForStandardResults
    {
        [Test]
        public async Task SearchResultsShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            var stubSearchResponse = new StandardProviderSearchResponse { Success = true };

            var stubViewModel = new ProviderStandardSearchResultViewModel();

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupMappingService(x => x.Map(It.IsAny<StandardProviderSearchResponse>(), It.IsAny<Action<IMappingOperationOptions<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>>>()), stubViewModel)
                                .SetupMediator(x => x.Send<StandardProviderSearchResponse>(It.IsAny<StandardProviderSearchQuery>(), default(CancellationToken)), Task.FromResult(stubSearchResponse));

            var result = await controller.StandardResults(new StandardProviderSearchQuery());

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;
            viewResult.Model.Should().Be(stubViewModel);
        }

        [Test]
        public async Task SearchResultsShouldReturnBadRequestStatusCodeIfApprenticeshipIdIsInvalid()
        {
            var stubSearchResponse = new StandardProviderSearchResponse { Success = true, StatusCode = ProviderSearchResponseCodes.InvalidApprenticeshipId };

            var stubViewModel = new ProviderStandardSearchResultViewModel();

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupMappingService(x => x.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(It.IsAny<StandardProviderSearchResponse>()), stubViewModel)
                                .SetupMediator(x => x.Send<StandardProviderSearchResponse>(It.IsAny<StandardProviderSearchQuery>(), default(CancellationToken)), Task.FromResult(stubSearchResponse));

            var result = await controller.StandardResults(new StandardProviderSearchQuery());

            result.Should().BeOfType<HttpStatusCodeResult>();

            var viewResult = (HttpStatusCodeResult)result;

            viewResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task SearchResultsShouldReturnNotFoundStatusCodeIfTheStandardIsNotFound()
        {
            var stubSearchResponse = new StandardProviderSearchResponse { Success = true, StatusCode = ProviderSearchResponseCodes.ApprenticeshipNotFound };

            var stubViewModel = new ProviderStandardSearchResultViewModel();

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupMappingService(x => x.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(It.IsAny<StandardProviderSearchResponse>()), stubViewModel)
                                .SetupMediator(x => x.Send<StandardProviderSearchResponse>(It.IsAny<StandardProviderSearchQuery>(), default(CancellationToken)), Task.FromResult(stubSearchResponse));

            var result = await controller.StandardResults(new StandardProviderSearchQuery());

            result.Should().BeOfType<HttpStatusCodeResult>();

            var viewResult = (HttpStatusCodeResult)result;

            viewResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task SearchResultsShouldRedirectToSearchEntryPageIfPostCodeIsNotSet()
        {
            var stubSearchResponse = new StandardProviderSearchResponse { Success = true, StatusCode = ProviderSearchResponseCodes.PostCodeInvalidFormat };
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Action("SearchForStandardProviders", "Apprenticeship", It.IsAny<object>())).Returns("someurl");

            var stubViewModel = new ProviderStandardSearchResultViewModel();

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupMappingService(x => x.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(It.IsAny<StandardProviderSearchResponse>()), stubViewModel)
                                .SetupMediator(x => x.Send<StandardProviderSearchResponse>(It.IsAny<StandardProviderSearchQuery>(), default(CancellationToken)), Task.FromResult(stubSearchResponse))
                                .WithUrl(mockUrlHelper.Object);

            var result = await controller.StandardResults(new StandardProviderSearchQuery());

            result.Should().BeOfType<RedirectResult>();

            var viewResult = (RedirectResult)result;

            viewResult.Url.Should().Be("someurl");
        }

        [Test]
        public async Task SearchResultsShouldRedirectToLatPageIfParameterExtendsUpperBound()
        {
            var stubSearchResponse = new StandardProviderSearchResponse { Success = true, StatusCode = ProviderSearchResponseCodes.PageNumberOutOfUpperBound };
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Action("StandardResults", "Provider", It.IsAny<RouteValueDictionary>())).Returns("someurl");

            var stubViewModel = new ProviderStandardSearchResultViewModel();

            ProviderController controller = new ProviderControllerBuilder()
                                .SetupMappingService(x => x.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(It.IsAny<StandardProviderSearchResponse>()), stubViewModel)
                                .SetupMediator(x => x.Send<StandardProviderSearchResponse>(It.IsAny<StandardProviderSearchQuery>(), default(CancellationToken)), Task.FromResult(stubSearchResponse))
                                .WithUrl(mockUrlHelper.Object);

            var result = await controller.StandardResults(new StandardProviderSearchQuery());

            result.Should().BeOfType<RedirectResult>();

            var viewResult = (RedirectResult)result;

            viewResult.Url.Should().Be("someurl");
        }
    }
}
