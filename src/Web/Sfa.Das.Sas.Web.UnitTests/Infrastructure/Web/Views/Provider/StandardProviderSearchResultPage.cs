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
    using System.Linq;

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
                StandardId = "1",
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturned()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                StandardLevel = 2,
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 1 }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There is 1 training option for the Test name, level 2 apprenticeship.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturned()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                StandardLevel = 3,
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 1 }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There are 7 training options for the Test name, level 3 apprenticeship.");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturnedInAllCountry()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                StandardLevel = 2,
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel { Count = 1 },
                ShowAll = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There is 1 training option for the Test name, level 2 apprenticeship in England.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturnedInAllCountry()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                StandardLevel = 3,
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel { Count = 1 },
                ShowAll = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There are 7 training options for the Test name, level 3 apprenticeship in England.");
        }

        [Test]
        public void ShouldShowMessageInformingAboutNationalLabel()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                StandardLevel = 3,
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel { Count = 1 },
                ShowAll = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p", 3).Should().Contain("Results labelled National are training options run by providers who are willing to offer training across England.");
        }

        [TestCase(7, 0)]
        [TestCase(0, 7)]
        [TestCase(0, 0)]
        public void ShouldNotShowMessageInformingAboutNationalLabel(int totalResults, int nationalProviders)
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = totalResults,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                StandardLevel = 3,
                PostCode = "N17",
                Hits = new List<StandardProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel { Count = nationalProviders },
                ShowAll = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p", 2).Should().NotStartWith("Results labelled National are training providers");
            GetPartial(html, "p", 3).Should().NotStartWith("Results labelled National are training providers");
            GetPartial(html, "p", 3).Should().BeEmpty();
        }

        [Test]
        public void ShouldHaveAllFieldsInSearchResult()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Distance = 1,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };
            var item2 = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 2",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item, item2 },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("1 mile away");

            var secondResult = GetHtmlElement(html, ".result", 2);

            GetPartial(secondResult, "dl dd").Should().Be("1.2 miles away");
        }

        [Test]
        public void ShouldShowDeliveryModesWhenThereIsJustOne()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12,
                DeliveryOptionsMessage = "block release"
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".deliveryOptions").Should().Be("block release");
        }

        [Test]
        public void ShouldShowDeliveryModesWhenThereAreSeveralOnes()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12,
                DeliveryOptionsMessage = "block release, at your location"
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".deliveryOptions").Should().Be("block release, at your location");
        }

        [Test]
        public void ShouldShowDeliveryModesWithCorrectOrder()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer", "DayRelease" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12,
                DeliveryOptionsMessage = "day release, block release, at your location"
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".deliveryOptions").Should().Be("day release, block release, at your location");
        }

        [Test]
        public void ShouldShowJustDistanceIfDeliveryModeIsNotEmployerLocation()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
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
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer", "BlockRelease" },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                LocationAddressLine = "Address 1, Address 2, Town, County, PostCode",
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("0 miles away");

            GetPartial(html, ".address").Should().Be("Address 1, Address 2, Town, County, PostCode");
        }

        [Test]
        public void ShouldntShowTrainingLocationIfDeliveryModeContainsEmployerLocationAndItIsTheOnlyOne()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("0 miles away");

            GetPartial(html, ".address").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldShowProviderLocationIfDeliveryModeDoesNotContainHundredEmployerLocation()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
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
                LocationAddressLine = "Address 1, Address 2, Town, County, PostCode",
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result dl dt").Should().Be("Distance:");

            GetPartial(html, ".result dl dd").Should().Be("3 miles away");

            GetPartial(html, ".address").Should().Be("Address 1, Address 2, Town, County, PostCode");
        }

        [Test]
        public void ShouldShowPercentageForSatisfactionResultWhenprovided()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address(),
                EmployerSatisfactionMessage = "87%",
                LearnerSatisfactionMessage = "99.9%",
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel()
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-data-list dd", 3).Should().Be("87%");
            GetPartial(html, ".result-data-list dd", 4).Should().Be("99.9%");
        }

        [Test]
        public void ShouldShowPercentageForAchievementRateResultWhenprovided()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address(),
                EmployerSatisfactionMessage = "87%",
                LearnerSatisfactionMessage = "99.9%",
                AchievementRateMessage = "42.5%",
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel()
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-data-list dt", 5).Should().Be("Achievement rate:");
            GetPartial(html, ".result-data-list dd", 5).Should().Be("42.5%");
        }

        [Test]
        public void ShouldIncludeNationalTagIfItsANationalProvider()
        {
            var page = new StandardProviderInformation();
            var item = new StandardProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                NationalProvider = true,
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Address = new Address(),
                EmployerSatisfactionMessage = "87%",
                LearnerSatisfactionMessage = "99.9%",
                AchievementRateMessage = "42.5%",
                UkPrn = 1,
                LocationId = 2,
                StandardCode = 12
            };

            var model = new ProviderStandardSearchResultViewModel()
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test name",
                Hits = new List<StandardProviderResultItemViewModel> { item },
                HasError = false
            };
            var html = page.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-title").Should().EndWith("National");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsTheFirstPageShouldShowOnlyNextPageLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 20,
                PostCodeMissing = false,
                StandardId = "1",
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
                StandardId = "1",
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
                StandardId = "1",
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
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = "1",
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
            GetPartial(html, ".start-again").Should().Be("start your job role or keyword search again");
        }

        [Test]
        public void WhenSearchResultHasNoResultAndTheraAreNoProvidersInCountryShouldntShowProvidersMessage()
        {
            var detail = new StandardSearchResultMessage();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = "1",
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
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = "1",
                StandardName = "Test standard name",
                StandardLevel = 3,
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
            var expectedText = string.Format("view all ({0}) training providers for Test standard name, level 3 in England", model.TotalResultsForCountry);
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
                StandardId = "1",
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
                StandardId = "1",
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
                StandardId = "1",
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
                StandardId = "1",
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
                StandardId = "1",
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

        [TestCase(1, "1-3", 1)]
        [TestCase(2, "1-3", 1)]
        [TestCase(3, "1-3", 1)]
        [TestCase(4, "4-6", 1)]
        [TestCase(5, "4-6", 1)]
        [TestCase(6, "4-6", 1)]
        [TestCase(7, "7-9", 1)]
        [TestCase(8, "7-9", 1)]
        [TestCase(9, "7-9", 1)]
        [TestCase(10, "All Others", 1)]
        [TestCase(1, "All Others", 0)]
        [TestCase(1, "All Others", -1)]
        [TestCase(1, "All Others", 2, Description = "On second page")]
        [TestCase(4, "All Others", 2, Description = "On second page")]
        [TestCase(7, "All Others", 2, Description = "On second page")]
        public void ShouldAddIntervalToResultsForGoogleAnalytics(int resultIndex, string expectedText, int acctualPage)
        {
            var searchPage = new StandardProviderInformation();

            var results = from ll in new StandardProviderResultItemViewModel[10] select new StandardProviderResultItemViewModel { ProviderName = "Test" };

            var model = new ProviderStandardSearchResultViewModel { TotalResults = 10, Hits = results, ActualPage = acctualPage, ResultsToTake = 10, };
            var html = searchPage.RenderAsHtml(model).ToAngleSharp();
            GetAttribute(html, "article a", "attr-ga-result-interval", resultIndex).Should().BeEquivalentTo(expectedText);
        }

        [Test]
        public void ShouldHaveDataForGoogleAnalytic()
        {
            var searchPage = new StandardResults();
            var nameOfStandard = "Name of standard";
            var level = 2;
            var title = $"{nameOfStandard}, level {level}";
            var postcode = "N17";
            var model = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                StandardName = nameOfStandard,
                StandardLevel = level,
                PostCode = postcode,
                Hits = new StandardProviderResultItemViewModel [0]
            };

            var html = searchPage.RenderAsHtml(model).ToAngleSharp();

            GetAttribute(html, "#ga-apprenticeship-title", "value").Should().BeEquivalentTo(title);
            GetAttribute(html, "#ga-postcode", "value").Should().BeEquivalentTo(postcode);
        }

        [Test]
        public void ShouldDetermineEmptyResultsForGoogleAnalytic()
        {
            var searchPage = new StandardResults();
            var modelWithResults = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                Hits = new[] { new StandardProviderResultItemViewModel() }
            };

            var modelWithoutResults = new ProviderStandardSearchResultViewModel
            {
                TotalResults = 1,
                Hits = new StandardProviderResultItemViewModel[0]
            };

            var htmlWithResults = searchPage.RenderAsHtml(modelWithResults).ToAngleSharp();
            var htmlWithoutResults = searchPage.RenderAsHtml(modelWithoutResults).ToAngleSharp();

            GetAttribute(htmlWithResults, "#ga-no-result", "value").Should().BeEquivalentTo("False");
            GetAttribute(htmlWithoutResults, "#ga-no-result", "value").Should().BeEquivalentTo("True");
        }
    }
}
