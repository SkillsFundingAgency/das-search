using Sfa.Automation.Framework.Selenium;
using System;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    public class WebSite : TestBase
    {
        public Uri BaseUrl { get; }

        public WebSite(string webSiteUrl, Automation.Framework.Enums.WebDriver webDriver)
        {
            BaseUrl = new Uri(webSiteUrl);
            CommonTestSetup(BaseUrl, true, webDriver);
        }

        internal StartPage NavigateToStartPage()
        {
            WebBrowserDriver.Navigate().GoToUrl(BaseUrl.Combine(""));
            return new StartPage(WebBrowserDriver);
        }

        internal SearchResultPage NavigateToSearchResultPage()
        {
            WebBrowserDriver.Navigate().GoToUrl(BaseUrl.Combine("Apprenticeship/SearchResults?Keywords="));
            return new SearchResultPage(WebBrowserDriver);
        }

        internal ProviderLocationSearchResultPage NavigateToSearchProviderLocationResultPage(string course, string postCode)
        {
            WebBrowserDriver.Navigate().GoToUrl(BaseUrl.Combine($"Provider/FrameworkResults?PostCode={postCode}&IsLevyPayingEmployer=true&apprenticeshipid={course}"));
            return new ProviderLocationSearchResultPage(WebBrowserDriver);
        }

        internal void Close()
        {
            WebBrowserDriver.Quit();
        }
    }
}
