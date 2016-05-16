namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    public abstract class SharedTemplatePage
    {
        public By DashboardLink => By.Id("dashboard-link");
    }
}