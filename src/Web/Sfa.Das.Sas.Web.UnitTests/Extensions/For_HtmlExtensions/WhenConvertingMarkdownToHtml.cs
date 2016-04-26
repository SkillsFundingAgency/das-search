using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Extensions;

namespace Sfa.Das.Sas.Web.UnitTests.Extensions.For_HtmlExtensions
{
    [TestFixture]
    public class WhenConvertingMarkdownToHtml
    {
        private readonly HtmlHelper helper = new HtmlHelper(new ViewContext(), Mock.Of<IViewDataContainer>());

        [Test]
        public void ConvertingBold()
        {
            var htmlText = helper.MarkdownToHtml("**Hello World**");

            Assert.AreEqual("<p><strong>Hello World</strong></p>", htmlText.ToHtmlString().TrimEnd());
        }

        [Test]
        public void ConvertingEmptyString()
        {
            var htmlText = helper.MarkdownToHtml(string.Empty);

            Assert.AreEqual(string.Empty, htmlText.ToHtmlString());
        }
    }
}