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
    public sealed class StandardProviderSearchResultPage : ViewTestBase
    {
        [Test]
        public void ShouldShowAnErrorWhenSomethingIsWrong()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                Hits = new List<ProviderResultItemViewModel>(),
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturned()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There is 1 training provider for the apprenticeship course: 'Test name'.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturned()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There are 7 training providers for the apprenticeship course: 'Test name'.");
        }

        [Test]
        public void ShouldHaveAllFieldsInSearchResult()
        {
            var page = new StandardProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Distance = 1,
                Address = new Address()
            };
            var item2 = new ProviderResultItemViewModel
            {
                Name = "Provider 2",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address()
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel> { item, item2 },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("1 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Training can take place at your location.");

            var secondResult = GetHtmlElement(html, ".result", 2);

            GetPartial(secondResult, "dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowJustDistanceIfDeliveryModeIsNotEmployerLocation()
        {
            var page = new StandardProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address()
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");
            GetPartial(html, ".result dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowJustEmployerLocationIfDeliveryModeContainsEmployerLocation()
        {
            var page = new StandardProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer", "blockRelease" },
                Address = new Address()
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("0 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Training can take place at your location.");
        }

        [Test]
        public void ShouldShowProviderLocationIfDeliveryModeDoesNotContainHundredEmployerLocation()
        {
            var page = new StandardProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 3,
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                }
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("3 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Address 1 Address 2 County PostCode");
        }

        [Test]
        public void ShouldShowPercentageForSatisfactionResultWhenprovided()
        {
            var page = new StandardProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address(),
                EmployerSatisfactionMessage = "87%",
                LearnerSatisfactionMessage = "99.9%"
            };

            var model = new ProviderStandardSearchResultViewModel()
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-data-list dd", 3).Should().Be("87%");
            GetPartial(html, ".result-data-list dd", 4).Should().Be("99.9%");
        }
    }
}
