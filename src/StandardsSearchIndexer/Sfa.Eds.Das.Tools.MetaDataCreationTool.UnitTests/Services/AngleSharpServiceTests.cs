namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test.Services
{
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;

    [TestFixture]
    public class AngleSharpServiceTests
    {
        private readonly string _htmlText = "<html><body>" + "<div>" + "<a href=\"goodbye.com\">Goodbye</a>" + "<a href=\"hello.com\">HELLO</a>" + "<a href=\"Hej.com\">Hej</a>" + "</div>" + "</body></html>";

        [Test]
        public void TestTest()
        {
            var mockBrowsingContext = new Mock<IHttpGet>();
            mockBrowsingContext.Setup(m => m.Get(It.IsAny<string>(), null, null)).Returns(_htmlText);

            AngleSharpService angleSharpService = new AngleSharpService(mockBrowsingContext.Object);
            var x = angleSharpService.GetLinks(string.Empty, "div a", "HELLO");

            Assert.AreEqual(1, x.Count);
            Assert.AreEqual("hello.com", x.FirstOrDefault());
        }
    }
}
