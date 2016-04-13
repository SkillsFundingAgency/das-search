namespace Sfa.Eds.Das.Web.UnitTests.Views.Provider
{
    using System.Collections.Generic;
    using System.Linq;

    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Model;

    using ViewModels;
    using Web.Views.Provider;

    [TestFixture]
    public sealed class FrameworkProviderSearchResultPage : ViewTestBase
    {
        [Test]
        public void ShouldShowAnErrorWhenSomethingIsWrong()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturned()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There is 1 training provider for the apprenticeship course: 'Pathway test name'.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturned()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There are 7 training providers for the apprenticeship course: 'Pathway test name'.");
        }

        [Test]
        public void ShouldHaveAllFieldsInSearchResult()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address()
            };
            var item2 = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 2",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address()
            };

            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                Hits = new List<FrameworkProviderResultItemViewModel> { item, item2 },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");
            GetPartial(html, ".result dl dd").Should().Be("Training can take place at your location.");

            var secondResult = GetHtmlElement(html, ".result", 2);

            GetPartial(secondResult, "dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowJustDistanceIfDeliveryModeIsNotEmployerLocation()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address()
            };

            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                Hits = new List<FrameworkProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");
            GetPartial(html, ".result dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowJustEmployerLocationIfDeliveryModeContainsEmployerLocation()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer", "blockRelease" },
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address()
            };

            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                Hits = new List<FrameworkProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");
            GetPartial(html, ".result dl dd").Should().Be("Training can take place at your location.");
        }

        [Test]
        public void ShouldShowJustEmployerLocationIfDeliveryModeOnlyHasEmployerLocation()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address()
            };

            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                Hits = new List<FrameworkProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");
            GetPartial(html, ".result dl dd").Should().Be("Training can take place at your location.");
        }
    }
}
