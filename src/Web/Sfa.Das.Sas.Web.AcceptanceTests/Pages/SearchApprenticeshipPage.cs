using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class SearchApprenticeshipPage : FatBasePage
    {
        protected override string PageTitle => "Home Page - Find apprenticeship training";

        public override bool Verify()
        {
           return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

        public SearchApprenticeshipPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public IWebElement SearchButton => GetById("submit-keywords");
        
        public IWebElement SearchBox => GetById("keywords");

        public ApprenticeshipSearchResultPage SearchFor(string v)
        {
            SearchBox.SendKeys(v);
            SearchButton.Click();
            return new ApprenticeshipSearchResultPage(Driver);
        }
    }
}