namespace Sfa.Eds.Das.Web.UnitTests.Views.Start
{
    using NUnit.Framework;

    using RazorGenerator.Testing;

    using ExtensionHelpers;

    using FluentAssertions;

    using Web.Views.Start;

    [TestFixture]
    public class StartPageTest : ViewTestBase
    {
        [Test]
        public void ShouldHaveStartButton()
        {
            var startPage = new Start();

            var html = startPage.RenderAsHtml().ToAngleSharp();

            var button = GetHtmlElement(html, "#start-button");

            button.OuterHtml.Should().Contain(" id=\"start-button\"");

        }
    }
}
