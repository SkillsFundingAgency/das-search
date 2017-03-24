namespace Sfa.Das.Sas.WebTest.Pages.Shared
{
    using OpenQA.Selenium;

    using SpecBind.Pages;

    public abstract class SharedTemplatePage
    {
        [ElementLocator(Id= "dashboard-link")]
        public IWebElement DashboardLink { get; set; }

        [ElementLocator(CssSelector = "h2.heading-large")]
        public IWebElement summary { get; set; }

        public string SummaryText { get { return summary.Text; } }
    }
}