using OpenQA.Selenium;
using Specflow_Selenium_PO_Example2.Step_Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Specflow_Selenium_PO_Example2.Utils;
using OpenQA.Selenium.Support.UI;
using System.Configuration;

namespace Specflow_Selenium_PO_Example2.Pages
{
    [Binding]
    class BasePage // :  Base
    {
        readonly IWebDriver driver;
        private static string baseUrl;
         public BasePage() {
            driver = (IWebDriver)ScenarioContext.Current["driver"];
            baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        }

        /// <summary>
        /// Enters text into an element
        /// </summary>
        /// <param name="inputText">The text to input</param>
        /// <param name="locator">The element to enter text into</param>
        public void type (string inputText, By locator) {
            find(locator).SendKeys(inputText);
        }
        /// <summary>
        /// Waits up to 15 seconds for an element to be visible, then returns it
        /// </summary>
        /// <param name="locator">The element to find</param>
        /// <returns>WebDriver IWebElement</returns>
        public IWebElement find(By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return driver.FindElement(locator);
        }

        public void verifyPage(string pageTitle)
        {
            if (!pageTitle.Equals(driver.Title))
            {
                throw new InvalidOperationException("This page is not " + pageTitle + ". The title is: " + driver.Title);
            }
        }

        /// <summary>
        /// Opens the BaseUrl appended with the page url and verifies the 
        /// page title is as expected.
        /// </summary>
        /// <param name="url"></param>
        public void visit(string url, string pageTitle)
        {
            driver.Navigate().GoToUrl(baseUrl + url);
            verifyPage(pageTitle);   
        }

        /// <summary>
        /// Left clicks an element
        /// </summary>
        /// <param name="locator">The element to click</param>
        public void click(By locator)
        {
            find(locator).Click();
        }

        /// <summary>
        /// gets the inner HTML test of an element
        /// </summary>
        /// <param name="locator">The element which contains the text</param>
        /// <returns></returns>
        public string getText(By locator)
        {
            return find(locator).Text;
        }

        /// <summary>
        /// Checks whether an element is displayed and enabled
        /// </summary>
        /// <param name="locator">The element to be checked</param>
        /// <returns>True if the element is visible and enabled</returns>
        public bool isDisplayed(By locator)
        {
            try
            {
                IWebElement element = find(locator);
                return element.Displayed && element.Enabled;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }

        
        /// <summary>
        /// Submits a form
        /// </summary>
        /// <param name="locator"></param>
        public void submit(By locator)
        {
            find(locator).Submit();
        }

        /// <summary>
        /// Returns the destination URL of hyperlinks
        /// </summary>
        /// <param name="locator"></param>
        /// <returns>the contents of the href attribute (for hyperlinks and images)</returns>
        public string getLinkDestination(By locator)
        {
            return find(locator).GetAttribute("href");
        }

        /// <summary>
        /// Checks a checkbox or radio button. Performs no action if the element is already checked or selected
        /// </summary>
        /// <param name="locator">The element to be checked</param>
        /// <returns></returns>
        public void check(By locator)
        {
            IWebElement element = find(locator);
            if (!element.Selected) 
            {
                element.Click();
            }
        }

        /// <summary>
        /// Unchecks a checkbox or radio button. Performs no action if the element is already unchecked or selected
        /// </summary>
        /// <param name="locator">The element to be unchecked</param>
        /// <returns></returns>
        public void uncheck(By locator)
        {
            IWebElement element = find(locator);
            if (element.Selected)
            {
                element.Click();
            }
        }

        /// <summary>
        /// Verifies if a checkbox is checked
        /// </summary>
        /// <param name="locator">The element to verify</param>
        /// <returns>True if checkbox is selected otherwise returns False</returns>
        public bool isSelected(By locator)
        {
            return find(locator).Selected;
        }
    }
}
