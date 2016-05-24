namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

        public IPageContext WaitForPageLoad()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(TimeoutInSeconds))
                .Until(ExpectedConditions.StalenessOf(_htmlElement));

            return this;
        }

        public IPageContext CheckForJavascriptErrors()
        {
            var errors = ((IJavaScriptExecutor)_driver).ExecuteScript("return window.jsErrors") as ReadOnlyCollection<object> ?? new ReadOnlyCollection<object>(new List<object>());
            if (errors.Any())
            {
                throw new ApplicationException(string.Join("\n", errors.Select(x => x.ToString())));
            }

            return this;
        }
    }
}