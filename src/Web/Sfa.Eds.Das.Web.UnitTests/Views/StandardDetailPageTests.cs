namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using ViewModels;
    using Web.Views.Apprenticeship;

    [TestFixture]
    public sealed class StandardDetailPageTests : ViewTestBase
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

        [Test]
        public void ShouldShowEquivalentLevel()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                NotionalEndLevel = 6,
            };
            var model2 = new StandardViewModel
            {
                NotionalEndLevel = 4,
            };
            var model3 = new StandardViewModel
            {
                NotionalEndLevel = 8,
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "dd", 2).Should().Contain("bachelor's degree");

            html = detail.RenderAsHtml(model2).ToAngleSharp();
            GetPartial(html, "dd", 2).Should().Contain("certificate of higher education");

            html = detail.RenderAsHtml(model3).ToAngleSharp();
            GetPartial(html, "dd", 2).Should().Contain("doctorate");
        }
    }
}
