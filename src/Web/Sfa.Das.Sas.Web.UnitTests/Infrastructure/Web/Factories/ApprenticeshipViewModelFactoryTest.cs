namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Factories
{
    using System.Web.Mvc;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipViewModelFactoryTest
    {
        private Mock<UrlHelper> _mockUrlHelper;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<UrlHelper>();

            _mockUrlHelper.Setup(m => m.Action("StandardResults", "Provider")).Returns("/hello/standard");
            _mockUrlHelper.Setup(m => m.Action("Standard", "Apprenticeship", It.IsAny<object>())).Returns("/hello/StandardPrevLink/id");

            _mockUrlHelper.Setup(m => m.Action("FrameworkResults", "Provider")).Returns("/hello/framework");
            _mockUrlHelper.Setup(m => m.Action("Framework", "Apprenticeship", It.IsAny<object>())).Returns("/hello/FrameworkPrevLink/id");
        }
    }
}