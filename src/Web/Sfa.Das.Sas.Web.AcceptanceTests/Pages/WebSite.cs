using Sfa.Automation.Framework.Selenium;
using System;
using OpenQA.Selenium;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    public class WebSite : TestBase
    {
        public Uri BaseUrl { get; }
        internal PageFactory PageFactory;

        public WebSite(string webSiteUrl, Automation.Framework.Enums.WebDriver webDriver)
        {
            BaseUrl = new Uri(webSiteUrl);
            CommonTestSetup(BaseUrl, true, webDriver);
            PageFactory = new PageFactory(WebBrowserDriver);
        }

        internal StartPage NavigateToStartPage()
        {
            WebBrowserDriver.Navigate().GoToUrl(BaseUrl.Combine(""));
            return new StartPage(WebBrowserDriver);
        }

        internal ApprenticeshipSearchResultPage NavigateToSearchResultPage()
        {
            WebBrowserDriver.Navigate().GoToUrl(BaseUrl.Combine("Apprenticeship/SearchResults?Keywords="));
            return new ApprenticeshipSearchResultPage(WebBrowserDriver);
        }

        internal ProviderLocationSearchResultPage NavigateToSearchProviderLocationResultPage(string course, string postCode)
        {
            WebBrowserDriver.Navigate().GoToUrl(BaseUrl.Combine($"Provider/FrameworkResults?PostCode={postCode}&IsLevyPayingEmployer=true&apprenticeshipid={course}"));
            return new ProviderLocationSearchResultPage(WebBrowserDriver);
        }

        internal void ClickBackLink()
        {
          var backlink =  WebBrowserDriver.FindElement(By.CssSelector("a.link-back"));

            backlink.Click();

        }

        internal void Close()
        {
            WebBrowserDriver.Quit();
        }
    }
}
