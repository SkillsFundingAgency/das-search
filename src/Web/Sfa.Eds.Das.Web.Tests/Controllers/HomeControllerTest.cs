namespace Sfa.Eds.Das.Web.Tests.Controllers
{
    using System.Web.Mvc;
    using Core.Interfaces.Search;
    using Core.Models;
    using Moq;
    using NUnit.Framework;
    using ViewModels;
    using Web.Controllers;
    using Web.Services;

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
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10)).Returns(new SearchResults());

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

        [Test]
        public void WhenSearchResultIsNull()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10)).Returns(value: null);

            var moqMappingServices = new Mock<IMappingService>();
            moqMappingServices.Setup(
                x => x.Map<SearchResults, StandardSearchResultViewModel>(It.IsAny<SearchResults>()))
                .Returns(new StandardSearchResultViewModel());

            HomeController controller = new HomeController(mockSearchService.Object, moqMappingServices.Object);

            // Act
            ViewResult result = controller.Search(new SearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(null, ((StandardSearchResultViewModel)result.Model).SearchTerm);
            Assert.IsNotNull(result);
        }
    }
}
