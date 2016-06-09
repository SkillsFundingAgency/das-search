namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    [PageNavigation("/Apprenticeship/Search")]
    public class SearchPage : SharedTemplatePage
    {
        [ElementLocator(Id="keywords")]
        public IWebElement SearchBox { get; set; }

        [ElementLocator(Id = "submit-keywords")]
        public IWebElement SearchButton { get; set; }

        [ElementLocator(CssSelector = ".footer__meta")]
        public IWebElement FooterElement { get; set; }
    }
}