namespace Sfa.Das.WebTest.AcceptanceTests.Pages
{
    using System;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.AcceptanceTests;

    class FrameworkDetailPage : BasePage
    {
        By frameworkDetailTitle = By.CssSelector(".heading-xlarge");

        By frameworkDetailLevel = By.XPath(".//*[@id='content']/section/dl/dd[2]");

        By frameworkDetailPathway = By.CssSelector(".column-two-thirds>p");

        By frameworkDetailContent = By.XPath(".//*[@id='content']/section/dl/dd[3]");

        By standardDetailEntryReq = By.XPath(".//*[@id='content']/section/dl/dd[4]");

        public void verifyFrameworkTitle(String title)
        {
            AssertContainsText(frameworkDetailTitle, title);
        }

        public void verifyFrameworkPathway(String pathway)
        {
            AssertContainsText(frameworkDetailPathway, pathway);
        }
    }
}