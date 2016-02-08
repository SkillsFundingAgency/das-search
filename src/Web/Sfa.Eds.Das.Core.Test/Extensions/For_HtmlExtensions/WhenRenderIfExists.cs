namespace Sfa.Eds.Das.Core.Test.Extensions.For_HtmlExtensions
{
    using Core.Extensions;
    using System.Web.Mvc;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class WhenRenderIfExists
    {
        private HtmlHelper helper = new HtmlHelper(new ViewContext(), Mock.Of<IViewDataContainer>());

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

            Assert.AreEqual(new MvcHtmlString("").ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldBeEmptyIfLinkEmpty()
        {
            var result = helper.RenderAIfExists("hello", "", null);

            Assert.AreEqual(new MvcHtmlString("").ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldBeEmptyIfTitleEmpty()
        {
            var result = helper.RenderAIfExists("", "http://localhost:8888", null);

            Assert.AreEqual(new MvcHtmlString("").ToHtmlString(), result.ToHtmlString());
        }

        [Test]
        public void ItShouldBeEmptyIfTitleNull()
        {
            var result = helper.RenderAIfExists(null, "http://localhost:8888", null);

            Assert.AreEqual(new MvcHtmlString("").ToHtmlString(), result.ToHtmlString());
        }
    }
}

