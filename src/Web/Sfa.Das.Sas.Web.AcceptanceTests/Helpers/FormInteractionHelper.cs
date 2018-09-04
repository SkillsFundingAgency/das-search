using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Helpers
{
    public class FormCompletionHelper : PageInteractionHelper
    {
        public static void ClickElement(IWebElement element)
        {
            element.Click();
        }

        public static void ClickElement(By locator)
        {
            WebDriver.FindElement(locator).Click();
        }

        public static void EnterText(IWebElement element, String text)
        {
            element.Clear();
            element.SendKeys(text);
        }

        public static void EnterText(By locator, String text)
        {
            WebDriver.FindElement(locator).Clear();
            WebDriver.FindElement(locator).SendKeys(text);
        }

        public static void EnterText(IWebElement element, int value)
        {
            element.Clear();
            element.SendKeys(value.ToString());
        }

        public static void SelectFromDropDownByValue(IWebElement element, String value)
        {
            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);
        }

        public static void SelectFromDropDownByText(IWebElement element, String text)
        {
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(text);
        }

        public static void SelectCheckBox(IWebElement element)
        {
            if (element.Displayed && !element.Selected)
            {
                element.Click();
            }
        }

        public static void SelectRadioOptionByForAttribute(By locator, String forAttribute)
        {
            IList<IWebElement> radios = WebDriver.FindElements(locator);
            var radioToSelect = radios.FirstOrDefault(radio => radio.GetAttribute("for") == forAttribute);

            if (radioToSelect != null)
            {
                ClickElement(radioToSelect);
            }
        }

        public static void SelectRadioOptionById(String id)
        {
            var radioOption = WebDriver.FindElement(By.Id(id));

            if (radioOption != null)
            {
                ClickElement(radioOption);
            }
        }
    }
}
