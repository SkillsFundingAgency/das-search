namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/SearchForProviders")]
    public class ProviderSearchPage : SharedTemplatePage
    {
        [ElementLocator(Id = "search-box")]
        public IWebElement PostcodeSearchBox { get; set; }

        [ElementLocator(Class = "postcode-search-button")]
        public IWebElement SearchButton { get; set; }

        [ElementLocator(CssSelector = ".postcode-form .error-message")]
        public IWebElement ErrorMessage { get; set; }
    }
}