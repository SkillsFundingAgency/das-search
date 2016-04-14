namespace Sfa.Das.WebTest.Infrastructure.Steps
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class ValidatingSteps
    {
        private readonly IPageContext _pageContext;

        private readonly IWebDriver _driver;

        public ValidatingSteps(IPageContext pageContext, IWebDriver driver)
        {
            _pageContext = pageContext;
            _driver = driver;
        }
        [Then(@"I see (.*) list contains (.*) (.*) items")]
        public void ThenISeeListContainsItems(string element, string rule, int count)
        {
            var selector = _pageContext.FindSelector(element);
            if (rule.ToLower() == "at least")
            {
                Assert.GreaterOrEqual(_driver.FindElements(selector).Count(), count);
            }
            else
            {
                throw new ApplicationException("unknown rule " + rule);
            }
        }

    }
}