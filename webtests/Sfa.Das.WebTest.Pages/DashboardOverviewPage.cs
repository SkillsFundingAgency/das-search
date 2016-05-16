namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/Dashboard/Overview")]
    public class DashboardOverviewPage : SharedTemplatePage
    {
        public By StandardsShortlist => By.Id("standards-shortlist");

        public By EmptyShortlistMessage => By.Id("empty-shortlist-message");

        public By StandardItems => By.ClassName("standard-item");

        public By StandardTitles => By.ClassName("standard-title");

        public By StandardDeleteLinks => By.ClassName("standard-delete-link");

        public By DeleteLink => By.LinkText("Delete");

    }
}