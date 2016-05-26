namespace Sfa.Das.Sas.WebTest.RegressionTests.pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.RegressionTests.pages.shared;

    using SpecBind.Pages;

    [PageNavigation("/Provider/Detail")]
    public class ProviderDetailsPage : SharedTemplatePage
    {
        [ElementLocator(Id = "provider-name")]
        public IWebElement ProviderName { get; set; }
    }
}