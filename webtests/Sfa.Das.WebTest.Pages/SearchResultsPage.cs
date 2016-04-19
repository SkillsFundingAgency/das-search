namespace Sfa.Das.WebTest.Pages
{
    using System.Linq;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/searchresults")]
    public class SearchResultsPage
    {
        private readonly IWebDriver _driver;

        private readonly By _firstFrameworkSelector = By.CssSelector("#apprenticeship-results article[id^=framework] a");

        private readonly By _firstStandardSelector = By.CssSelector("#apprenticeship-results article[id^=standard] a");

        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement FirstStandardResult => _driver.FindElements(_firstStandardSelector).FirstOrDefault();

        public IWebElement FirstFrameworkResult => _driver.FindElements(_firstFrameworkSelector).FirstOrDefault();
    }
}