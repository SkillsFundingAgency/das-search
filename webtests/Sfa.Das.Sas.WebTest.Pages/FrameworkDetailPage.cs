namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/Framework")]
    public class FrameworkDetailsPage : SharedTemplatePage
    {
        [ElementLocator(Class = "ui-find-training-providers")]
        public IWebElement SearchPageButton { get; set; }
    }
}