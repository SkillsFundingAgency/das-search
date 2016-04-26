namespace Sfa.Eds.Das.Web.UnitTests.Controllers
{
    using System;

    using FluentAssertions;
    using FluentAssertions.Mvc;
    using Moq;
    using NUnit.Framework;

    using Sfa.Eds.Das.Core.Configuration;
    using Sfa.Eds.Das.Web.Controllers;

    [TestFixture]
    public class SharedControllerTests
    {
        [Test]
        public void ShouldReturnAPartialViewResultWithCorrectViewNameForFooterAction()
        {
            const string SurveyUrl = "http://test.com/";
            var mockSettings = new Mock<IConfigurationSettings>();
            mockSettings.SetupGet(x => x.SurveyUrl).Returns(new Uri(SurveyUrl));
            var controller = new SharedController(mockSettings.Object);

            var result = controller.Footer();

            result.Should().BePartialViewResult().WithViewName("_footer");
        }

        [Test]
        public void ShouldSetSurveyUrlInModelForFooterAction()
        {
            const string SurveyUrl = "http://test.com/";
            var mockSettings = new Mock<IConfigurationSettings>();
            mockSettings.SetupGet(x => x.SurveyUrl).Returns(new Uri(SurveyUrl));
            var controller = new SharedController(mockSettings.Object);

            var result = controller.Footer();

            result.Should().BePartialViewResult().Model.Should().Be(SurveyUrl);
        }

        [Test]
        public void ShouldSetSurveyUrlInModelForHeaderAction()
        {
            const string SurveyUrl = "http://test.com/";
            var mockSettings = new Mock<IConfigurationSettings>();
            mockSettings.SetupGet(x => x.SurveyUrl).Returns(new Uri(SurveyUrl));
            var controller = new SharedController(mockSettings.Object);

            var result = controller.Header();

            result.Should().BePartialViewResult().Model.Should().Be(SurveyUrl);
        }
    }
}