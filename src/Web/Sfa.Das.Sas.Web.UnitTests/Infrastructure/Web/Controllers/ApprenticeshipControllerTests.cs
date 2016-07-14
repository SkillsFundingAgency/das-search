using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    [TestFixture]
    public sealed class ApprenticeshipControllerTests
    {
        [Test]
        public void Search_WhenNavigateTo_ShouldReturnAViewResult()
        {
            // Arrange
            ApprenticeshipController controller = new ApprenticeshipController(null, null, null);

            // Act
            ViewResult result = controller.Search() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        // TODO: Fix these tests

        //[Test]
        //public void Search_WhenPassedAKeyword_ShouldReturnAViewResult()
        //{
        //    // Arrange
        //    var mockLogger = new Mock<ILog>();
        //    var mockFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    var mockMediator = new Mock<IMediator>();
        //    ApprenticeshipController controller = new ApprenticeshipController(mockLogger.Object, mockFactory.Object, mockMediator.Object);

        //    // Act
        //    ViewResult result = controller.SearchResults(new ApprenticeshipSearchQuery { Keywords = "test" }) as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Search_WhenSearchResponseReturnsANull_ModelShouldContainTheSearchKeyword()
        //{
        //    // Arrange
        //    var mockSearchService = new Mock<IApprenticeshipSearchService>();
        //    var mockLogger = new Mock<ILog>();
        //    mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10, 0, null)).Returns(value: null);

        //    var mockFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    ApprenticeshipController controller = new ApprenticeshipController(mockSearchService.Object, mockLogger.Object, null, mockFactory.Object);

        //    // Act
        //    ViewResult result = controller.SearchResults(new ApprenticeshipSearchQuery { Keywords = "test" }) as ViewResult;

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.AreEqual(null, ((ApprenticeshipSearchResultViewModel)result.Model).SearchTerm);
        //    Assert.IsNotNull(result);
        //}

        //[TestCase(-15)]
        //[TestCase(-1)]
        //[TestCase(0)]
        //public void Search_WhenCriteriaPage_IsLessOrEqualTo0(int input)
        //{
        //    // Arrange
        //    var mockLogger = new Mock<ILog>();
        //    var mockFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    var mockMediator = new Mock<IMediator>();

        //    ApprenticeshipController controller = new ApprenticeshipController(mockLogger.Object, mockFactory.Object, mockMediator.Object);

        //    // Act
        //    ViewResult result = controller.SearchResults(new ApprenticeshipSearchQuery { Keywords = "test", Page = input }) as ViewResult;

        //    // Assert
        //    //mockSearchService.Verify(m => m.SearchByKeyword("test", 1, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()));
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void StandardDetailPageStandardIsNull()
        //{
        //    var mockApprenticeshipFactory = new Mock<IApprenticeshipViewModelFactory>();

        //    var mockRequest = new Mock<HttpRequestBase>();
        //    mockRequest.Setup(x => x.UrlReferrer).Returns(new Uri("http://www.abba.co.uk"));
        //    var moqLogger = new Mock<ILog>();
        //    ApprenticeshipController controller = new ApprenticeshipController(null, moqLogger.Object, null, mockApprenticeshipFactory.Object);

        //    HttpNotFoundResult result = (HttpNotFoundResult)controller.Standard(1, string.Empty);

        //    Assert.NotNull(result);
        //    Assert.AreEqual(404, result.StatusCode);
        //    Assert.AreEqual("Cannot find standard: 1", result.StatusDescription);
        //    moqLogger.Verify(m => m.Warn("404 - Cannot find standard: 1"));
        //}

        //[Test]
        //public void FrameworkDetailPageStandardIsNull()
        //{
        //    var mockApprenticeshipFactory = new Mock<IApprenticeshipViewModelFactory>();

        //    var mockRequest = new Mock<HttpRequestBase>();
        //    mockRequest.Setup(x => x.UrlReferrer).Returns(new Uri("http://www.abba.co.uk"));
        //    var moqLogger = new Mock<ILog>();
        //    ApprenticeshipController controller = new ApprenticeshipController(null, moqLogger.Object, null, mockApprenticeshipFactory.Object);

        //    HttpNotFoundResult result = (HttpNotFoundResult)controller.Framework(1, string.Empty);

        //    Assert.NotNull(result);
        //    Assert.AreEqual(404, result.StatusCode);
        //    Assert.AreEqual("Cannot find framework: 1", result.StatusDescription);
        //    moqLogger.Verify(m => m.Warn("404 - Cannot find framework: 1"));
        //}

        //[Test(Description = "should create vie model for standard when standardid parameter is specified ")]
        //public void SearchForProvidersActionWithStandardIdParameter()
        //{
        //    var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    mockApprenticeshipViewModelFactory.Setup(m => m.GetProviderSearchViewModelForStandard(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
        //    var controller = new ApprenticeshipController(null, null, null, mockApprenticeshipViewModelFactory.Object);

        //    controller.SearchForProviders(1, null, string.Empty, string.Empty, null);

        //    mockApprenticeshipViewModelFactory.Verify(m => m.GetProviderSearchViewModelForStandard(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Once);
        //    mockApprenticeshipViewModelFactory.Verify(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Never);
        //}

        //[Test(Description = "should create a viewmodel for frameworks when frameworkid parameter is specified ")]
        //public void SearchForProvidersActionWithFrameworIdParameter()
        //{
        //    var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    mockApprenticeshipViewModelFactory.Setup(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
        //    var controller = new ApprenticeshipController(null, null, null, mockApprenticeshipViewModelFactory.Object);

        //    controller.SearchForProviders(null, 12, string.Empty, string.Empty, null);

        //    mockApprenticeshipViewModelFactory.Verify(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Once);
        //    mockApprenticeshipViewModelFactory.Verify(m => m.GetProviderSearchViewModelForStandard(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Never);
        //}

        //[Test(Description = "should create a viewmodel with error true ")]
        //public void SearchForProvidersWithErrors()
        //{
        //    var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    mockApprenticeshipViewModelFactory.Setup(m => m.GetProviderSearchViewModelForStandard(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
        //    var controller = new ApprenticeshipController(null, null, null, mockApprenticeshipViewModelFactory.Object);

        //    var result = controller.SearchForProviders(1, null, string.Empty, string.Empty, "true") as ViewResult;
        //    var viewModel = result?.Model as ProviderSearchViewModel;
        //    viewModel?.HasError.Should().Be(true);
        //}

        //[Test(Description = "should create a viewmodel with error false")]
        //public void SearchForProvidersWithNoErrors()
        //{
        //    var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    mockApprenticeshipViewModelFactory.Setup(m => m.GetProviderSearchViewModelForStandard(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
        //    var controller = new ApprenticeshipController(null, null, null, mockApprenticeshipViewModelFactory.Object);

        //    var result = controller.SearchForProviders(1, null, string.Empty, string.Empty, null) as ViewResult;
        //    var viewModel = result?.Model as ProviderSearchViewModel;
        //    viewModel?.HasError.Should().BeFalse();
        //}

        //[Test]
        //public void WhenNoValidValuesAreProvided()
        //{
        //    var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
        //    mockApprenticeshipViewModelFactory.Setup(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
        //    var controller = new ApprenticeshipController(null, null, null, mockApprenticeshipViewModelFactory.Object);

        //    var result = controller.SearchForProviders(null, null, null, null, null) as HttpStatusCodeResult;
        //    result.StatusCode.Should().Be(400);
        //}
    }
}
