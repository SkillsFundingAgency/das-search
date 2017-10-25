namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/Framework")]
    public class FrameworkDetailsPage : SharedTemplatePage
    {
        [ElementLocator(CssSelector = "#ga-find-provider-bottom")]
        public IWebElement SearchPageButton { get; set; }

        [ElementLocator(CssSelector = ".column-two-thirds .heading-xlarge")]
        public IWebElement FrameworkHeading { get; set; }
    }
}