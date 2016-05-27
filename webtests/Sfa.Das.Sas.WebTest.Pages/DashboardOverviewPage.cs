namespace Sfa.Das.Sas.WebTest.RegressionTests.pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;
    using SpecBind.Selenium;

    [PageNavigation("/Dashboard/Overview")]
    public class DashboardOverviewPage : SharedTemplatePage
    {
        [ElementLocator(Id = "empty-shortlist-message")]
        public IWebElement EmptyShortlistMessage { get; set; }

        [ElementLocator(Id = "standards-shortlist")]
        public IElementList<IWebElement, ShortlistItem> StandardsShortlist { get; set; }

        [ElementLocator(Class = "standard-item")]
        public class ShortlistItem : WebElement
        {
            public ShortlistItem(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            [ElementLocator(CssSelector = ".standard-title > a")]
            public IWebElement Title { get; set; }
        }
    }
}