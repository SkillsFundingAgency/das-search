namespace Sfa.Eds.Das.Web.UnitTests.Views.Provider
{
    using System.Collections.Generic;
    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Sfa.Eds.Das.Core.Domain.Model;
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
                Name = "Test name",
                LocationName = "Test venue name",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                StandardNameWithLevel = "Demo standard level 2",
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

            GetPartial(html, "header h2").Should().Contain(model.StandardNameWithLevel);

            var locationText = GetPartial(html, "dl dd", 6);

            locationText.Should().NotContain(model.LocationName);
            locationText.Should().NotContain(model.Address.Address1);
            locationText.Should().NotContain(model.Address.Address2);
            locationText.Should().Contain("Training will take place at your location");
        }

        [Test]
        public void ShouldShowProviderLocationWhenMoreDeliveryModesAppartFrom100PercentEmployerLocation()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                Name = "Test name",
                LocationName = "Test venue name",
                DeliveryModes = new List<string> { "100PercentEmployer", "blockRelease" },
                StandardNameWithLevel = "Demo standard level 2",
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

            GetPartial(html, "header h2").Should().Contain(model.StandardNameWithLevel);

            var locationText = GetPartial(html, "dl dd", 6);

            locationText.Should().Contain(model.LocationName);
            locationText.Should().Contain(model.Address.Address1);
            locationText.Should().Contain(model.Address.Address2);
        }

        [Test]
        public void ShouldShowProviderLocationWhenDifferentDeliveryModesThan100PercentEmployerLocation()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                Name = "Test name",
                LocationName = "Test venue name",
                DeliveryModes = new List<string> { "blockRelease" },
                StandardNameWithLevel = "Demo standard level 2",
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

            GetPartial(html, "header h2").Should().Contain(model.StandardNameWithLevel);

            var locationText = GetPartial(html, "dl dd", 6);

            locationText.Should().Contain(model.LocationName);
            locationText.Should().Contain(model.Address.Address1);
            locationText.Should().Contain(model.Address.Address2);
        }

        [Test]
        public void ShouldShowDeliveryModesWithCorrectText()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                Name = "Test name",
                LocationName = "Test venue name",
                DeliveryModes = new List<string> { "100PercentEmployer", "dayRelease", "blockRelease" },
                StandardNameWithLevel = "Demo standard level 2",
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

            GetPartial(html, "header h2").Should().Contain(model.StandardNameWithLevel);

            GetPartial(html, "ul li").Should().Contain("at your location");
        }
    }
}
