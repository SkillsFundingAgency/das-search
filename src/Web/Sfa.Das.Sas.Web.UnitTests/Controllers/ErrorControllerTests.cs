using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Controllers;

namespace Sfa.Das.Sas.Web.UnitTests.Controllers
{
    [TestFixture]
    public sealed class ErrorControllerTests
    {
        [Test]
        public void NotFoundShouldReturnAValidViewResult()
        {
            var controller = new ErrorController();

            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            Mock<HttpResponseBase> httpResponseMock = new Mock<HttpResponseBase>();
            httpContextMock.SetupGet(c => c.Response).Returns(httpResponseMock.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            controller.Url = urlHelperMock.Object;
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            var result = controller.NotFound();

            result.Should().BeOfType<ViewResult>();

            var responseResult = (ViewResult)result;

            responseResult.Should().NotBe(null);
        }

        [Test]
        public void ErrorShouldReturnAValidViewResult()
        {
            var controller = new ErrorController();

            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            Mock<HttpResponseBase> httpResponseMock = new Mock<HttpResponseBase>();
            httpContextMock.SetupGet(c => c.Response).Returns(httpResponseMock.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            controller.Url = urlHelperMock.Object;
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            var result = controller.Error();

            result.Should().BeOfType<ViewResult>();

            var responseResult = (ViewResult)result;

            responseResult.Should().NotBe(null);
        }
    }
}
