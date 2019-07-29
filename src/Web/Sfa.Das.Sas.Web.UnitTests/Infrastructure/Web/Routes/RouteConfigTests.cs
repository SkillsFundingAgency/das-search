using System;
using System.Linq;
using System.Web.Routing;
using MvcRouteTester;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Queries;

namespace Sfa.Das.Sas.Web.UnitTests.Routes
{
    [TestFixture]
    public class RouteConfigTests
    {
        private RouteCollection _routes;

        [SetUp]
        public void Init()
        {
            _routes = new RouteCollection();

            RouteConfig.RegisterRoutes(_routes);
        }

        [Test]
        public void ShouldMapProviderWithoutName()
        {
            Console.WriteLine(string.Join(Environment.NewLine,_routes.Select(x => (x as Route)?.Url)));

            var expectedRoute = new { controller = "Provider", action = "ProviderDetail" };
            RouteAssert.HasRoute(_routes, "/provider/1", expectedRoute);
        }

        [Test]
        public void ShouldMapProviderWithName()
        {
            var expectedRoute = new { controller = "Provider", action = "ProviderDetail" };
            RouteAssert.HasRoute(_routes, "/provider/1/name", expectedRoute);
        }

        [Test]
        public void ShouldMapProviderDetail()
        {
            var expectedRoute = new {controller = "Provider", action = "Detail", criteria = new ApprenticeshipProviderDetailQuery()};
            RouteAssert.HasRoute(_routes, "/provider/detail/", expectedRoute);
        }

        [Test]
        public void ShouldMapFrameworkResults()
        {
            var expectedRoute = new { controller = "Provider", action = "FrameworkResults", criteria = new ProviderSearchQuery() };
            RouteAssert.HasRoute(_routes, "/provider/frameworkresults", expectedRoute);
        }
    }
}