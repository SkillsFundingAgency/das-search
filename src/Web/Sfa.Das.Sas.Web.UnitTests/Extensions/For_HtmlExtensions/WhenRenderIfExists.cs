using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Extensions;

namespace Sfa.Das.Sas.Web.UnitTests.Extensions.For_HtmlExtensions
{
    [TestFixture]
    public sealed class WhenRenderIfExists
    {
        private readonly HtmlHelper helper = new HtmlHelper(new ViewContext(), Mock.Of<IViewDataContainer>());

        [Test]
        public void ItShouldCreateLink()
        {
            var result = helper.RenderAIfExists("hello", "http://localhost:8888", null);

            Assert.AreEqual(new MvcHtmlString("<a href=\"http://localhost:8888\" target=\"_self\" class=\"\">hello</a>").ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldBeEmptyIfLinkNull()
        {
            var result = helper.RenderAIfExists("hello", null, null);

            Assert.AreEqual(new MvcHtmlString(string.Empty).ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldBeEmptyIfLinkEmpty()
        {
            var result = helper.RenderAIfExists("hello", string.Empty, null);

            Assert.AreEqual(new MvcHtmlString(string.Empty).ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldBeEmptyIfTitleEmpty()
        {
            var result = helper.RenderAIfExists(string.Empty, "http://localhost:8888", null);

            Assert.AreEqual(new MvcHtmlString(string.Empty).ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldBeEmptyIfTitleNull()
        {
            var result = helper.RenderAIfExists(null, "http://localhost:8888", null);

            Assert.AreEqual(new MvcHtmlString(string.Empty).ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldAddTarget()
        {
            var result = helper.RenderAIfExists("hello", "http://www.localhost:8888", null, "_blank");

            Assert.AreEqual(new MvcHtmlString("<a href=\"http://www.localhost:8888\" target=\"_blank\" class=\"\">hello</a>").ToHtmlString(), result.ToHtmlString());
        }
    }
}