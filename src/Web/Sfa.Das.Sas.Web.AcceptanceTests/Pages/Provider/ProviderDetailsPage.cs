using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages.Apprenticeships
{
    internal class ProviderDetailsPage : FatBasePage
    {
        public ProviderDetailsPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageTitle => "Provider Details for ";
        private ICollection<IWebElement> _trainingProviderResultsList => Driver.FindElements(By.ClassName("results"));
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }
        public bool Verify(string providerName)
        {
            return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle, providerName);
        }

        public void Navigate(Uri baseUrl, string ukprn)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Provider/{ukprn}"));
        }
        public void Navigate(Uri baseUrl, string ukprn,string keyword)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Provider/{ukprn}?keyword={keyword}"));
        }


    }
}
