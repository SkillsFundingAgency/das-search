namespace Sfa.Eds.Das.Web.Tests.Controllers
{
    using System;
    using System.Web.Mvc;
    using Core.Interfaces.Search;
    using Core.Models;
    using Moq;
    using NUnit.Framework;

    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    using Web.Controllers;

    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(null, null);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Search()
        {
            // Arrange

            var mockSearchService = new Mock<ISearchService>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>())).Returns(new SearchResults());

            var moqMappingServices = new Mock<IMappingService>();
            moqMappingServices.Setup(
                x => x.Map<SearchResults, StandardSearchResultViewModel>(It.IsAny<SearchResults>()))
                .Returns(new StandardSearchResultViewModel());

            HomeController controller = new HomeController(mockSearchService.Object, moqMappingServices.Object);

            // Act
            ViewResult result = controller.Search(new SearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
