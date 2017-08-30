using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Repositories;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Helpers;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    [TestFixture]
    public class SitemapControllerTests
    {
        [Test]
        public void ShouldGenerateSitemapXmlGivenProviderList()
        {
            var mockContext = new Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            var ukprn1 = 11;
            var ukprn2 = 2;
            var name1 = "eleven";
            var name2 = "a-two";

            var dummyDomain = "http://test.com";
            mockRequest.Setup(x => x.Url).Returns(new System.Uri(dummyDomain + "/dummy-path-and-query"));

            mockContext
                .Setup(c => c.Request)
                .Returns(mockRequest.Object);

            var mockProviderRepository = new Mock<IProviderDetailRepository>();
            mockProviderRepository.Setup(x => x.GetProviderList())
                .Returns(Task.FromResult(
                    (IDictionary<long, string>)
                    new Dictionary<long, string>
                {
                    { ukprn1, name1 },
                    { ukprn2, name2 }
                }));

            var mockStringUrlHelper = new Mock<IUrlEncoder>();
            mockStringUrlHelper.Setup(x => x.EncodeTextForUri(It.IsAny<string>()))
                .Returns<string>(x => x);

            var sitemapController = new SitemapController(null,mockProviderRepository.Object, mockStringUrlHelper.Object);
            sitemapController.ControllerContext = new ControllerContext(mockContext.Object, new RouteData(), sitemapController);
            var result = (ContentResult)sitemapController.Providers().Result;

            var expectedResult = $@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
  <url>
    <loc>
      {dummyDomain}/provider/{ukprn1}/{name1}
    </loc>
  </url>
  <url>
    <loc>
      {dummyDomain}/provider/{ukprn2}/{name2}
    </loc>
  </url>
</urlset>";

           Assert.AreEqual(result.Content, expectedResult,"The xml returned was not as expected");
           Assert.AreEqual(result.ContentType, "text/xml", "The content type was not set as xml");
        }
    }
}
