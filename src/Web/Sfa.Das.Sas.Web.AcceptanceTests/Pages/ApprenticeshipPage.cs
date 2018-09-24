using System;
using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class ApprenticeshipPage : FatBasePage
    {
        protected override string PageTitle => "Accounting - Apprenticeship Framework - Find apprenticeship training";
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

        public void NavigateToFramework(Uri baseUrl, string FrameworkId, string keywords,string ukprn)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Apprenticeship/Framework/{FrameworkId}?Keyword={keywords}&ukprn={ukprn}"));
        }
        public void NavigateToStandard(Uri baseUrl, string StandardId, string keywords)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Apprenticeship/Standard/{StandardId}?Keyword={keywords}"));
        }

        public ApprenticeshipPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public IWebElement Level => GetById("level");
        public IWebElement TypicalLength => GetById("length");
        public IWebElement SearchButton => GetById("ga-find-provider-bottom");

        public void Navigate(Uri baseUrl,string keywords)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Apprenticeship/SearchResults?Keywords={keywords}"));
        }
        public PostCodeSearchPage SearchForProvidersClick()
        {
            SearchButton.Click();
            return new PostCodeSearchPage(Driver);
        }
    }
}