namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    using System.Web.Mvc;
    using Core.Configuration;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Controllers;
    using SFA.DAS.NLog.Logger;

    [TestFixture]
    public sealed class StartControllerTests
    {
        private Mock<IConfigurationSettings> _mockConfiguration;
        [Test]
        public void Start_WhenNavigateTo_ShouldReturnAViewResult()
        {
            _mockConfiguration = new Mock<IConfigurationSettings>();

            // Arrange
            StartController controller = new StartController(_mockConfiguration.Object, new Mock<ILog>().Object);

            // Act
            ViewResult result = controller.Start() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
