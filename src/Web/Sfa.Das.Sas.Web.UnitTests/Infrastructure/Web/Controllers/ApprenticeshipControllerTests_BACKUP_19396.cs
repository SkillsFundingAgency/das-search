﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.UnitTests.Extensions;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Controllers
{
    using System.Web.Routing;
    using FluentAssertions;
    using Sfa.Das.Sas.Web.Factories;

    [TestFixture]
    public sealed class ApprenticeshipControllerTests
    {
        [Test]
        public void Search_WhenNavigateTo_ShouldReturnAViewResult()
        {
            // Arrange
            ApprenticeshipController controller = new ApprenticeshipController(null, null, null, null, null, new Mock<IProfileAStep>().Object, null, null);

            // Act
            ViewResult result = controller.Search() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Search_WhenPassedAKeyword_ShouldReturnAViewResult()
        {
            // Arrange
            var mockSearchService = new Mock<IApprenticeshipSearchService>();
            var mockLogger = new Mock<ILog>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10, 0, null)).Returns(new ApprenticeshipSearchResults());

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultItemViewModel>(It.IsAny<ApprenticeshipSearchResults>()))
                .Returns(new ApprenticeshipSearchResultItemViewModel());

            ApprenticeshipController controller = new ApprenticeshipController(mockSearchService.Object, null, null, mockLogger.Object, mockMappingServices.Object, new Mock<IProfileAStep>().Object, null, null);

            // Act
            ViewResult result = controller.SearchResults(new ApprenticeshipSearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Search_WhenSearchResponseReturnsANull_ModelShouldContainTheSearchKeyword()
        {
            // Arrange
            var mockSearchService = new Mock<IApprenticeshipSearchService>();
            var mockLogger = new Mock<ILog>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10, 0, null)).Returns(value: null);

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(It.IsAny<ApprenticeshipSearchResults>()))
                .Returns(new ApprenticeshipSearchResultViewModel());

            ApprenticeshipController controller = new ApprenticeshipController(mockSearchService.Object, null, null, mockLogger.Object, mockMappingServices.Object, new Mock<IProfileAStep>().Object, null, null);

            // Act
            ViewResult result = controller.SearchResults(new ApprenticeshipSearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(null, ((ApprenticeshipSearchResultViewModel)result.Model).SearchTerm);
            Assert.IsNotNull(result);
        }

        [TestCase(-15)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Search_WhenCriteriaPage_IsLessOrEqualTo0(int input)
        {
            // Arrange
            var mockSearchService = new Mock<IApprenticeshipSearchService>();
            var mockLogger = new Mock<ILog>();

            var mockMappingServices = new Mock<IMappingService>();
            ApprenticeshipController controller = new ApprenticeshipController(mockSearchService.Object, null, null, mockLogger.Object, mockMappingServices.Object, new Mock<IProfileAStep>().Object, null, null);

            // Act
            ViewResult result = controller.SearchResults(new ApprenticeshipSearchCriteria { Keywords = "test", Page = input }) as ViewResult;

            // Assert
            mockSearchService.Verify(m => m.SearchByKeyword("test", 1, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()));
            Assert.IsNotNull(result);
        }

        [TestCase("true", true, Description = "Has error")]
        [TestCase("false", false, Description = "No error")]
        public void StandardDetailPageWithErrorParameter(string hasErrorParmeter, bool expected)
        {
            var mockStandardRepository = new Mock<IGetStandards>();

            var standard = new Standard { Title = "Hello", };
            mockStandardRepository.Setup(x => x.GetStandardById(It.IsAny<int>())).Returns(standard);
            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<Standard, StandardViewModel>(It.IsAny<Standard>()))
                .Returns(new StandardViewModel());

<<<<<<< HEAD
            var mockCookie = new Mock<IListCollection<int>>();
            mockCookie.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship> {new ShortlistedApprenticeship {ApprenticeshipId = 1}});

=======
>>>>>>> master
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(x => x.UrlReferrer).Returns(new Uri("http://www.abba.co.uk"));

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mockRequest.Object);

<<<<<<< HEAD
            ApprenticeshipController controller = new ApprenticeshipController(null, mockStandardRepository.Object, null, null, mockMappingServices.Object, new Mock<IProfileAStep>().Object, mockCookie.Object, null);
=======
            ApprenticeshipController controller = new ApprenticeshipController(null, mockStandardRepository.Object, null, null, mockMappingServices.Object, new Mock<IProfileAStep>().Object, new Mock<IListCollection<int>>().Object, null);
>>>>>>> master
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            controller.Url = new UrlHelper(
                new RequestContext(context.Object, new RouteData()),
                new RouteCollection());

<<<<<<< HEAD
            var result = controller.Standard(1, hasErrorParmeter) as ViewResult;
=======
            var result = controller.Standard(1, string.Empty) as ViewResult;
>>>>>>> master

            Assert.NotNull(result);
        }

        [Test]
        public void StandardDetailPageStandardIsNull()
        {
            var mockStandardRepository = new Mock<IGetStandards>();

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(x => x.UrlReferrer).Returns(new Uri("http://www.abba.co.uk"));
            var moqLogger = new Mock<ILog>();
            ApprenticeshipController controller = new ApprenticeshipController(null, mockStandardRepository.Object, null, moqLogger.Object, null, new Mock<IProfileAStep>().Object, null, null);

<<<<<<< HEAD
            HttpNotFoundResult result = (HttpNotFoundResult)controller.Standard(1, "false");
=======
            HttpNotFoundResult result = (HttpNotFoundResult)controller.Standard(1, string.Empty);
>>>>>>> master

            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Cannot find standard: 1", result.StatusDescription);
            moqLogger.Verify(m => m.Warn("404 - Cannot find standard: 1"));
        }

        [Test]
        public void FrameworkDetailPageStandardIsNull()
        {
            var mockFrameworkRepository = new Mock<IGetFrameworks>();

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(x => x.UrlReferrer).Returns(new Uri("http://www.abba.co.uk"));
            var moqLogger = new Mock<ILog>();
            ApprenticeshipController controller = new ApprenticeshipController(null, null, mockFrameworkRepository.Object,  moqLogger.Object, null, new Mock<IProfileAStep>().Object, null, null);

            HttpNotFoundResult result = (HttpNotFoundResult)controller.Framework(1);

            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Cannot find framework: 1", result.StatusDescription);
            moqLogger.Verify(m => m.Warn("404 - Cannot find framework: 1"));
        }

        /*[Test]
        [TestCase(testCase1,
            2, 
            true, 
            Description = "Shortlisted")]
        [TestCase(new[] { 1, 3 }, 2, false, Description = "Not Shortlisted")]
        public void ShouldSetViewModelShortListValueToTrueIfStandardIsInShortList(
            IEnumerable<ShortlistedApprenticeship> shortListItems,
            int standardId,
            bool expectedResult)
        {
            // Arrange
            var mockStandardRepository = new Mock<IGetStandards>();
            mockStandardRepository.Setup(x => x.GetStandardById(It.IsAny<int>())).Returns(new Standard());

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(x => x.Map<Standard, StandardViewModel>(It.IsAny<Standard>()))
                                            .Returns(new StandardViewModel());

            var mockCookieRepository = new Mock<IListCollection<int>>();
            var controller = new ApprenticeshipController(
                null,
                mockStandardRepository.Object,
                null,
                null,
                mockMappingServices.Object,
                new Mock<IProfileAStep>().Object,
                mockCookieRepository.Object,
                null);

            controller.SetRequestUrl("http://www.abba.co.uk");
            mockCookieRepository.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new List<ShortlistedApprenticeship>(shortListItems));

            // Act
            var result = controller.Standard(standardId, string.Empty) as ViewResult;
            var viewModel = result?.Model as StandardViewModel;

            // Assert
            Assert.IsNotNull(viewModel);
            mockCookieRepository.Verify(x => x.GetAllItems(Constants.StandardsShortListCookieName));
            Assert.AreEqual(expectedResult, viewModel.IsShortlisted);
        }*/

        [Test(Description = "should create vie model for standard when standardid parameter is specified ")]
        public void SearchForProvidersActionWithStandardIdParameter()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            controller.SearchForProviders(1, null, null);

            mockApprenticeshipViewModelFactory.Verify(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Once);
            mockApprenticeshipViewModelFactory.Verify(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Never);
        }

        [Test(Description = "should create a viewmodel for frameworks when frameworkid parameter is specified ")]
        public void SearchForProvidersActionWithFrameworIdParameter()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            controller.SearchForProviders(null, 12, null);

            mockApprenticeshipViewModelFactory.Verify(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Once);
            mockApprenticeshipViewModelFactory.Verify(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Never);
        }

        [Test(Description = "should create a viewmodel with error true ")]
        public void SearchForProvidersWithErrors()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            var result = controller.SearchForProviders(1, null, "true") as ViewResult;
            var viewModel = result?.Model as ProviderSearchViewModel;
            viewModel?.HasError.Should().Be(true);
        }

        [Test(Description = "should create a viewmodel with error false")]
        public void SearchForProvidersWithNoErrors()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            var result = controller.SearchForProviders(1, null, null) as ViewResult;
            var viewModel = result?.Model as ProviderSearchViewModel;
            viewModel?.HasError.Should().BeFalse();
        }

        [Test]
        public void WhenNoValidValuesAreProvided()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            var result = controller.SearchForProviders(null, null, null) as HttpStatusCodeResult;
            result.StatusCode.Should().Be(400);
        }

        [Test(Description = "should create vie model for standard when standardid parameter is specified ")]
        public void SearchForProvidersActionWithStandardIdParameter()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            controller.SearchForProviders(1, null, null);

            mockApprenticeshipViewModelFactory.Verify(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Once);
            mockApprenticeshipViewModelFactory.Verify(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Never);
        }

        [Test(Description = "should create a viewmodel for frameworks when frameworkid parameter is specified ")]
        public void SearchForProvidersActionWithFrameworIdParameter()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            controller.SearchForProviders(null, 12, null);

            mockApprenticeshipViewModelFactory.Verify(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Once);
            mockApprenticeshipViewModelFactory.Verify(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>()), Times.Never);
        }

        [Test(Description = "should create a viewmodel with error true ")]
        public void SearchForProvidersWithErrors()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            var result = controller.SearchForProviders(1, null, "true") as ViewResult;
            var viewModel = result?.Model as ProviderSearchViewModel;
            viewModel?.HasError.Should().Be(true);
        }

        [Test(Description = "should create a viewmodel with error false")]
        public void SearchForProvidersWithNoErrors()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetStandardViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            var result = controller.SearchForProviders(1, null, null) as ViewResult;
            var viewModel = result?.Model as ProviderSearchViewModel;
            viewModel?.HasError.Should().BeFalse();
        }

        [Test]
        public void WhenNoValidValuesAreProvided()
        {
            var mockApprenticeshipViewModelFactory = new Mock<IApprenticeshipViewModelFactory>();
            mockApprenticeshipViewModelFactory.Setup(m => m.GetFrameworkProvidersViewModel(It.IsAny<int>(), It.IsAny<UrlHelper>())).Returns(new ProviderSearchViewModel());
            var controller = new ApprenticeshipController(null, null, null, null, null, null, null, mockApprenticeshipViewModelFactory.Object);

            var result = controller.SearchForProviders(null, null, null) as HttpStatusCodeResult;
            result.StatusCode.Should().Be(400);
        }
    }
}
