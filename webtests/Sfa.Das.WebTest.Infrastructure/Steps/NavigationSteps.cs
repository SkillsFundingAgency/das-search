namespace Sfa.Das.WebTest.Infrastructure.Steps
{
    using System;
    using System.Linq;

    using BoDi;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure.Selenium;
    using Sfa.Das.WebTest.Infrastructure.Settings;

    using TechTalk.SpecFlow;

    [Binding]
    public class NavigationSteps
    {
        private readonly IWebDriver _driver;

        private readonly IBrowserSettings _browserSettings;

        private readonly IPageContext _pageContext;

        private readonly IObjectContainer _objectContainer;

        public NavigationSteps(IWebDriver driver, IBrowserSettings browserSettings, IPageContext pageContext, IObjectContainer objectContainer)
        {
            _driver = driver;
            _browserSettings = browserSettings;
            _pageContext = pageContext;
            _objectContainer = objectContainer;
        }

        [Given(@"I navigated to the (.*) page")]
        [When(@"I navigate to the (.*) page")]
        [Then(@"I navigate to the (.*) page")]
        public void NavigateToPage(string page)
        {
            var objectType = FindPageType(page);
            var attribute = (PageNavigationAttribute)Attribute.GetCustomAttribute(objectType, typeof(PageNavigationAttribute));
            var url = _browserSettings.BaseUrl + attribute.Url;
            Console.WriteLine("-> Navigating to " + url);
            _driver.Navigate().GoToUrl(url);
            _pageContext.CurrentPage = _objectContainer.Resolve(objectType);
        }

        [Given(@"I navigated to the (.*) page with parameters")]
        [When(@"I navigate to the (.*) page with parameters")]
        [Then(@"I navigate to the (.*) page with parameters")]
        public void NavigateToPageWithParameters(string page, Table parameters)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I was on the (.*) page")]
        [When(@"I am on the (.*) page")]
        [Then(@"I am on the (.*) page")]
        public void IamOnThePage(string page)
        {
            var objectType = FindPageType(page);
            var attribute = (PageNavigationAttribute)Attribute.GetCustomAttribute(objectType, typeof(PageNavigationAttribute));
            var url = _browserSettings.BaseUrl + attribute.Url.ToLower();
            _pageContext.WaitForPageLoad();
            var cleanUrl = _driver.CleanUrl();
            Assert.True(cleanUrl.StartsWith(url), $"Expected to start with {url} but was {cleanUrl}");
            _pageContext.CurrentPage = _objectContainer.Resolve(objectType);
        }

        private static Type FindPageType(string page)
        {
            var types =
                (from asm in AppDomain.CurrentDomain.GetAssemblies() from type in asm.GetTypes() where type.IsClass && type.Name == page.Replace(" ", string.Empty) + "Page" select type).ToList();
            if (!types.Any())
            {
                throw new ApplicationException($"couldn't find a class for the page '{page.Replace(" ", string.Empty)}Page.cs'");
            }

            return types.Single();
        }
    }
}