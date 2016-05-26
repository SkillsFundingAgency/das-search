namespace Sfa.Das.Sas.WebTest.RegressionTests.pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.RegressionTests.pages.shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/Search")]
    public class SearchPage : SharedTemplatePage
    {
        [ElementLocator(Id="keywords")]
        public IWebElement SearchBox { get; set; }

        [ElementLocator(Id = "submit-keywords")]
        public IWebElement SearchButton { get; set; }
    }
}