namespace Sfa.Eds.Das.Web.UnitTests.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Core.Domain.Model;
    using Core.Domain.Services;
    using Core.Logging;
    using Models;
    using Moq;
    using NUnit.Framework;
    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using ViewModels;
    using Web.Controllers;
    using Web.Services;

    [TestFixture]
    public sealed class StandardControllerTest
    {
        [Test]
        public void Search_WhenPassedAKeyword_ShouldReturnAViewResult()
        {
            // Arrange
            var mockSearchService = new Mock<IStandardSearchService>();
            var mockLogger = new Mock<ILog>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10)).Returns(new ApprenticeshipSearchResults());

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultItemViewModel>(It.IsAny<ApprenticeshipSearchResults>()))
                .Returns(new ApprenticeshipSearchResultItemViewModel());

            StandardController controller = new StandardController(mockSearchService.Object, null, mockLogger.Object, mockMappingServices.Object);

            // Act
            ViewResult result = controller.SearchResults(new StandardSearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Search_WhenSearchResponseReturnsANull_ModelShouldContainTheSearchKeyword()
        {
            // Arrange
            var mockSearchService = new Mock<IStandardSearchService>();
            var mockLogger = new Mock<ILog>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10)).Returns(value: null);

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(It.IsAny<ApprenticeshipSearchResults>()))
                .Returns(new ApprenticeshipSearchResultViewModel());

            StandardController controller = new StandardController(mockSearchService.Object, null, mockLogger.Object, mockMappingServices.Object);

            // Act
            ViewResult result = controller.SearchResults(new StandardSearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(null, ((ApprenticeshipSearchResultViewModel)result.Model).SearchTerm);
            Assert.IsNotNull(result);
        }

        [TestCase("true", true, Description = "Has error")]
        [TestCase("false", false, Description = "No error")]
        public void DetailPageWithErrorParameter(string hasErrorParmeter, bool expected)
        {
            var mockSearchRepository = new Mock<IStandardRepository>();

            var standard = new Standard { Title = "Hello", };
            mockSearchRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(standard);
            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<Standard, StandardViewModel>(It.IsAny<Standard>()))
                .Returns(new StandardViewModel());

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(x => x.UrlReferrer).Returns(new Uri("http://www.abba.co.uk"));

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mockRequest.Object);

            StandardController controller = new StandardController(null, mockSearchRepository.Object, null, mockMappingServices.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            controller.Url = new UrlHelper(
                new RequestContext(context.Object, new RouteData()),
                new RouteCollection());

            var result = controller.Detail(1, hasErrorParmeter) as ViewResult;

            Assert.NotNull(result);
            var actual = ((StandardViewModel)result.Model).HasError;
            Assert.AreEqual(expected, actual);
        }
    }
}
