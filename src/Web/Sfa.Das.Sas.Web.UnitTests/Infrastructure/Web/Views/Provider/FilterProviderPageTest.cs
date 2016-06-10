using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Provider;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Provider
{
    [TestFixture]
    public sealed class FilterProviderPageTest : ViewTestBase
    {
        [Test]
        public void ShouldRenderInfoBox()
        {
            var detail = new FilterProviders();
            var model = new DeliveryModeViewModel[0];

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var summary = GetPartial(html, "details summary");
            summary.Should().Be("Explain training options");
        }

        [Test]
        public void ShouldRenderTraingOptionsInputs()
        {
            var detail = new FilterProviders();

            var model = new DeliveryModeViewModel[3];

            var first = new DeliveryModeViewModel { Checked = true, Value = "dayrelease", Count = 35, Title = "day release" };
            var second = new DeliveryModeViewModel { Checked = true, Value = "blockrelease", Count = 0, Title = "block release" };
            var third = new DeliveryModeViewModel { Checked = true, Value = "100percentemployer", Count = 28, Title = "at your location" };

            model[0] = first;
            model[1] = second;
            model[2] = third;

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var result1 = GetHtmlElement(html, "input", 1);
            var result2 = GetHtmlElement(html, "input", 2);
            var result3 = GetHtmlElement(html, "input", 3);

            result1.Attributes["value"].Value.Should().Be("dayrelease");
            result2.Attributes["value"].Value.Should().Be("blockrelease");
            result3.Attributes["value"].Value.Should().Be("100percentemployer");
        }

        [Test]
        public void ShouldRenderTraingOptionsText()
        {
            var detail = new FilterProviders();

            var model = new DeliveryModeViewModel[3];

            var first = new DeliveryModeViewModel { Checked = true, Value = "dayrelease", Count = 35, Title = "day release" };
            var second = new DeliveryModeViewModel { Checked = true, Value = "blockrelease", Count = 0, Title = "block release" };
            var third = new DeliveryModeViewModel { Checked = true, Value = "100percentemployer", Count = 28, Title = "at your location" };

            model[0] = first;
            model[1] = second;
            model[2] = third;

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var result1 = GetPartial(html, "label", 1);
            var result2 = GetPartial(html, "label", 2);
            var result3 = GetPartial(html, "label", 3);

            result1.Should().Be("day release (35)");
            result2.Should().Be("block release (0)");
            result3.Should().Be("at your location (28)");
        }
    }
}
