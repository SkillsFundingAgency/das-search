namespace Sfa.Eds.Das.Web.UnitTests.Extensions.For_HtmlExtensions
{
    using System.Web.Mvc;

    using Moq;

    using NUnit.Framework;

    using Web.Extensions;

    [TestFixture]
    public sealed class WhenRenderIfExists
    {
        private readonly HtmlHelper helper = new HtmlHelper(new ViewContext(), Mock.Of<IViewDataContainer>());

        [Test]
        public void ItShouldCreateLink()
        {
            var result = helper.RenderAIfExists("hello", "http://localhost:8888", null);

            Assert.AreEqual(new MvcHtmlString("<a href=\"http://localhost:8888\" class=\"\">hello</a>").ToHtmlString(), result.ToHtmlString());
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
    }
}