using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class StartPage : FatBasePage
    {
        protected override string PageTitle => "Home Page - Find apprenticeship training";
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(Heading.Text, PageTitle);
        }

        private readonly IWebDriver _webBrowserDriver;

        //[FindsBy(How = How.Id, Using = "start-button")]
        //public IWebElement StartButton2 { get; set; }

        public IWebElement StartButton => GetById("start-button");

        public StartPage(IWebDriver webBrowserDriver) : base(webBrowserDriver)
        {
            _webBrowserDriver = webBrowserDriver;
        }

        internal SearchApprenticeshipPage ClickStartButton()
        {
            StartButton.Click();
            return new SearchApprenticeshipPage(_webBrowserDriver);
        }
    }
}