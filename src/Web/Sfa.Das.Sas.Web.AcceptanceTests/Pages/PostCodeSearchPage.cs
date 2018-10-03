using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class PostCodeSearchPage : FatBasePage
    {
        protected override string PageTitle => "Search for providers - Find apprenticeship training";
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

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