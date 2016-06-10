using System;
using FluentAssertions;
using FluentAssertions.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    [TestFixture]
    public class SharedControllerTests
    {
        [Test]
        public void ShouldReturnAPartialViewResultWithCorrectViewNameForFooterAction()
        {
            const string SurveyUrl = "http://test.com/";
            var mockSettings = new Mock<IConfigurationSettings>();
            var mockCookieService = new Mock<ICookieService>();
            mockSettings.SetupGet(x => x.SurveyUrl).Returns(new Uri(SurveyUrl));
            var controller = new SharedController(mockSettings.Object, mockCookieService.Object);

            var result = controller.Footer();

            result.Should().BePartialViewResult().WithViewName("_footer");
        }

        [Test]
        public void ShouldSetSurveyUrlInModelForFooterAction()
        {
            const string SurveyUrl = "http://test.com/";
            var mockSettings = new Mock<IConfigurationSettings>();
            var mockCookieService = new Mock<ICookieService>();
            mockSettings.SetupGet(x => x.SurveyUrl).Returns(new Uri(SurveyUrl));
            var controller = new SharedController(mockSettings.Object, mockCookieService.Object);

            var result = controller.Footer();

            result.Should().BePartialViewResult().Model.Should().Be(SurveyUrl);
        }

        [Test]
        public void ShouldSetSurveyUrlInModelForHeaderAction()
        {
            const string SurveyUrl = "http://test.com/";
            var mockSettings = new Mock<IConfigurationSettings>();
            var mockCookieService = new Mock<ICookieService>();
            mockSettings.SetupGet(x => x.SurveyUrl).Returns(new Uri(SurveyUrl));
            var controller = new SharedController(mockSettings.Object, mockCookieService.Object);

            var result = controller.Header();

            result.Should().BePartialViewResult();
            var viewModel = (HeaderViewModel)result.Model;
            viewModel.SurveyUrl.Should().Be(SurveyUrl);
        }
    }
}