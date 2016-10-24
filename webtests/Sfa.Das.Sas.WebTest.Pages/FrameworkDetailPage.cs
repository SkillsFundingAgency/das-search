namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/Framework")]
    public class FrameworkDetailsPage : SharedTemplatePage
    {
        [ElementLocator(CssSelector = "#bottom-actions .ui-find-training-providers")]
        public IWebElement SearchPageButton { get; set; }

        [ElementLocator(CssSelector = ".column-two-thirds .heading-xlarge")]
        public IWebElement FrameworkHeading { get; set; }
    }
}