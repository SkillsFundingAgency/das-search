using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Extensions;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Extensions.For_HtmlExtensions
{
    [TestFixture]
    public class WhenConvertingMarkdownToHtml
    {
        private readonly HtmlHelper _helper = new HtmlHelper(new ViewContext(), Mock.Of<IViewDataContainer>());

        [Test]
        public void ConvertingBold()
        {
            var htmlText = _helper.MarkdownToHtml("**Hello World**");

            Assert.AreEqual("<div class=\"markdown\"><p><strong>Hello World</strong></p>\r\n\r\n</div>", htmlText.ToHtmlString().TrimEnd());
        }

        [Test]
        public void ConvertingEmptyString()
        {
            var htmlText = _helper.MarkdownToHtml(string.Empty);

            Assert.AreEqual(string.Empty, htmlText.ToHtmlString());
        }
    }
}