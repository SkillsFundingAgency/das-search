namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    using System.Linq;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;
    public class SearchResultsPage : BasePage
    {
        private By resultsContainer = By.Id("standard-results");
        private By searchResultItem = By.CssSelector("#standard-results article.result");
        private By itemLink = By.CssSelector(".result-title > a");


        public void WaitForLoad()
        {
            WaitFor(resultsContainer);
        }

        public void chooseStandard()
        {
            var firstResultLink = FindElements(searchResultItem).First().FindElement(itemLink);
            firstResultLink.Click();
        }
    }
}