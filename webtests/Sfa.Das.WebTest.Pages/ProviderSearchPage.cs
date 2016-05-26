namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/SearchForProviders")]
    public class ProviderSearchPage : SharedTemplatePage
    {
        public By PostcodeSearchBox => By.CssSelector("#search-box");

        public By SearchButton => By.CssSelector("input.postcode-search-button");

    }
}