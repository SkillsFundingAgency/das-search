using System;
using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class ProviderLocationSearchResultPage : FatBasePage
    {
        protected override string PageTitle => "Provider Search Results - Find apprenticeship training";
        public override bool Verify()
        {
          return  PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }


        public ProviderLocationSearchResultPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public IWebElement EmployerSatisfaction => GetByCss("dd.employer-satisfaction");

        public IWebElement LearnerSatisfaction => GetByCss("dd.learner-satisfaction");

        public IWebElement AchievementRate => GetByCss("dd.achievement-rate");

        public void Navigate (Uri baseUrl,string course, string postCode)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Provider/FrameworkResults?PostCode={postCode}&IsLevyPayingEmployer=true&apprenticeshipid={course}"));
            
        }
        internal ApprenticeshipPage ClickOnResult(int resultIndex)
        {
            var result = GetByCss($"#apprenticeship-results a:nth-child({resultIndex})");
            result.Click();
            return new ApprenticeshipPage(Driver);
        }

    }
}