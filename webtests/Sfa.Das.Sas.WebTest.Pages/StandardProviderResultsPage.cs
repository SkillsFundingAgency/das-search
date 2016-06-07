namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;
    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Provider/StandardResults")]
    public class StandardProviderResultsPage : ProviderResultsBase
    {
        //[ElementLocator(CssSelector = "#apprenticeship-results .standard-result a")]
        public IWebElement FirstStandardProviderResult { get; set; }
    }
}