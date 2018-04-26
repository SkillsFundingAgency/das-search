using OpenQA.Selenium;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class SearchApprenticeshipPage : FatBasePage
    {
        protected override string PageTitle => "Home Page - Find apprenticeship training";

        public SearchApprenticeshipPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public IWebElement SearchButton => GetById("submit-keywords");

        public IWebElement Heading => GetByCss("h1.heading-xlarge");

        public SearchResultPage SearchFor(string v)
        {
            SearchButton.Click();
            return new SearchResultPage(Driver);
        }
    }
}