using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;
using System;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages.Apprenticeships
{
    internal class ProviderSearchPage : FatBasePage
    {
        public ProviderSearchPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageTitle => "Home Page - Find apprenticeship training";
        private IWebElement _trainingProviderTextbox => GetById("searchTerm");
        private IWebElement _searchButton => GetById("submit-keywords");

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

        public void Navigate(Uri baseUrl)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"provider/search"));
        }

        public void EnterTraningProviderName(string keyword)
        {
            FormCompletionHelper.EnterText(_trainingProviderTextbox, keyword);
        }

        public void ClickSearchButton()
        {
            FormCompletionHelper.ClickElement(_searchButton);
        }

        public void EnterTrainingProviderNameAndSearch(string keywords)
        {
            EnterTraningProviderName(keywords);
            ClickSearchButton();
        }

    }
}
