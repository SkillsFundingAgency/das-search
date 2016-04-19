namespace Sfa.Das.WebTest.AcceptanceTests.Pages
{
    using System.Linq;

    using OpenQA.Selenium;

    public class SearchResultsPage : BasePage
    {
        private By resultsContainer = By.Id("apprenticeship-results");

        private By searchResultItem = By.CssSelector("#apprenticeship-results article.result");

        private By itemLink = By.CssSelector(".result-title > a");

        By searchkeywordresult = By.CssSelector("#apprenticeship-results .result:nth-child(1) #result-title a");

        public void WaitForLoad()
        {
            WaitFor(resultsContainer);
        }

        public void chooseStandard()
        {
            var firstResultLink = FindElements(searchResultItem).First().FindElement(itemLink);
            firstResultLink.Click();
        }

        public void searchChooseStandard(string keyword)
        {
            searchNSelect(searchkeywordresult, keyword);
        }
    }
}