using OpenQA.Selenium;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class ProviderLocationSearchResultPage : FatBasePage
    {
        protected override string PageTitle => "Provider Search Results - Find apprenticeship training";


        public ProviderLocationSearchResultPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public IWebElement EmployerSatisfaction => GetByCss("dd.employer-satisfaction");

        public IWebElement LearnerSatisfaction => GetByCss("dd.learner-satisfaction");

        public IWebElement AchievementRate => GetByCss("dd.achievement-rate");

        internal ApprenticeshipPage ClickOnResult(int resultIndex)
        {
            var result = GetByCss($"#apprenticeship-results a:nth-child({resultIndex})");
            result.Click();
            return new ApprenticeshipPage(Driver);
        }

    }
}