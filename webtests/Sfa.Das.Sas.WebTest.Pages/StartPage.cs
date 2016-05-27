namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/")]
    public class StartPage : SharedTemplatePage
    {
        [ElementLocator(Id = "start-button")]
        public IWebElement StartButton { get; set; }
    }
}