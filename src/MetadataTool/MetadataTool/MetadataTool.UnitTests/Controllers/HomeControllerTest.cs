using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetadataTool;
using MetadataTool.Controllers;
using MetadataTool.Web.Controllers;

namespace MetadataTool.UnitTests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
