namespace Sfa.Das.Sas.Web.UnitTests.Services
{
    using System;
    using System.Web;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Sfa.Das.Sas.Core.Configuration;
    using Sfa.Das.Sas.Web.Services;

    [TestFixture]
    public class ForCookieService
    {
        private readonly string _testCookieName = "test_cookie_banner_name";

        private CookieService _cookieService;

        private HttpCookieCollection _cookieCollection;

        private Mock<HttpContextBase> _httpContext;

        [SetUp]
        public void SetUp()
        {
            var mockSettings = new Mock<IConfigurationSettings>();
            mockSettings.Setup(m => m.CookieInfoBannerCookieName).Returns(_testCookieName);
            _cookieService = new CookieService(mockSettings.Object);
            _cookieCollection = new HttpCookieCollection();

            var httpRequestBase = new Mock<HttpRequestBase>();
            var httpResponseBase = new Mock<HttpResponseBase>();
            httpRequestBase.Setup(m => m.Cookies).Returns(_cookieCollection);
            httpResponseBase.Setup(m => m.Cookies).Returns(_cookieCollection);

            _httpContext = new Mock<HttpContextBase>();
            _httpContext.Setup(m => m.Request).Returns(httpRequestBase.Object);
            _httpContext.Setup(m => m.Response).Returns(httpResponseBase.Object);

            _cookieCollection.Get(_testCookieName).Should().BeNull();
        }

        [Test]
        public void ShouldSetCookieForBanner()
        {
            var response = _cookieService.ShowCookieForBanner(_httpContext.Object);

            var cookie = _cookieCollection.Get(_testCookieName);
            _cookieCollection.Get(_testCookieName).Should().NotBeNull();

            response.Should().BeTrue();

            cookie.Expires.Should().BeAfter(DateTime.UtcNow.AddDays(30).AddMinutes(-1), because: "Expire date needs to be 30 days from now");
            cookie.Expires.Should().BeBefore(DateTime.UtcNow.AddDays(30).AddMinutes(1), because: "Expire date needs to be 30 days from now");
        }

        [Test]
        public void ShouldNotDisplayBannerIfCookieIsPresent()
        {
            _cookieCollection.Add(new HttpCookie(_testCookieName));
            var response = _cookieService.ShowCookieForBanner(_httpContext.Object);

            response.Should().BeFalse();
            _cookieCollection.AllKeys.Length.Should().Be(1);
        }

        [Test]
        public void ShouldNotDisplayBannerIfHttpContextIsNull()
        {
            var response = _cookieService.ShowCookieForBanner(null);

            response.Should().BeFalse();
            _cookieCollection.AllKeys.Length.Should().Be(0);
        }
    }
}
