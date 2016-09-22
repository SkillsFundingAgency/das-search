using System.Net.Http;
using System.Web.Http.Routing;

namespace Sfa.Das.ApprenticeshipInfoService.UnitTests.Controllers
{
    using System;
    using System.Web.Http;
    using Api.Controllers;
    using Core.Services;

    using FluentAssertions;

    using Moq;
    using NUnit.Framework;

    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public class FrameworkControllerTests
    {
        private FrameworksController _sut;

        [SetUp]
        public void Init()
        {
            var iGetFrameworks = new Mock<IGetFrameworks>();
            iGetFrameworks.Setup(m => m.GetFrameworkById(1234)).Returns(new Framework { FrameworkId = 1234, Title = "test title" });
            _sut = new FrameworksController(iGetFrameworks.Object);
            _sut.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/frameworks")
            };
            _sut.Configuration = new HttpConfiguration();
            _sut.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
            _sut.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "frameworks" } });
        }

        [Test]
        public void ShouldReturnFrameworkNotFound()
        {
            Exception exception = null;
            try
            {
                _sut.Get(-2);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.NotNull(exception);
            Assert.IsTrue(exception.GetType() == typeof(HttpResponseException));
        }

        [Test]
        public void ShouldReturnFramework()
        {
            var framework = _sut.Get(1234);

            Assert.NotNull(framework);
            framework.FrameworkId.Should().Be(1234);
            framework.Title.Should().Be("test title");
            framework.Uri.Should().Be("http://localhost/api/Frameworks/1234");
        }
    }
}
