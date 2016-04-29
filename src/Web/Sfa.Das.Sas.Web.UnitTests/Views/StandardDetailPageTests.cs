using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Apprenticeship;

namespace Sfa.Das.Sas.Web.UnitTests.Views
{
    [TestFixture]
    public sealed class StandardDetailPageTests : ViewTestBase
    {
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
