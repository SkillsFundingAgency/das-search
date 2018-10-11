using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages.Apprenticeships
{
    class FindATrainingProviderPage :FatBasePage
    {
        public FindATrainingProviderPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageTitle => "Search for providers - Find apprenticeship training";
        private IWebElement PostcodeTextBox => GetById("search-box");
        private IWebElement LevyPayingTrueRadioButton => GetById("levyPaying");
        private IWebElement LevyPayingFalseRadioButton => GetById("notLevyPaying");
        private IWebElement SearchButton => GetByCss("postcode-search-button");
        public override bool Verify()
        {
           return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

        public void Navigate(Uri baseUrl, string frameworkId, string keywords)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Apprenticeship/SearchForFrameworkProviders?frameworkId={frameworkId}&keywords={keywords}"));
        }
        public void EnterPostcode(string postcode)
        {
            FormCompletionHelper.EnterText(PostcodeTextBox,postcode);
        }

        public void SelectLevyPayer(bool value)
        {
            if (value)
            {
                FormCompletionHelper.ClickElement(LevyPayingTrueRadioButton);
            }
            else
            {
                FormCompletionHelper.ClickElement(LevyPayingFalseRadioButton);
            }
            
        }

        public void ClickSearchButton()
        {
            FormCompletionHelper.ClickElement(SearchButton);
        }

        public void CompleteSearch(string postcode, bool levyPayer)
        {
            EnterPostcode(postcode);
            SelectLevyPayer(levyPayer);
            ClickSearchButton();
        }
    }
}
