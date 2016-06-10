using OpenQA.Selenium;
using Sfa.Das.Sas.WebTest.Pages.Shared;
using SpecBind.Pages;
using SpecBind.Selenium;

namespace Sfa.Das.Sas.WebTest.Pages
{
    [PageNavigation("/Dashboard/Overview")]
    public class DashboardOverviewPage : SharedTemplatePage
    {
        [ElementLocator(Id = "empty-shortlist-message")]
        public IWebElement EmptyShortlistMessage { get; set; }

        [ElementLocator(Class = "apprenticeship-items")]
        public IElementList<IWebElement, ApprenticeshipShortlistItem> Shortlist { get; set; }

        [ElementLocator(Class = "apprenticeship-items")]
        public IElementList<IWebElement, StandardShortlistItem> StandardShortlist { get; set; }

        [ElementLocator(Class = "apprenticeship-items")]
        public IElementList<IWebElement, FrameworkShortlistItem> FrameworkShortlist { get; set; }
        

        [ElementLocator(Class = "apprenticeship-item")]
        public class ApprenticeshipShortlistItem : WebElement
        {
            public ApprenticeshipShortlistItem(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            [ElementLocator(CssSelector = ".apprenticeship-title > a")]
            public IWebElement Title { get; set; }
        }

        [ElementLocator(Class = "apprenticeship-item")]
        public class StandardShortlistItem : WebElement
        {
            public StandardShortlistItem(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            [ElementLocator(CssSelector = ".apprenticeship-title > a")]
            public IWebElement Title { get; set; }
        }

        [ElementLocator(Class = "apprenticeship-item")]
        public class FrameworkShortlistItem : WebElement
        {
            public FrameworkShortlistItem(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            [ElementLocator(CssSelector = ".apprenticeship-title > a")]
            public IWebElement Title { get; set; }
        }

    }
}