using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ProviderControllerTest
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using ApplicationServices.Models;
    using ApplicationServices.Queries;
    using ApplicationServices.Responses;
    using AutoMapper;
    using ControllerBuilders;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Controllers;
    using Sas.Web.Services;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using ViewModels;

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
        public void ProviderDetailsShouldResultExpectedViewModel()
        {
            var mockProviderService = new Mock<IProviderService>();
            var aliases = new List<string> {"item 1", "Another Item", "A different item"};
            const string tradingNames = "item 1, Another Item, A different item";
            const string phone = "123-456";
            const string providerName = "Joe The Plumbers";
            const string email = "test@test.com";
            const long ukPrn = 2221221;
            const string uri = "http://test.com/1234";
            const string website = "http://test.com";
            const double noSatisfactionScore = 0;
            const string noSatisfactionScoreMessage = "no data available";
            const double satisfactionScore = 15.9;
            const string satisfactionScoreMessage = "15.9%";

            var provider = new Provider
            {
                Aliases = aliases,
                EmployerSatisfaction = noSatisfactionScore,
                LearnerSatisfaction = satisfactionScore,
                Email = email,
                IsEmployerProvider = true,
                IsHigherEducationInstitute = false,
                NationalProvider = true,
                Phone = phone,
                Ukprn = ukPrn,
                ProviderName = providerName,
                Uri = uri,
                Website = website

            };
            var expectedProviderDetailViewModel = new ProviderDetailsViewModel
            {
                TradingNames = tradingNames,
                EmployerSatisfaction = noSatisfactionScore,
                EmployerSatisfactionMessage = noSatisfactionScoreMessage,
                LearnerSatisfaction = satisfactionScore,
                LearnerSatisfactionMessage = satisfactionScoreMessage,
                Email = email,
                IsEmployerProvider = true,
                IsHigherEducationInstitute = false,
                NationalProvider = true,
                Phone = phone,
                Ukprn = ukPrn,
                ProviderName = providerName,
                Uri = uri,
                Website = website
            };

            mockProviderService.Setup(x => x.GetProviderDetails(ukPrn))
                .Returns(
                    provider);

            var providerController = new ProviderController(null, null, null, null, mockProviderService.Object);
            var result = providerController.ProviderDetails(ukPrn);
            result.Should().BeOfType<ViewResult>();

            var viewResult = (ViewResult) result;

            var returnedModel = (ProviderDetailsViewModel)viewResult.Model;

            returnedModel.TradingNames.Should().Be(expectedProviderDetailViewModel.TradingNames);
            returnedModel.Email.Should().Be(expectedProviderDetailViewModel.Email);
            returnedModel.EmployerSatisfaction.Should().Be(expectedProviderDetailViewModel.EmployerSatisfaction);
            returnedModel.EmployerSatisfactionMessage.Should().Be(expectedProviderDetailViewModel.EmployerSatisfactionMessage);
            returnedModel.LearnerSatisfaction.Should().Be(expectedProviderDetailViewModel.LearnerSatisfaction);
            returnedModel.LearnerSatisfactionMessage.Should().Be(expectedProviderDetailViewModel.LearnerSatisfactionMessage);
            returnedModel.IsEmployerProvider.Should().Be(expectedProviderDetailViewModel.IsEmployerProvider);
            returnedModel.IsHigherEducationInstitute.Should().Be(expectedProviderDetailViewModel.IsHigherEducationInstitute);
            returnedModel.NationalProvider.Should().Be(expectedProviderDetailViewModel.NationalProvider);
            returnedModel.Phone.Should().Be(expectedProviderDetailViewModel.Phone);
            returnedModel.ProviderName.Should().Be(expectedProviderDetailViewModel.ProviderName);
            returnedModel.Ukprn.Should().Be(expectedProviderDetailViewModel.Ukprn);
            returnedModel.Uri.Should().Be(expectedProviderDetailViewModel.Uri);
            returnedModel.Website.Should().Be(expectedProviderDetailViewModel.Website);
          }

        [Test]
        public void DetailShouldReturnViewResultWhenStandardSearchIsSuccessful()
        {
            var searchCriteria = new ProviderDetailQuery { StandardCode = "1", LocationId = 2, Ukprn = 3 };

            var stubSearchResponse = new DetailProviderResponse();

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