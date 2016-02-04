namespace Sfa.Eds.Das.Web.Tests.Controllers
{
    using System.Web.Mvc;
    using Core.Interfaces.Search;
    using Core.Models;
    using Moq;
    using NUnit.Framework;
    using Web.Controllers;

    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Search()
        {
            var mockSearchService = new Mock<ISearchService>();

            // Arrange
            HomeController controller = new HomeController(mockSearchService.Object);

            // Act
            ViewResult result = controller.Search(new SearchCriteria()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
