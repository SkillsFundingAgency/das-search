namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;
    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Provider/FrameworkResults")]
    public class FrameworkProviderResultsPage : ProviderResultsBase
    {
        //[ElementLocator(CssSelector = "#apprenticeship-results .standard-result a")]
        public IWebElement FirstFrameworkProviderResult { get; set; }

    }
}