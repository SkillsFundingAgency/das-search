namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/Standard")]
    public class StandardDetailsPage : SharedTemplatePage
    {
        [ElementLocator(CssSelector = "#bottom-actions .ui-find-training-providers")]
        public IWebElement SearchPageButton { get; set; }

        [ElementLocator(CssSelector = ".shortlist-link")]
        public IWebElement ShortlistLink { get; set; }
    }
}