﻿namespace Sfa.Eds.Das.Web.UnitTests.Views.Provider
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

            this.GetPartial(html, "dl dt").Should().Contain("Website course page");
            this.GetPartial(html, "dl dd").Should().Contain("Test apprenticeship info url");
            this.GetPartial(html, "dl dt", 2).Should().Contain("Website contact page");
            this.GetPartial(html, "dl dd", 2).Should().Contain("Test contact url");
            this.GetPartial(html, "dl dt", 3).Should().Contain("Phone");
            this.GetPartial(html, "dl dd", 3).Should().Contain("Test phone");
            this.GetPartial(html, "dl dt", 4).Should().Contain("Email");
            this.GetPartial(html, "dl dd", 4).Should().Contain("Test email");
            this.GetPartial(html, "dl dt", 5).Should().Contain("Training structure");
            this.GetPartial(html, "dl dd ul").Should().Contain("block release");
            this.GetPartial(html, "dl dt", 6).Should().Contain("Training location");
            this.GetPartial(html, "dl dd", 6).Should().Contain("Test location name Address 1 Address 2 PostCode");
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

            this.GetPartial(html, "dl dt", 5).Should().Contain("Training structure");
            this.GetPartial(html, "dl dd ul li").Should().Contain("block release");
            this.GetPartial(html, "dl dd ul li", 2).Should().Contain("at your location");
            this.GetPartial(html, "dl dd ul li", 3).Should().Contain("day release");
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
            var locationText = GetPartial(html, "dl dd", 6);

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
            var locationText = GetPartial(html, "dl dd", 6);

            locationText.Should().NotContain(model.Location.LocationName);
            locationText.Should().NotContain(model.Address.Address1);
            locationText.Should().NotContain(model.Address.Address2);
            locationText.Should().Contain("Training will take place at your location");
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

            GetPartial(html, "header h2").Should().Contain(model.ApprenticeshipNameWithLevel);

            var locationText = GetPartial(html, "dl dd", 6);

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

            var providerMarketingInfoHtml = GetPartial(html, "div div div p");

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

            GetPartial(html, "header h2").Should().Contain(model.ApprenticeshipNameWithLevel);
        }
    }
}
