using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.ExtensionHelpers;

namespace Sfa.Das.Sas.Web.UnitTests.Views.Start
{
    [TestFixture]
    public class StartPageTest : ViewTestBase
    {
        [Test]
        public void ShouldHaveStartButton()
        {
            var startPage = new Sfa.Das.Sas.Web.Views.Start.Start();

            var html = startPage.RenderAsHtml().ToAngleSharp();

            var button = GetHtmlElement(html, "#start-button");

            button.OuterHtml.Should().Contain(" id=\"start-button\"");
        }
    }
}
