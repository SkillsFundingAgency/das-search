using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetadataTool;

namespace Sfa.Das.Sas.MetadataTool.UnitTests.Controllers
{
    using Sfa.Das.Sas.MetadataTool.Web.Controllers;

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
    }
}
