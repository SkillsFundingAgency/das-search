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
            baseUrl = ConfigurationManager.AppSettings["service.url"];

        }


        public void type(string inputText, By locator)
        {
            find(locator).SendKeys(inputText);
        }

        public IWebElement find(By locator)
        {
            validateSelector(locator);

            //   WebDriverWait wait = new (WebDriverWait(driver, TimeSpan.FromSeconds(15)));
            //wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return driver.FindElement(locator);
        }

        public IList<IWebElement> FindElements(By locator)
        {
            validateSelector(locator);
            return driver.FindElements(locator);
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
            IList<IWebElement> subelements = FindElements(locator);
            for (int i = 0; i < subelements.Count; i++)
            {
                
               // Console.Write(subelements[i].Text);

                if (subelements[i].Text == provider)
                {
                    Console.Write("Provider Found " + subelements[i].Text);
                    return true;

                }
               
                

            }
            return false;
        }

        public bool verifyTextMessage(By locator,String text)
        {
            if (find(locator).Text.Contains(text))
                return true;
            else 
                return false;
        }



        public bool isElementNotPresent(By locator, string provider)
        {
            IList<IWebElement> subelements = FindElements(locator);
            for (int i = 0; i < subelements.Count; i++)
            {

                //Console.Write(subelements[i].Text);

                while(subelements[i].Text != provider)
                {
                    Console.Write("Provider not Found " + provider);
                    return true;

                }



            }
            return false;
        }

        private void validateSelector(By locator)
        {
            var value = locator.ToString();
            if (value.Contains("XPath"))
            {
                Console.WriteLine("****** TODO remove " + value);
            }
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
