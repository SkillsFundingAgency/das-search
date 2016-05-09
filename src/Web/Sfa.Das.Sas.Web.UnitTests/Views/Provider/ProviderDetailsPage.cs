using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.UnitTests.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Provider;

namespace Sfa.Das.Sas.Web.UnitTests.Views.Provider
{
    [TestFixture]
    public sealed class ProviderDetailsPage : ViewTestBase
    {
        [Test]
        public void ShouldShowAllFieldsWhenEverythingIsOk()
        {
            var detail = new Detail();

            var model = new ProviderViewModel
            {
                Name = "Test name",
                EmployerSatisfactionMessage = "100%",
                LearnerSatisfactionMessage = "100%",
                Location = new Location
                {
                    LocationId = 1,
                    LocationName = "Test location name"
                },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                DeliveryModes = new List<string> { "BlockRelease" },
                ContactInformation = new ContactInformation
                {
                    ContactUsUrl = "http://www.testcontact.url",
                    Email = "Test email",
                    Website = "Test website",
                    Phone = "Test phone"
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = "www.test-apprenticeship.info.url",
                    ApprenticeshipMarketingInfo = "Test apprenticeship marketing info",
                    Code = 1
                },
                ProviderMarketingInfo = "Test provider marketing info",
                ApprenticeshipNameWithLevel = "Test level"
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, ".apprenticeshipContactTitle").Should().Contain("Website");
            this.GetPartial(html, ".apprenticeshipContact").Should().Contain("training provider website");
            this.GetAttribute(html, ".apprenticeshipContact", "href").Should().Be("http://www.test-apprenticeship.info.url", "because http be added if missing");
            this.GetPartial(html, ".providerContactTitle").Should().Contain("Contact page");
            this.GetPartial(html, ".providerContact").Should().Contain("contact this training provider");
            this.GetAttribute(html, ".providerContact", "href").Should().Be("http://www.testcontact.url", "http should only be added once");
            this.GetPartial(html, ".phone-title").Should().Contain("Phone");
            this.GetPartial(html, ".phone").Should().Contain(model.ContactInformation.Phone);
            this.GetPartial(html, ".email-title").Should().Contain("Email");
            this.GetPartial(html, ".email").Should().Contain(model.ContactInformation.Email);
            this.GetPartial(html, ".training-structure").Should().Contain("Training options");
            this.GetPartial(html, ".block-release").Should().Contain("block release");
            this.GetPartial(html, ".training-location-title").Should().Contain("Training location");
            this.GetPartial(html, ".training-location").Should().Contain("Test location name Address 1 Address 2 Town PostCode");
        }

        [Test]
        public void ShouldShowAllDeliveryModesProperly()
        {
            var detail = new Detail();

            var model = new ProviderViewModel
            {
                Name = "Test name",
                EmployerSatisfactionMessage = "100%",
                LearnerSatisfactionMessage = "100%",
                Location = new Location
                {
                    LocationId = 1,
                    LocationName = "Test location name"
                },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer", "DayRelease" },
                ContactInformation = new ContactInformation
                {
                    ContactUsUrl = "Test contact url",
                    Email = "Test email",
                    Website = "Test website",
                    Phone = "Test phone"
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = "Test apprenticeship info url",
                    ApprenticeshipMarketingInfo = "Test apprenticeship marketing info",
                    Code = 1
                },
                ProviderMarketingInfo = "Test provider marketing info",
                ApprenticeshipNameWithLevel = "Test level"
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, ".training-structure").Should().Contain("Training options");
            this.GetPartial(html, ".block-release").Should().Contain("block release");
            this.GetPartial(html, ".hundred-percent-employer").Should().Contain("at your location");
            this.GetPartial(html, ".day-release").Should().Contain("day release");
        }

        [Test]
        public void ShouldShowAddressIfThereAreMoreDeliveryModesAppartFromEmployerLocation()
        {
            var detail = new Detail();

            var model = new ProviderViewModel
            {
                Name = "Test name",
                EmployerSatisfactionMessage = "100%",
                LearnerSatisfactionMessage = "100%",
                Location = new Location
                {
                    LocationId = 1,
                    LocationName = "Test location name"
                },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer", "DayRelease" },
                ContactInformation = new ContactInformation
                {
                    ContactUsUrl = "Test contact url",
                    Email = "Test email",
                    Website = "Test website",
                    Phone = "Test phone"
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = "Test apprenticeship info url",
                    ApprenticeshipMarketingInfo = "Test apprenticeship marketing info",
                    Code = 1
                },
                ProviderMarketingInfo = "Test provider marketing info",
                ApprenticeshipNameWithLevel = "Test level"
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "dl dt", 6).Should().Contain("Training location");
            var locationText = GetPartial(html, ".training-location");

            locationText.Should().Contain(model.Location.LocationName);
            locationText.Should().Contain(model.Address.Address1);
            locationText.Should().Contain(model.Address.Address2);
        }

        [Test]
        public void ShouldShowMessageIfEmployerLocationIsTheOnlyDeliveryMode()
        {
            var detail = new Detail();

            var model = new ProviderViewModel
            {
                Name = "Test name",
                EmployerSatisfactionMessage = "100%",
                LearnerSatisfactionMessage = "100%",
                Location = new Location
                {
                    LocationId = 1,
                    LocationName = "Test location name"
                },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                DeliveryModes = new List<string> { "100PercentEmployer" },
                ContactInformation = new ContactInformation
                {
                    ContactUsUrl = "Test contact url",
                    Email = "Test email",
                    Website = "Test website",
                    Phone = "Test phone"
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = "Test apprenticeship info url",
                    ApprenticeshipMarketingInfo = "Test apprenticeship marketing info",
                    Code = 1
                },
                ProviderMarketingInfo = "Test provider marketing info",
                ApprenticeshipNameWithLevel = "Test level"
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "dl dt", 6).Should().Contain("Training location");
            var locationText = GetPartial(html, ".training-location");

            locationText.Should().NotContain(model.Location.LocationName);
            locationText.Should().NotContain(model.Address.Address1);
            locationText.Should().NotContain(model.Address.Address2);
            locationText.Should().Contain("Training takes place at your location");
        }

        [Test]
        public void ShouldShowProviderLocationWhenDifferentDeliveryModesThan100PercentEmployerLocation()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                Name = "Test name",
                EmployerSatisfactionMessage = "100%",
                LearnerSatisfactionMessage = "100%",
                Location = new Location
                {
                    LocationId = 1,
                    LocationName = "Test location name"
                },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                DeliveryModes = new List<string> { "BlockRelease" },
                ContactInformation = new ContactInformation
                {
                    ContactUsUrl = "Test contact url",
                    Email = "Test email",
                    Website = "Test website",
                    Phone = "Test phone"
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = "Test apprenticeship info url",
                    ApprenticeshipMarketingInfo = "Test apprenticeship marketing info",
                    Code = 1
                },
                ProviderMarketingInfo = "Test provider marketing info",
                ApprenticeshipNameWithLevel = "Test level"
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".apprenticeship-name-level").Should().Contain(model.ApprenticeshipNameWithLevel);

            var locationText = GetPartial(html, ".training-location");

            locationText.Should().Contain(model.Location.LocationName);
            locationText.Should().Contain(model.Address.Address1);
            locationText.Should().Contain(model.Address.Address2);
        }

        [Test]
        public void ShouldShowProviderMarketingInfo()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                Name = "Test name",
                EmployerSatisfactionMessage = "100%",
                LearnerSatisfactionMessage = "100%",
                Location = new Location
                {
                    LocationId = 1,
                    LocationName = "Test location name"
                },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer", "DayRelease" },
                ContactInformation = new ContactInformation
                {
                    ContactUsUrl = "Test contact url",
                    Email = "Test email",
                    Website = "Test website",
                    Phone = "Test phone"
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = "Test apprenticeship info url",
                    ApprenticeshipMarketingInfo = "Test apprenticeship marketing info",
                    Code = 1
                },
                ProviderMarketingInfo = "Test provider marketing info",
                ApprenticeshipNameWithLevel = "Test level"
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var providerMarketingInfoHtml = GetPartial(html, ".provider-marketing-info");

            providerMarketingInfoHtml.Should().Contain(model.ProviderMarketingInfo);
        }

        [Test]
        public void ShouldShowApprenticeshipNameWithLevel()
        {
            var detail = new Detail();
            var model = new ProviderViewModel
            {
                Name = "Test name",
                EmployerSatisfactionMessage = "100%",
                LearnerSatisfactionMessage = "100%",
                Location = new Location
                {
                    LocationId = 1,
                    LocationName = "Test location name"
                },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer", "DayRelease" },
                ContactInformation = new ContactInformation
                {
                    ContactUsUrl = "Test contact url",
                    Email = "Test email",
                    Website = "Test website",
                    Phone = "Test phone"
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = "Test apprenticeship info url",
                    ApprenticeshipMarketingInfo = "Test apprenticeship marketing info",
                    Code = 1
                },
                ProviderMarketingInfo = "Test provider marketing info",
                ApprenticeshipNameWithLevel = "Test level"
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".apprenticeship-name-level").Should().Contain(model.ApprenticeshipNameWithLevel);
        }
    }
}
