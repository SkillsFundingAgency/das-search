namespace Sfa.Das.Sas.WebTest.RegressionTests.pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.RegressionTests.pages.shared;

    using SpecBind.Pages;

    [PageNavigation("/")]
    public class StartPage : SharedTemplatePage
    {
        [ElementLocator(Id = "start-button")]
        public IWebElement StartButton { get; set; }
    }
}