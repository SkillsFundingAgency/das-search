using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages.Apprenticeships
{
    internal class ProviderSearchResultsPage : FatBasePage
    {
        public ProviderSearchResultsPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageTitle => "Search Results - Find apprenticeship training";
        private ICollection<IWebElement> _trainingProviderResultsList => Driver.FindElements(By.ClassName("results"));

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

        public void Navigate(Uri baseUrl, string searchTerm)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"provider/searchResults?searchTerm={searchTerm}"));
        }

        public void clickSearchResultAtIndex(int resultIndex)
        {
          var result =  _trainingProviderResultsList.ElementAt(resultIndex);

            result.Click();
        }

    }
}
