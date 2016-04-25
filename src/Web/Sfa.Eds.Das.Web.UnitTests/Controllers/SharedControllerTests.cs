using System;
using FluentAssertions;
using FluentAssertions.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Core.Configuration;

namespace Sfa.Eds.Das.Web.Controllers.Tests
{
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
    }
}