using OpenQA.Selenium;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class PostCodeSearchPage : FatBasePage
    {
        protected override string PageTitle => "Search for providers - Find apprenticeship training";

        public PostCodeSearchPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public IWebElement Lede => GetByCss("p.lede");

        public IWebElement SearchBox => GetById("search-box");

        public IWebElement LevyPaying => GetById("levyPaying");

        public IWebElement NotLevyPaying => GetById("notLevyPaying");

        public IWebElement SearchButton => GetByClass("postcode-search-button");

        public ProviderLocationSearchResultPage ClickSearchButton()
        {
            SearchButton.Click();
            return new ProviderLocationSearchResultPage(Driver);
        }
    }
}