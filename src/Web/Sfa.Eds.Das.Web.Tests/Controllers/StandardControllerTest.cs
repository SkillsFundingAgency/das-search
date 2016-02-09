namespace Sfa.Eds.Das.Web.Tests.Controllers
{
    using System.Web.Mvc;
    using Core.Interfaces.Search;
    using Core.Models;
    using log4net;
    using Moq;
    using NUnit.Framework;
    using ViewModels;
    using Web.Controllers;
    using Web.Services;

    [TestFixture]
    public class StandardControllerTest
    {
        [Test]
        public void Search_WhenPassedAKeyword_ShouldReturnAViewResult()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            var mockLogger = new Mock<ILog>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10)).Returns(new SearchResults());

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<SearchResults, StandardSearchResultViewModel>(It.IsAny<SearchResults>()))
                .Returns(new StandardSearchResultViewModel());

            StandardController controller = new StandardController(mockSearchService.Object, mockLogger.Object, mockMappingServices.Object);

            // Act
            ViewResult result = controller.SearchResults(new SearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Search_WhenSearchResponseReturnsANull_ModelShouldContainTheSearchKeyword()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            var mockLogger = new Mock<ILog>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10)).Returns(value: null);

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<SearchResults, StandardSearchResultViewModel>(It.IsAny<SearchResults>()))
                .Returns(new StandardSearchResultViewModel());

            StandardController controller = new StandardController(mockSearchService.Object, mockLogger.Object, mockMappingServices.Object);

            // Act
            ViewResult result = controller.SearchResults(new SearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(null, ((StandardSearchResultViewModel)result.Model).SearchTerm);
            Assert.IsNotNull(result);
        }
    }
}
