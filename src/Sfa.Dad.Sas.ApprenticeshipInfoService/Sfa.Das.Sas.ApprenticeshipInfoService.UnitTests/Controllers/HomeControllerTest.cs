using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfa.Das.Sas.ApprenticeshipInfoService.Web;
using Sfa.Das.Sas.ApprenticeshipInfoService.Web.Controllers;

namespace Sfa.Das.Sas.ApprenticeshipInfoService.UnitTests.Controllers
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
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
