using System;
using System.Web;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    [TestFixture]
    public class ButtonTextServiceTests
    {
        private Mock<HttpContextBase> _httpContext;
        private ButtonTextService _buttonTextService;
        private Mock<HttpRequestBase> _httpRequestBase;

        [SetUp]
        public void SetUp()
        {

            _buttonTextService = new ButtonTextService();
            _httpRequestBase = new Mock<HttpRequestBase>();
            _httpContext = new Mock<HttpContextBase>();
            _httpContext.Setup(m => m.Request).Returns(_httpRequestBase.Object);
        }

        [Test]
        public void ShouldSetDefaultTextIfNoReferrerUrl()
        {
            var buttonText = _buttonTextService.GetFindTrainingProvidersText( _httpContext.Object);
            buttonText.Should().Be("Find training providers");
        }

        [TestCase("", "Find training providers")]
        [TestCase("notThePath",  "Find training providers")]
        [TestCase( "provider/", "Find more training providers")]
        [TestCase( "Provider/",  "Find more training providers")]
        public void ShouldSetExpectedTextBasedOnAbsolutePath(string referringUrlAbsolutePath, string expectedText)
        {
            _httpContext.Setup(x => x.Request.UrlReferrer).Returns(new Uri($"http://www.test.com/{referringUrlAbsolutePath}"));
            var buttonText = _buttonTextService.GetFindTrainingProvidersText(httpContext: _httpContext.Object);
            buttonText.Should().Be(expectedText);
        }
    }
}
