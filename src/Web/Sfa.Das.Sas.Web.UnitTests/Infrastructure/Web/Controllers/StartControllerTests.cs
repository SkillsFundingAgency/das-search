using System.Web.Mvc;
using AngleSharp;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Web.Controllers;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    [TestFixture]
    public sealed class StartControllerTests
    {
        private Mock<IConfigurationSettings> _mockConfiguration;
        [Test]
        public void Start_WhenNavigateTo_ShouldReturnAViewResult()
        {
            // Arrange
            StartController controller = new StartController(_mockConfiguration.Object);

            // Act
            ViewResult result = controller.Start() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
