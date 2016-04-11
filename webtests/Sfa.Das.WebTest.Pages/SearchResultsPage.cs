namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/searchresults")]
    public class SearchResultsPage : BasePage
    {
        public By FirstResultItem => By.XPath(".//*[@id='standard-results']/div[1]/article/header/h2/a");
    }
}