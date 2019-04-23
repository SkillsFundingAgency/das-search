using System.Collections.Generic;
using System.Threading;
using NSubstitute;
using Sfa.Das.Sas.Core.Configuration;
using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ProviderControllerTest
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using ApplicationServices.Queries;
    using ApplicationServices.Responses;
    using AutoMapper;
    using ControllerBuilders;
    using FluentAssertions;
    using MediatR;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Controllers;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using ViewModels;

    [TestFixture]
    public class ProviderControllerTests
    {
        [Test]
        public async Task DetailShouldReturnNotFoundResultIfApprenticeshipProviderNotFound()
        {
            var searchCriteria = new ApprenticeshipProviderDetailQuery();
            var stubSearchResponse = Task.FromResult(new ApprenticeshipProviderDetailResponse { StatusCode = ApprenticeshipProviderDetailResponse.ResponseCodes.ApprenticeshipProviderNotFound });

            ProviderController controller = new ProviderControllerBuilder()
                .SetupMediator(x => x.Send(It.IsAny<ApprenticeshipProviderDetailQuery>(), default(CancellationToken)), stubSearchResponse);

            var result = controller.Detail(searchCriteria).Result;

            result.Should().BeOfType<HttpStatusCodeResult>();

            var responseResult = (HttpStatusCodeResult)result;

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void ProviderDetailsShouldResultExpectedViewModel()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator.Setup(x => x.Send(It.IsAny<ProviderDetailQuery>(), new CancellationToken()))
                .ReturnsAsync(new ProviderDetailResponse
                {
                    Provider = new Provider(),
                    StatusCode = ProviderDetailResponse.ResponseCodes.Success
                });

            var config = new Mock<IConfigurationSettings>();
            config.Setup(m => m.HideAboutProviderForUkprns)
                    .Returns(new List<long>());

            var providerController = new ProviderController(null, null, mockMediator.Object, config.Object);
            var result = providerController.ProviderDetail(It.IsAny<long>()).Result;
            result.Should().BeOfType<ViewResult>();

            var returnedModel = ((ViewResult)result).Model as ProviderDetailViewModel;

            returnedModel.Should().BeOfType<ProviderDetailViewModel>();
        }

        [Test]
        public void DetailShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            var searchCriteria = new ApprenticeshipProviderDetailQuery { StandardCode = "1", LocationId = 2, UkPrn = 3 };

            var stubSearchResponse = Task.FromResult(new ApprenticeshipProviderDetailResponse());

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
                .SetupMediator((x => x.Send(It.IsAny<ApprenticeshipProviderDetailQuery>(), default(CancellationToken))), stubSearchResponse)
                .SetupMappingService(x => x.Map(It.IsAny<ApprenticeshipProviderDetailResponse>(), It.IsAny<Action<IMappingOperationOptions<ApprenticeshipProviderDetailResponse, ApprenticeshipDetailsViewModel>>>()), stubProviderViewModel)
                .WithUrl(urlHelperMock.Object);

            var result = controller.Detail(searchCriteria).Result;

            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult)result;

            viewResult.Model.Should().Be(stubProviderViewModel);
        }
    }
}