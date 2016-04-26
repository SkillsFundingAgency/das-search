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

            this.GetPartial(html, ".result-message p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void ShouldShowZeroResultsMessageWhenThereAreNoResults()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                PostCode = "Test postcode",
                FrameworkLevel = 3,
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, ".result-message").Should().Contain("There are currently no providers for the apprenticeship: Test name: Pathway test name level 3 covering postcode 'Test postcode'");
        }

        [Test]
        public void ShouldShowZeroResultsMessageWhenThereAreNoResultsAndThereIsNoLevel()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                PostCode = "Test postcode",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, ".result-message").Should().Contain("There are currently no providers for the apprenticeship: Test name: Pathway test name level 0 covering postcode 'Test postcode'");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturned()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
                TotalResults = 1,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                FrameworkLevel = 3,
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-message p").Should().Contain("There is 1 training provider for the apprenticeship: Test name: Pathway test name level 3.");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturnedAndThereIsNoLevel()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
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

            GetPartial(html, ".result-message p").Should().Contain("There is 1 training provider for the apprenticeship: Test name: Pathway test name level 0.");

            GetPartial(html, "p").Should().Contain("There is 1 training provider for the apprenticeship: Test name: Pathway test name level 0.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturned()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
                TotalResults = 7,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkLevel = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                Level = 3,
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, ".result-message p").Should().Contain("There are 7 training providers for the apprenticeship: Test name: Pathway test name level 2.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturnedAndThereIsNoLevel()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
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

            this.GetPartial(html, "p").Should().Contain("There are 7 training providers for the apprenticeship: Test name: Pathway test name level 0.");
        }

        [Test]
        public void ShouldHaveAllFieldsInSearchResult()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Distance = 0.3,
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
            GetPartial(html, ".result dl dd").Should().Be("0.3 miles away");

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
        public void ShouldShowTrainingAddressIfDeliveryModeContainsEmployerLocationButIsNotTheOnlOne()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer", "blockRelease" },
                Distance = 3,
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                }
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
            GetPartial(html, ".result dl dd").Should().Be("3 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Address 1 Address 2 County PostCode");
        }

        [Test]
        public void ShouldShowEmployerLocationIfDeliveryModeContainsEmployerLocationAndIsTheOnlyOne()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Distance = 3,
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                }
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
            GetPartial(html, ".result dl dd").Should().Be("3 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Training takes place at your location.");
        }

        [Test]
        public void ShouldShowTrainingLocationIfDeliveryModeHasLocationDifferentToEmployerLocation()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                Id = "1",
                FrameworkCode = 123,
                PathwayCode = 321,
                Level = 4,
                MarketingName = "Marketing name test",
                ProviderMarketingInfo = "Provider marketing info test",
                ApprenticeshipMarketingInfo = "Apprenticeship marketing info test",
                Phone = "123456789",
                Email = "test@test.com",
                ContactUsUrl = "www.contactus.com",
                ApprenticeshipInfoUrl = "www.apprenticeshipinfourl.com",
                Name = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1,
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                }
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
            GetPartial(html, ".result dl dd").Should().Be("1 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Address 1 Address 2 County PostCode");
        }
    }
}
