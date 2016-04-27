namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;
    using System.Configuration;
    using System.Linq;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class PageContext : IPageContext
    {
        private readonly IWebDriver _driver;

        private static readonly int TimeoutInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["webdriver.timeout"] ?? "10");

        public PageContext(IWebDriver driver)
        {
            _driver = driver;
        }

        private object _currentPage;

        private IWebElement _htmlElement;

        public object CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                _htmlElement = _driver.FindElement(By.TagName("html"));
            }
        }

        public By FindSelector(string propertyName)
        {
            if (CurrentPage == null)
            {
                throw new NullReferenceException("Not currently on a page");
            }

            var propertyInfos = CurrentPage.GetType().GetProperties().Where(x => string.Equals(x.Name, propertyName.Replace(" ", string.Empty), StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (!propertyInfos.Any())
            {
                throw new SettingsPropertyNotFoundException($"Couldn't find the property '{propertyName.Replace(" ", string.Empty)}' on the page '{CurrentPage.GetType().Name}'");
            }

            var getter = propertyInfos.Single().GetMethod;
            return getter.Invoke(CurrentPage, null) as By;
        }

        public IWebElement FindElement(string propertyName)
        {
            if (CurrentPage == null)
            {
                throw new NullReferenceException("Not currently on a page");
            }

            var propertyInfos = CurrentPage.GetType().GetProperties().Where(x => string.Equals(x.Name, propertyName.Replace(" ", string.Empty), StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (!propertyInfos.Any())
            {
                throw new SettingsPropertyNotFoundException($"Couldn't find the property '{propertyName.Replace(" ", string.Empty)}' on the page '{CurrentPage.GetType().Name}'");
            }

            var getter = propertyInfos.Single().GetMethod;


            return getter.Invoke(CurrentPage, null) as IWebElement;
        }

        public void WaitForPageLoad()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(TimeoutInSeconds)).Until(OldPageHasGoneStale);
        }

        private bool OldPageHasGoneStale(IWebDriver driver)
        {
            try
            {
                _htmlElement.FindElements(By.TagName("waiting-for-page-to-finish-loading"));
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
        }
    }
}