using NUnit.Framework;
using Sfa.Das.Sas.Web.AcceptanceTests.Pages;
using System.Linq;
using TechTalk.SpecFlow;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    [Binding]
    public class SearchSteps : StepsBase
    {
        [Given(@"I navigated to the Search Results page")]
        public void GivenINavigatedToTheSearchResultsPage()
        {
            var page = WebSite.NavigateToSearchResultPage();
            Set(page);
        }


        [Then(@"I am on the Search Results page")]
        public void ThenIAmOnTheSearchResultsPage()
        {
            var searchResultPage = Get<SearchResultPage>();
            Assert.IsTrue(searchResultPage.IsCurrentPage);
        }

        [Then(@"all elements exists on page")]
        public void ThenAllElementsExistsOnPage()
        {
            var searchResultPage = Get<SearchResultPage>();
            Assert.IsTrue(searchResultPage.FirstStandardResult.Displayed);
            Assert.IsTrue(searchResultPage.SortingDropdown.Displayed);
            Assert.IsTrue(searchResultPage.FilterBlock.Displayed);
        }

        [When(@"I choose Level (.*) Checkbox")]
        public void WhenIChooseLevelCheckbox(int level)
        {
            var searchResultPage = Get<SearchResultPage>();

            var resultsb4 = searchResultPage.GetAllResults();
            Assert.IsTrue(resultsb4.Count(m => m.Level.StartsWith($"{level}")) != resultsb4.Count(), message: $"All results should not be level: {level} before filtering");

            searchResultPage.SelectLevel(level);
        }

        [When(@"all results are level (.*)")]
        public void WhenAllResultsAreLevel(int level)
        {
            var searchResultPage = Get<SearchResultPage>();
            var results = searchResultPage.GetAllResults();

            Assert.IsTrue(results.All(m => m.Level.StartsWith($"{level}")), message: $"All results should be level: {level}");
        }

        [When(@"I choose High to Low Option Selector")]
        public void WhenIChooseHighToLowOptionSelector()
        {
            var searchResultPage = Get<SearchResultPage>();
            searchResultPage.SortBy("Level (high to low)");
        }

        [Then(@"results are sorted from High to Low")]
        public void ThenResultsAreSordtedFromHighToLow()
        {
            var searchResultPage = Get<SearchResultPage>();
            var results = searchResultPage.GetAllResults();
            CollectionAssert.IsOrdered(results.Select(m => m.Level).Reverse());
        }

        [When(@"I choose Low to High Option Selector")]
        public void WhenIChooseLowToHighOptionSelector()
        {
            var searchResultPage = Get<SearchResultPage>();
            searchResultPage.SortBy("Level (low to high)");
        }

        [Then(@"results are sorted from Low to High")]
        public void ThenResultsAreSortedFromLowToHigh()
        {
            var searchResultPage = Get<SearchResultPage>();
            var results = searchResultPage.GetAllResults();
            CollectionAssert.IsOrdered(results.Select(m => m.Level));
        }

    }
}