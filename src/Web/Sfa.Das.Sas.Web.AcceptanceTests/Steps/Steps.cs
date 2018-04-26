using NUnit.Framework;
using Sfa.Das.Sas.Web.AcceptanceTests.Pages;
using TechTalk.SpecFlow;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    [Binding]
    public class Steps : StepsBase
    {
        [Given(@"I navigated to the Search page")]
        public void GivenINavigatedToTheSearchPage()
        {
            var page = WebSite.NavigateToStartPage();
            var searchPage = page.ClickStartButton();
            Set(searchPage);
        }

        [When(@"I choose Search Button")]
        public void WhenIChooseSearchButton()
        {
            var searchPage = Get<SearchApprenticeshipPage>();
            Assert.IsTrue(searchPage.IsCurrentPage);
            Set(searchPage.SearchFor(""));
        }

        [When(@"I search for '(.*)'")]
        public void WhenISearchFor(string searchTerm)
        {
            var searchPage = Get<SearchApprenticeshipPage>();
            Assert.IsTrue(searchPage.IsCurrentPage);
            Set(searchPage.SearchFor(searchTerm));
        }
    }
}