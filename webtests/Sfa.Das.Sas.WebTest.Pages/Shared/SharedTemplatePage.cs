namespace Sfa.Das.Sas.WebTest.Pages.Shared
{
    using OpenQA.Selenium;

    using SpecBind.Pages;

    public abstract class SharedTemplatePage
    {
        [ElementLocator(Id= "dashboard-link")]
        public IWebElement DashboardLink { get; set; }
    }
}