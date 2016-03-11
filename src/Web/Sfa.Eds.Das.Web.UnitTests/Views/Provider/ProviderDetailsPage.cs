namespace Sfa.Eds.Das.Web.UnitTests.Views.Provider
{
    using System.Collections.Generic;
    using System.Linq;

    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    using Sfa.Das.ApplicationServices.Models;

    using ViewModels;
    using Web.Views.Provider;

    [TestFixture]
    public sealed class ProviderDetailsPage : ViewTestBase
    {
        [Test]
        public void ShouldNotShowProviderLocationWhenOnly100PercentEmployerLocation()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                ProviderName = "Test name",
                VenueName = "Test venue name",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address
                {
                    Address1 = "Address1",
                    Address2 = "Address2",
                    County = "County",
                    Postcode = "Postcode",
                    Town = "Town"
                }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "header h2").Should().Contain(model.ProviderName);

            GetPartial(html, "dl dd div").Should().NotContain(model.VenueName);
            GetPartial(html, "dl dd div", 2).Should().NotContain(model.Address.Address1);
            GetPartial(html, "dl dd div", 3).Should().NotContain(model.Address.Address2);
            GetPartial(html, "dl dd div").Should().Contain("Training will take place at your location");

        }

        [Test]
        public void ShouldShowProviderLocationWhenMoreDeliveryModesAppartFrom100PercentEmployerLocation()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                ProviderName = "Test name",
                VenueName = "Test venue name",
                DeliveryModes = new List<string> { "100PercentEmployer", "blockRelease" },
                Address = new Address
                {
                    Address1 = "Address1",
                    Address2 = "Address2",
                    County = "County",
                    Postcode = "Postcode",
                    Town = "Town"
                }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "header h2").Should().Contain(model.ProviderName);

            GetPartial(html, "dl dd div").Should().Contain(model.VenueName);
            GetPartial(html, "dl dd div", 2).Should().Contain(model.Address.Address1);
            GetPartial(html, "dl dd div", 3).Should().Contain(model.Address.Address2);
        }

        [Test]
        public void ShouldShowProviderLocationWhenDifferentDeliveryModesThan100PercentEmployerLocation()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                ProviderName = "Test name",
                VenueName = "Test venue name",
                DeliveryModes = new List<string> { "blockRelease" },
                Address = new Address
                {
                    Address1 = "Address1",
                    Address2 = "Address2",
                    County = "County",
                    Postcode = "Postcode",
                    Town = "Town"
                }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "header h2").Should().Contain(model.ProviderName);

            GetPartial(html, "dl dd div").Should().Contain(model.VenueName);
            GetPartial(html, "dl dd div", 2).Should().Contain(model.Address.Address1);
            GetPartial(html, "dl dd div", 3).Should().Contain(model.Address.Address2);
        }
    }
}
