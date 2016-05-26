namespace Sfa.Das.Sas.WebTest.RegressionTests.pages.shared
{
    using OpenQA.Selenium;

    using SpecBind.Pages;
    using SpecBind.Selenium;

    public class ProviderResultsBase : SharedTemplatePage
    {
        [ElementLocator(CssSelector = "#provider-results article.result:nth-of-type(1) .result-title a")]
        public IWebElement FirstProviderLink { get; set; }

        [ElementLocator(CssSelector = "#provider-results")]
        public IElementList<IWebElement, ProviderResultItem> ProviderResults { get; set; }

        [ElementLocator(Class="result")]
        public class ProviderResultItem : WebElement
        {
            public ProviderResultItem(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            [ElementLocator(CssSelector = ".result-title > a")]
            public IWebElement Title { get; set; }
        }
    }
}