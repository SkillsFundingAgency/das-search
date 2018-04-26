using OpenQA.Selenium;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class ApprenticeshipPage : FatBasePage
    {
        protected override string PageTitle => "Accounting - Apprenticeship Framework - Find apprenticeship training";

        public ApprenticeshipPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public IWebElement Level => GetById("level");
        public IWebElement TypicalLength => GetById("length");
        public IWebElement SearchButton => GetById("ga-find-provider-bottom");

        public PostCodeSearchPage SearchForProvidersClick()
        {
            SearchButton.Click();
            return new PostCodeSearchPage(Driver);
        }
    }
}