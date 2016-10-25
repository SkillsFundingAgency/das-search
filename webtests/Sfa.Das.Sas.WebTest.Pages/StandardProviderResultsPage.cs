namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;
    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Provider/StandardResults")]
    public class StandardProviderResultsPage : ProviderResultsBase
    {
        public IWebElement FirstStandardProviderResult { get; set; }
    }
}