using System.Web.Mvc;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Controllers;

namespace Sfa.Das.Sas.Web.UnitTests.Controllers
{
    [TestFixture]
    public sealed class StartControllerTests
    {
        [Test]
        public void Start_WhenNavigateTo_ShouldReturnAViewResult()
        {
            // Arrange
            StartController controller = new StartController();

            // Act
            ViewResult result = controller.Start() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
