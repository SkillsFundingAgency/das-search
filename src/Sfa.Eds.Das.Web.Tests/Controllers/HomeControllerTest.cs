using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfa.Eds.Das.Web.Controllers;
using Sfa.Eds.Das.Web.Services;
using Moq;

namespace Sfa.Eds.Das.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
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
