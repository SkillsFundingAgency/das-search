using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ProviderControllerTest
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.ApplicationServices.Queries;
    using Sfa.Das.Sas.Web.Controllers;
    using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ControllerBuilders;
    using Sfa.Das.Sas.Web.ViewModels;

    [TestFixture]
    public class ProviderControllerTests
    {
        [Test]
        public void DetailShouldReturnNotFoundResultIfApprenticeshipProviderNotFound()
        {
            var searchCriteria = new ProviderDetailQuery();
            var stubSearchResponse = new DetailProviderResponse { StatusCode = DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound };

            ProviderController controller = new ProviderControllerBuilder()
                .SetupMediator(x => x.Send(It.IsAny<ProviderDetailQuery>()), stubSearchResponse);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<HttpStatusCodeResult>();

            var responseResult = (HttpStatusCodeResult)result;

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void DetailShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            var searchCriteria = new ProviderDetailQuery { StandardCode = "1", LocationId = 2, Ukprn = 3 };

            var stubSearchResponse = new DetailProviderResponse { };

            var stubProviderViewModel = new ApprenticeshipDetailsViewModel
            {
                ApprenticeshipType = ApprenticeshipTrainingType.Standard
            };

            var httpContextMock = new Mock<HttpContextBase>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.Setup(m => m.UrlReferrer).Returns(new Uri("http://www.helloworld.com"));
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);

            ProviderController controller = new ProviderControllerBuilder()
                .SetupMediator(x => x.Send(It.IsAny<ProviderDetailQuery>()), stubSearchResponse)
                .SetupMappingService(x => x.Map(It.IsAny<DetailProviderResponse>(), It.IsAny<Action<IMappingOperationOptions<DetailProviderResponse, ApprenticeshipDetailsViewModel>>>()), stubProviderViewModel)
                .WithUrl(urlHelperMock.Object);

            var result = controller.Detail(searchCriteria);

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            viewResult.Model.Should().Be(stubProviderViewModel);
        }
    }
}