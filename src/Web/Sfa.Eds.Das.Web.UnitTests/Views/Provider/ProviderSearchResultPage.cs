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
    public sealed class ProviderSearchResultPage : ViewTestBase
    {
        [Test]
        public void ShouldShowAnErrorWhenSomethingIsWrong()
        {
            var detail = new SearchResultMessage();
            var model = new ProviderSearchResultViewModel
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
            var detail = new SearchResultMessage();
            var model = new ProviderSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There is 1 training provider for the apprenticeship standard: 'Test name'.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturned()
        {
            var detail = new SearchResultMessage();
            var model = new ProviderSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<ProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There are 7 training providers for the apprenticeship standard: 'Test name'.");
        }

        [Test]
        public void ShouldHaveAllFieldsInSearchResult()
        {
            var page = new ProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address()
            };
            var item2 = new ProviderResultItemViewModel
            {
                Name = "Provider 2",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address()
            };

            var model = new ProviderSearchResultViewModel
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
            GetPartial(html, ".result dl dd").Should().Be("Training can take place at your location.");

            GetPartial(html, ".result dl dt", 2).Should().Be("Website:");
            GetPartial(html, ".result dl dd", 2).Should().Be("http://www.trainingprovider.co.uk");

            var secondResult = GetHtmlElement(html, ".result", 2);

            GetPartial(secondResult, "dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowJustDistanceIfDeliveryModeIsNotEmployerLocation()
        {
            var page = new ProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address()
            };

            var model = new ProviderSearchResultViewModel
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
            var page = new ProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer", "blockRelease" },
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address()
            };

            var model = new ProviderSearchResultViewModel
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
            GetPartial(html, ".result dl dd").Should().Be("Training can take place at your location.");

            GetPartial(html, ".result dl dt", 2).Should().Be("Website:");
            GetPartial(html, ".result dl dd", 2).Should().Be("http://www.trainingprovider.co.uk");
        }

        [Test]
        public void ShouldShowJustEmployerLocationIfDeliveryModeOnlyHasEmployerLocation()
        {
            var page = new ProviderInformation();
            var item = new ProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address()
            };

            var model = new ProviderSearchResultViewModel
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
            GetPartial(html, ".result dl dd").Should().Be("Training can take place at your location.");

            GetPartial(html, ".result dl dt", 2).Should().Be("Website:");
            GetPartial(html, ".result dl dd", 2).Should().Be("http://www.trainingprovider.co.uk");
        }
    }
}
