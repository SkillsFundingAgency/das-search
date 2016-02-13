namespace Sfa.Eds.Das.Web.UnitTests.Controllers
{
    using System.Web.Mvc;

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
    public class StandardControllerTest
    {
        [Test]
        public void Search_WhenPassedAKeyword_ShouldReturnAViewResult()
        {
            // Arrange
            var mockSearchService = new Mock<IStandardSearchService>();
            var mockLogger = new Mock<ILog>();
            mockSearchService.Setup(x => x.SearchByKeyword(It.IsAny<string>(), 0, 10)).Returns(new StandardSearchResults());

            var mockMappingServices = new Mock<IMappingService>();
            mockMappingServices.Setup(
                x => x.Map<StandardSearchResults, StandardSearchResultViewModel>(It.IsAny<StandardSearchResults>()))
                .Returns(new StandardSearchResultViewModel());

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
                x => x.Map<StandardSearchResults, StandardSearchResultViewModel>(It.IsAny<StandardSearchResults>()))
                .Returns(new StandardSearchResultViewModel());

            StandardController controller = new StandardController(mockSearchService.Object, null, mockLogger.Object, mockMappingServices.Object);

            // Act
            ViewResult result = controller.SearchResults(new StandardSearchCriteria { Keywords = "test" }) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(null, ((StandardSearchResultViewModel)result.Model).SearchTerm);
            Assert.IsNotNull(result);
        }
    }
}
