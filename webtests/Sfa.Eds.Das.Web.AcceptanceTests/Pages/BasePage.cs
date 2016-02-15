using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using System.Diagnostics.Eventing;
using Sfa.Eds.Das.Web.AcceptanceTests.utils;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    class BasePage // :  Base
    {
        public IWebDriver driver;
        private static string baseUrl;

        //public object ConfigurationManager { get; private set; }

        public BasePage()
        {
         
            driver = (IWebDriver)FeatureContext.Current["driver"];
            baseUrl = ConfigurationManager.AppSettings["baseUrl"];
                     
        }

        
        public void type(string inputText, By locator)
        {
            find(locator).SendKeys(inputText);
        }

        public IWebElement find(By locator)
        {
            // WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            //  wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return driver.FindElement(locator);
        }

        public void verifyPage(string pageTitle)
        {
            if (!pageTitle.Equals(driver.Title))
            {
                throw new InvalidOperationException("This page is not " + pageTitle + ". The title is: " + driver.Title);
            }
        }


        public void Launch(string url, string pageTitle)
        {
         driver.Navigate().GoToUrl(baseUrl);
            verifyPage(pageTitle);
        }
       
        public void click(By locator)
        {
            find(locator).Click();
        }

       
        public string getText(By locator)
        {
            return find(locator).Text;
        }

        public bool isDisplayed(By locator)
        {
            try
            {
                IWebElement element = find(locator);
                return element.Displayed && element.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }

        }


    }
}
