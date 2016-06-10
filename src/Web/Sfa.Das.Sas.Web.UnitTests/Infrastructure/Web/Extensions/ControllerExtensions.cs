using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static void SetRequestUrl(this Controller controller, string url)
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(x => x.UrlReferrer).Returns(new Uri(url));

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mockRequest.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            controller.Url = new UrlHelper(
                new RequestContext(context.Object, new RouteData()),
                new RouteCollection());
        }
    }
}
