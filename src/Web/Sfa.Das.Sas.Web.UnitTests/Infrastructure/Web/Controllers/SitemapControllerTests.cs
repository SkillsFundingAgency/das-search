namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Core.Domain.Repositories;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Controllers;
    using Sas.Web.Helpers;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

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
            var ukprn3 = 3;
            var name1 = "eleven";
            var name2 = "a-two";
            var name3 = "three";

            var dummyDomain = "http://test.com";
            mockRequest.Setup(x => x.Url).Returns(new System.Uri(dummyDomain + "/dummy-path-and-query"));

            mockContext
                .Setup(c => c.Request)
                .Returns(mockRequest.Object);

            var providerSummaries = new List<ProviderSummary>
            {
                new ProviderSummary { Ukprn = ukprn1, ProviderName = name1, IsEmployerProvider = false },
                new ProviderSummary { Ukprn = ukprn2, ProviderName = name2, IsEmployerProvider = true },
                new ProviderSummary { Ukprn = ukprn3, ProviderName = name3, IsEmployerProvider = false }
            };


            var mockProviderRepository = new Mock<IProviderDetailRepository>();
            mockProviderRepository.Setup(x => x.GetProviderList())
                .Returns(Task.FromResult((IEnumerable<ProviderSummary>)providerSummaries));

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
      {dummyDomain}/provider/{ukprn3}/{name3}
    </loc>
  </url>
</urlset>";

           Assert.AreEqual(result.Content, expectedResult, "The xml returned was not as expected");
           Assert.AreEqual(result.ContentType, "text/xml", "The content type was not set as xml");
        }
    }
}
