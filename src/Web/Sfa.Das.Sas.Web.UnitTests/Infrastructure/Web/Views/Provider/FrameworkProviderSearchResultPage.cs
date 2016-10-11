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
            var detail = new FrameworkResults();
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
            GetPartial(html, ".result-message");
            this.GetPartial(html, ".result-message").Should().Contain("Sorry, there are currently no training providers for Test name: Pathway test name, level 3 for 'Test postcode'.");
        }

        [Test]
        public void ShouldShowZeroResultsMessageWhenThereAreNoResultsAndThereIsNoLevel()
        {
            var detail = new FrameworkResults();
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

            GetPartial(html, ".result-message").Should().Contain("Sorry, there are currently no training providers for Test name: Pathway test name, level 0 for 'Test postcode'.");
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
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 1 }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-message p").Should().Contain("1 training provider for the Test name: Pathway test name, level 3 apprenticeship.");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturnedInAllCountry()
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
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 1 },
                ShowAll = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-message p").Should().Contain("1 training provider for the Test name: Pathway test name, level 3 apprenticeship in England.");
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
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 1 }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-message p").Should().Contain("1 training provider for the Test name: Pathway test name, level 0 apprenticeship.");

            GetPartial(html, "p").Should().Contain("1 training provider for the Test name: Pathway test name, level 0 apprenticeship.");
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
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 1 }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, ".result-message p").Should().Contain("7 training providers for the Test name: Pathway test name, level 2 apprenticeship.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturnedInAllCountry()
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
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 1 },
                ShowAll = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".result-message p").Should().Contain("7 training providers for the Test name: Pathway test name, level 2 apprenticeship in England.");
        }

        [Test]
        public void ShouldShowMessageInformingAboutNationalLabel()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test name",
                FrameworkLevel = 3,
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                HasError = false,
                NationalProviders = new NationalProviderViewModel { Count = 1 },
                ShowAll = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p", 3).Should().Contain("Results labelled National are training providers who are willing to offer apprenticeship training across England.");
        }

        [TestCase(7, 0)]
        [TestCase(0, 7)]
        [TestCase(0, 0)]
        public void ShouldNotShowMessageInformingAboutNationalLabel(int totalResults, int nationalProviders)
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = totalResults,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test name",
                FrameworkLevel = 3,
                PostCode = "N17",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
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
                HasError = false,
                NationalProviders = new NationalProviderViewModel() { Count = 0 }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "p").Should().Contain("7 training providers for the Test name: Pathway test name, level 0 apprenticeship.");
        }

        [Test]
        public void ShouldShowDeliveryModesWhenThereIsJustOne()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                FrameworkId = "3",
                DeliveryOptionsMessage = "block release"
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

            GetPartial(html, ".deliveryOptions").Should().Be("block release");
        }

        [Test]
        public void ShouldShowDeliveryModesWhenThereAreSeveralOnes()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                FrameworkId = "3",
                DeliveryOptionsMessage = "block release, at your location"
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

            GetPartial(html, ".deliveryOptions").Should().Be("block release, at your location");
        }

        [Test]
        public void ShouldShowDeliveryModesWithCorrectOrder()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease", "100PercentEmployer", "DayRelease" },
                Distance = 1.2,
                Address = new Address(),
                UkPrn = 1,
                LocationId = 2,
                FrameworkId = "1",
                DeliveryOptionsMessage = "day release, block release, at your location"
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

            GetPartial(html, ".deliveryOptions").Should().Be("day release, block release, at your location");
        }

        [Test]
        public void ShouldHaveAllFieldsInSearchResult()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer" },
                Distance = 0.3,
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address(),
                LocationId = 2,
                UkPrn = 12,
                FrameworkId = "3"
            };

            var item2 = new FrameworkProviderResultItemViewModel
            {
                ProviderName = "Provider 2",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                LocationId = 2,
                UkPrn = 12,
                FrameworkId = "3"
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
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "BlockRelease" },
                Distance = 1.2,
                Address = new Address(),
                LocationId = 2,
                UkPrn = 12,
                FrameworkId = "3"
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
                ProviderName = "Provider 1",
                DeliveryModes = new List<string> { "100PercentEmployer", "BlockRelease" },
                Distance = 3,
                Website = "http://www.trainingprovider.co.uk",
                Address = new Address
                {
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    County = "County",
                    Postcode = "PostCode",
                    Town = "Town"
                },
                LocationAddressLine = "Address 1, Address 2, Town, County, PostCode",
                LocationId = 2,
                UkPrn = 12,
                FrameworkId = "3"
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

            GetPartial(html, ".address").Should().Be("Address 1, Address 2, Town, County, PostCode");
        }

        [Test]
        public void ShouldntShowEmployerLocationIfDeliveryModeContainsEmployerLocationAndIsTheOnlyOne()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                ProviderName = "Provider 1",
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
                },
                LocationId = 2,
                UkPrn = 12,
                FrameworkId = "3"
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

            GetPartial(html, ".address").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldShowTrainingLocationIfDeliveryModeHasLocationDifferentToEmployerLocation()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                UkPrn = 1,
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
                ProviderName = "Provider 1",
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
                },
                LocationAddressLine = "Address 1, Address 2, Town, County, PostCode",
                LocationId = 2,
                FrameworkId = "3"
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
            GetPartial(html, ".result dl dd").Should().Be("1 mile away");

            GetPartial(html, ".result dl dd", 2).Should().Be("Address 1, Address 2, Town, County, PostCode");
        }

        [Test]
        public void ShouldIncludeNationalTagIfItsANationalProvider()
        {
            var page = new FrameworkProviderInformation();
            var item = new FrameworkProviderResultItemViewModel
            {
                UkPrn = 1,
                FrameworkCode = 123,
                PathwayCode = 321,
                Level = 4,
                MarketingName = "Marketing name test",
                ProviderMarketingInfo = "Provider marketing info test",
                ApprenticeshipMarketingInfo = "Apprenticeship marketing info test",
                NationalProvider = true,
                Phone = "123456789",
                Email = "test@test.com",
                ContactUsUrl = "www.contactus.com",
                ApprenticeshipInfoUrl = "www.apprenticeshipinfourl.com",
                ProviderName = "Provider 1",
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
                },
                LocationId = 2,
                FrameworkId = "3"
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

            GetPartial(html, ".result-title").Should().EndWith("National");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsTheFirstPageShouldShowOnlyNextPageLink()
        {
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
                TotalResults = 20,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkLevel = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                Hits = new List<FrameworkProviderResultItemViewModel>
                {
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel()
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
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
                TotalResults = 30,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkLevel = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                Hits = new List<FrameworkProviderResultItemViewModel>
                {
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel()
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
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                Title = "Test name: Pathway test name",
                TotalResults = 30,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkCode = 2,
                FrameworkLevel = 2,
                FrameworkName = "Test name",
                PathwayName = "Pathway test name",
                Hits = new List<FrameworkProviderResultItemViewModel>
                {
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel(),
                    new FrameworkProviderResultItemViewModel()
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
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test framework name",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false,
                TotalProvidersCountry = 0
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".return-search-results").Should().Be("return to your apprenticeship training search results");
            GetPartial(html, ".start-again").Should().Be("start your job role or keyword search again");
        }

        [Test]
        public void WhenSearchResultHasNoResultAndTheraAreNoProvidersInCountryShouldntShowProvidersMessage()
        {
            var detail = new FrameworkSearchResultMessage();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test framework name",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false,
                TotalProvidersCountry = 0
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".total-providers-country").Should().BeEmpty();
        }

        [Test]
        public void WhenSearchResultHasNoResultButThereAreProvidersInCountryShouldShowCountMessage()
        {
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test framework name",
                FrameworkLevel = 2,
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>(),
                HasError = false,
                TotalProvidersCountry = 3
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var expectedText = string.Format("view all ({0}) training providers for Test framework name, level 2 in England", model.TotalProvidersCountry);
            GetPartial(html, ".total-providers-country").Should().Be(expectedText);
        }

        [Test]
        public void WhenSearchResultHasNoResultAndNoDeliveryModeHasResultsShouldNotShowFilterBox()
        {
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test framework name",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>
                {
                    new DeliveryModeViewModel()
                },
                HasError = false,
                TotalProvidersCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".filter-box").Should().BeNull();
        }

        [Test]
        public void WhenSearchResultHasResultsShouldShowNewSearchLink()
        {
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test framework name",
                Hits = new List<FrameworkProviderResultItemViewModel>()
                {
                    new FrameworkProviderResultItemViewModel()
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
                TotalProvidersCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".new-postcode-search").Should().NotBeNull();
            GetPartial(html, ".new-postcode-search").Should().Be("Find providers for a different postcode");
        }

        [Test]
        public void WhenSearchResultHasNoResultsShouldNotShowNewSearchLink()
        {
            var detail = new FrameworkResults();
            var model = new ProviderFrameworkSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                FrameworkId = 1,
                FrameworkName = "Test framework name",
                Hits = new List<FrameworkProviderResultItemViewModel>(),
                ActualPage = 1,
                LastPage = 1,
                ResultsToTake = 10,
                PostCode = "Test postcode",
                DeliveryModes = new List<DeliveryModeViewModel>
                {
                    new DeliveryModeViewModel()
                },
                HasError = false,
                TotalProvidersCountry = 3,
                AbsolutePath = "www.abba.co.uk"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetHtmlElement(html, ".new-postcode-search").Should().BeNull();
            GetPartial(html, ".new-postcode-search").Should().BeEmpty();
        }
    }
}
