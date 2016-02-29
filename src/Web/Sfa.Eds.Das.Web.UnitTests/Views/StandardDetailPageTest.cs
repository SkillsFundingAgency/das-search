namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using ViewModels;
    using Web.Views.Standard;

    [TestFixture]
    public sealed class StandardDetailPageTest : ViewTestBase
    {
        [Test]
        public void ShouldShowRequiredFieldWhenErrorIsReceived()
        {
            var detail = new BlankFieldErrorMessage();
            var model = new StandardViewModel
            {
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Be("This field can't be blank");
        }

        [Test]
        public void ShouldNotShowRequiredFieldWhenErrorIsMissing()
        {
            var detail = new BlankFieldErrorMessage();
            var model = new StandardViewModel
            {
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().BeEmpty();
        }
    }
}
