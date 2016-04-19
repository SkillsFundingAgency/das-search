namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/standard")]
    public class StandardDetailsPage : BasePage
    {
        public By PostcodeSearchBox => By.CssSelector(".postcode-form-bottom .postcode-search-box");

        public By SearchButton => By.CssSelector(".postcode-form-bottom .postcode-search-button");
    }
}