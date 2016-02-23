using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using System.Diagnostics.Eventing;
using Sfa.Eds.Das.Web.AcceptanceTests.utils;
using System.Collections.Generic;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    class BasePage // :  Base
    {
        /// <summary>
        /// Puprose of this Base class is to
        /// Create and maintain generic functions which will be called across other pages.
        /// 
        /// </summary>
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

            // WebDriverWait wait = new (WebDriverWait(driver, TimeSpan.FromSeconds(15)));
            // wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return driver.FindElement(locator);
        }

        public void verifyPage(string pageTitle)
        {
            if (!pageTitle.Equals(driver.Title))
            {
                throw new InvalidOperationException("This page is not " + pageTitle + ". The title is: " + driver.Title);
            }
        }


        public void Launch(string pageTitle)
        {
            driver.Navigate().GoToUrl(baseUrl);
            verifyPage(pageTitle);
        }


        public void Open(String standard)
        {
            driver.Navigate().GoToUrl(baseUrl + "Standard/Detail/" + standard);

        }
        public void click(By locator)
        {
            find(locator).Click();
        }


        public string getText(By locator)
        {
            return find(locator).Text;

        }

        public bool isElementPresent(By locator, string provider)
        {
            
            IList<IWebElement> subelements = driver.FindElements(locator);
            for (int i = 0; i < subelements.Count; i++)
            {
                Console.Write(subelements[1].Text);

                if (subelements[1].Text == provider)
                {
                    Console.Write(subelements[1].Text);
                    return true;

                }
               
                else
                    return false;

            }
            return false;
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
