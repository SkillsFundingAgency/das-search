namespace Sfa.Das.WebTest.Pages
{
    using System.Linq;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;
    using Sfa.Das.WebTest.Infrastructure.Selenium;

    [PageNavigation("/apprenticeship/searchresults")]
    public class SearchResultsPage : SharedTemplatePage
    {
        private readonly IWebDriver _driver;

        private readonly By _firstFrameworkSelector = By.CssSelector("#apprenticeship-results .framework-result a");

        private readonly By _firstStandardSelector = By.CssSelector("#apprenticeship-results .standard-result a");

        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement FirstStandardResult => _driver.FindElements(_firstStandardSelector).FirstOrDefault();

        public IWebElement FirstFrameworkResult => _driver.FindElements(_firstFrameworkSelector).FirstOrDefault();
    }
}