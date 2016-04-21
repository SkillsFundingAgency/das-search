namespace Sfa.Das.WebTest.Infrastructure.Steps
{
    using System;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    using Sfa.Das.WebTest.Infrastructure.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class ClickingSteps
    {
        private readonly IPageContext _pageContext;

        private readonly IWebDriver _driver;

        public ClickingSteps(IPageContext pageContext, IWebDriver driver)
        {
            _pageContext = pageContext;
            _driver = driver;
        }

        [Given("I choose (.*)")]
        [When("I choose (.*)")]
        public void Ichoose(string propertyName)
        {
            FindElement(propertyName).Click();
        }

        private IWebElement FindElement(string propertyName)
        {
            IWebElement element;
            var selector = _pageContext.FindSelector(propertyName);
            if (selector != null)
            {
                element = _driver.FindElement(selector);
            }
            else
            {
                element = _pageContext.FindElement(propertyName);
            }

            if (element == null)
            {
                throw new MissingMemberException("Couldn't find an element on the page for " + propertyName);
            }
            return element;
        }
    }
}