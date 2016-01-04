using System.Web.Mvc;
using Sfa.Eds.Das.Web.Controllers;
using Sfa.Eds.Das.Web.Services;
using Moq;
using NUnit.Framework;

namespace Sfa.Eds.Das.Web.Tests.Controllers
{
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
            var mockSearchService = new Mock<ISearchForStandards>();

            // Arrange
            HomeController controller = new HomeController(mockSearchService.Object);

            // Act
            ViewResult result = controller.Search(new Models.SearchCriteria()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
