namespace Sfa.Das.WebTest.AcceptanceTests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading;

    using NUnit.Framework;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using Sfa.Das.WebTest.Infrastructure.Selenium;

    using TechTalk.SpecFlow;

    public abstract class BasePage
    {
        /// <summary>
        /// Puprose of this Base class is to
        /// Create and maintain generic functions which will be called across other pages.
        /// 
        /// </summary>
        public IWebDriver driver;

        public string baseUrl;

        protected BasePage()
        {
            driver = (IWebDriver)FeatureContext.Current["driver"];
            baseUrl = ConfigurationManager.AppSettings["service.url"];
        }

        public void type(string inputText, By locator)
        {
            Find(locator).SendKeys(inputText);
        }

        public IWebElement Find(By locator)
        {
            //ValidateSelector(locator); will update css selectors, however not to cause any extra delays by checking this programmatically.

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));

            return driver.FindElement(locator);
        }

        public IList<IWebElement> FindElements(By locator)
        {
            ValidateSelector(locator);
            return driver.FindElements(locator);
        }

        public void VerifyTitle(string pageTitle)
        {
            if (!pageTitle.Equals(driver.Title))
            {
                throw new InvalidOperationException("This page is not " + pageTitle + ". The title is: " + driver.Title);
            }
        }

        public void Navigate(string url)
        {
            var fullUrl = baseUrl + url;
            Console.WriteLine($"-> Navigating to {fullUrl}");
            driver.Navigate().GoToUrl(fullUrl);
        }

        public void Launch(string pageTitle)
        {
            driver.Navigate().GoToUrl(baseUrl);
            VerifyTitle(pageTitle);
        }

        public void click(By locator)
        {
            Find(locator).Click();
        }

        public string GetText(By locator)
        {
            return Find(locator).Text;
        }

        public void AssertIsElementPresent(By locator, string match)
        {
            Assert.True(isElementPresent(locator, match), $"Couldn't find the text '{match}' with the selector '{locator}'\n{driver.Url}");
        }

        public void AssertContainsText(By locator, string match)
        {
            var text = GetText(locator);
            Assert.True(text.Contains(match), $"Expected to contain '{match}' but was '{text}'\n{driver.Url}");
        }

        public bool isElementPresent(By locator, string match)
        {
            var subelements = FindElements(locator);
            for (var i = 0; i < subelements.Count; i++)
            {
                if (subelements[i].Text == match)
                {
                    Console.WriteLine("Found " + subelements[i].Text);
                    return true;
                }
            }
            return false;
        }

        public void searchNSelect(By locator, string match)
        {
            var subelements = FindElements(locator);
            for (var i = 0; i < subelements.Count; i++)
            {
                if (subelements[i].Text == match)
                {
                    subelements[i].Click();
                    break;
                }
            }
        }

        public void WaitFor(By locator)
        {
            ValidateSelector(locator);
            driver.WaitFor(locator);
        }

        public void AssertIsElementNotPresent(By locator, string provider)
        {
            Assert.IsTrue(isElementNotPresent(locator, provider), $"found text matching '{provider}' in any of the subelements of {locator}\n{driver.Url}");
        }

        private bool isElementNotPresent(By locator, string provider)
        {
            var subelements = FindElements(locator);
            for (var i = 0; i < subelements.Count; i++)
            {
                while (subelements[i].Text != provider)
                {
                    Console.WriteLine("Provider not Found " + provider);
                    return true;
                }
            }
            return false;
        }

        private static void ValidateSelector(By locator)
        {
            var value = locator.ToString();
            if (value.Contains("XPath"))
            {
                Console.WriteLine("****** TODO remove " + value);
            }
        }

        public void AssertIsDisplayed(By locator)
        {
            Assert.IsTrue(isDisplayed(locator), $"Couldn't find the element {locator}");
        }

        public bool isDisplayed(By locator)
        {
            try
            {
                var element = Find(locator);
                return ElementIsDisplayed(element);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void Sleep(int milliseconds) /// conditional wait is now added at WebElement level
        {
            Console.WriteLine("****** TODO wait for an element instead -> Slept for " + milliseconds + "ms");
            Thread.Sleep(milliseconds);
        }

        public static bool ElementIsDisplayed(IWebElement element)
        {
            return element.Displayed && element.Enabled;
        }
    }
}