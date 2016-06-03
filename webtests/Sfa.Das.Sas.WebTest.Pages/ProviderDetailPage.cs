namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Provider/Detail")]
    public class ProviderDetailsPage : SharedTemplatePage
    {
        [ElementLocator(Id = "provider-name")]
        public IWebElement ProviderName { get; set; }

        [ElementLocator(CssSelector = "#bottom-actions .shortlist-link")]
        public IWebElement ProviderShortlistLink { get; set; }
    }
}