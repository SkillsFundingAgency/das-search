namespace Sfa.Das.Sas.WebTest.RegressionTests.pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.RegressionTests.pages.shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/Standard")]
    public class StandardDetailsPage : SharedTemplatePage
    {
        [ElementLocator(CssSelector = "a.ui-find-training-providers")]
        public IWebElement SearchPageButton { get; set; }
        
        [ElementLocator(Class = "shortlist-link")]
        public IWebElement ShortlistLink { get; set; }
    }
}