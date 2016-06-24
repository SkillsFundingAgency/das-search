using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Provider;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Provider
{
    [TestFixture]
    public sealed class StandardProviderSearchResultPage : ViewTestBase
    {
        [Test]
        public void ShouldEmptyModel()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel();
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("Sorry, there are currently no training providers for  for ''.");
        }

        [Test]
        public void ShouldShowAnErrorWhenSomethingIsWrong()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                Hits = new List<StandardProviderResultItemViewModel>(),
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
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There is 1 training provider for the apprenticeship: Test name.");
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
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("There are 7 training providers for the apprenticeship: Test name.");
        }

        [Test]
        public void ShouldHaveAllFieldsInSearchResult()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Distance = 1,
                Address = new Address(),
                Id = "1",
                LocationId = 2,
                StandardCode = 12
            };
            var item2 = new StandardProviderResultItemViewModel
            {
                Name = "Provider 2",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                Id = "1",
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item, item2 },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("1 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Training takes place at your location.");

            var secondResult = GetHtmlElement(html, ".result", 2);

            GetPartial(secondResult, "dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowJustDistanceIfDeliveryModeIsNotEmployerLocation()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                Id = "1",
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");
            GetPartial(html, ".result dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowTrainingLocationIfDeliveryModeContainsEmployerLocationButItIsNotTheOnlyOne()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer", "blockRelease" },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                Id = "1",
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("0 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Address 1 Address 2 Town County PostCode");
        }

        [Test]
        public void ShouldShowTrainingLocationIfDeliveryModeContainsEmployerLocationAndItIsTheOnlyOne()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                Id = "1",
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("0 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Training takes place at your location.");
        }

        [Test]
        public void ShouldShowProviderLocationIfDeliveryModeDoesNotContainHundredEmployerLocation()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
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
                },
                Id = "1",
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("3 miles away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Address 1 Address 2 Town County PostCode");
        }

        [Test]
        public void ShouldShowPercentageForSatisfactionResultWhenprovided()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                Name = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address(),
                EmployerSatisfactionMessage = "87%",
                LearnerSatisfactionMessage = "99.9%",
                Id = "1",
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel()
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-data-list dd", 3).Should().Be("87%");
            GetPartial(html, ".result-data-list dd", 4).Should().Be("99.9%");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsTheFirstPageShouldShowOnlyNextPageLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 20,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>
                {
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel()
                },
                ActualPage = 1,
                LastPage = 2,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".page-navigation__btn.prev").Should().BeEmpty();
            GetPartial(html, ".page-navigation__btn.next").Should().Contain("Next page").And.Contain("2 of 2");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsAPageInTheMiddleShouldShowBackAndNextPageLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 20,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>
                {
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel()
                },
                ActualPage = 2,
                LastPage = 3,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".page-navigation__btn.prev").Should().Contain("Previous page").And.Contain("1 of 3");
            GetPartial(html, ".page-navigation__btn.next").Should().Contain("Next page").And.Contain("3 of 3");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsTheLastPageShouldShowOnlyBackPageLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 20,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>
                {
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel(),
                    new StandardProviderResultItemViewModel()
                },
                ActualPage = 3,
                LastPage = 3,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".page-navigation__btn.prev").Should().Contain("Previous page").And.Contain("2 of 3");
            GetPartial(html, ".page-navigation__btn.next").Should().BeEmpty();
        }

        [Test]
        public void WhenSearchResultHasNoResultShouldShowUsefulInformationMessage()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false,
                TotalResultsForCountry = 0
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".return-search-results").Should().Be("return to your apprenticeship training search results");
            GetPartial(html, ".start-again").Should().Be("start your keyword search again");
        }

        [Test]
        public void WhenSearchResultHasNoResultAndTheraAreNoProvidersInCountryShouldntShowProvidersMessage()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false,
                TotalResultsForCountry = 0
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".total-providers-country").Should().BeEmpty();
        }

        [Test]
        public void WhenSearchResultHasNoResultButThereAreProvidersInCountryShouldShowCountMessage()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false,
                TotalResultsForCountry = 3
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".total-providers-country").Should().NotBeEmpty();
            var expectedText = string.Format("view all ({0}) training providers for Test standard name in England", model.TotalResultsForCountry);
            GetPartial(html, ".total-providers-country").Should().Be(expectedText);
        }

        [Test]
        public void WhenSearchResultHasResultButNoDeliveryModeHasResultsShouldShowFilterBox()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 10,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>() { new StandardProviderResultItemViewModel() },
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>
                {
                    new DeliveryModeViewModel
                    {
                        Count = 0
                    }
                },
                HasError = false,
                TotalResultsForCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".filter-box").Should().NotBeNull();
        }

        [Test]
        public void WhenSearchResultHasNoResultButDeliveryModeHasResultsShouldNotShowFilterBox()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>
                {
                    new DeliveryModeViewModel
                    {
                        Count = 10
                    }
                },
                HasError = false,
                TotalResultsForCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".filter-box").Should().BeNull();
        }

        [Test]
        public void WhenSearchResultHasNoResultAndNoDeliveryModeHasResultsShouldNotShowFilterBox()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>
                {
                    new DeliveryModeViewModel()
                },
                HasError = false,
                TotalResultsForCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".filter-box").Should().BeNull();
        }

        [Test]
        public void WhenSearchResultHasResultsShouldShowNewSearchLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>()
                {
                    new StandardProviderResultItemViewModel()
                },
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>
                {
                    new DeliveryModeViewModel()
                },
                HasError = false,
                TotalResultsForCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".new-postcode-search").Should().NotBeNull();
            GetPartial(html, ".new-postcode-search").Should().Be("Find providers for a different postcode");
        }

        [Test]
        public void WhenSearchResultHasNoResultsShouldNotShowNewSearchLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test standard name",
                Hits = new List<StandardProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>
                {
                    new DeliveryModeViewModel()
                },
                HasError = false,
                TotalResultsForCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".new-postcode-search").Should().BeNull();
            GetPartial(html, ".new-postcode-search").Should().BeEmpty();
        }
    }
}
