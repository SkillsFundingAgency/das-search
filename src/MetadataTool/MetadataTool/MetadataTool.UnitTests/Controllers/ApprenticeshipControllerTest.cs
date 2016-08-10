using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetadataTool;
using MetadataTool.Controllers;
using MetadataTool.Web.Controllers;

namespace MetadataTool.UnitTests.Controllers
{
    [TestClass]
    public class ApprenticeshipControllerTest
    {
        [TestMethod]
        public void Standards()
        {
            // Arrange
            ApprenticeshipController controller = new ApprenticeshipController();

            // Act
            ViewResult result = controller.Standards() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Frameworks()
        {
            // Arrange
            ApprenticeshipController controller = new ApprenticeshipController();

            // Act
            ViewResult result = controller.Frameworks() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
