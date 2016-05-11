namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/Dashboard/Overview")]
    public class DashboardOverviewPage
    {
        public By StandardsShortlist => By.Id("standards-shortlist");

        public By EmptyShortlistMessage => By.Id("empty-shortlist-message");
    }
}