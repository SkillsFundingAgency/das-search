namespace Sfa.Das.WebTest.Infrastructure.Steps
{
    using OpenQA.Selenium;

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

            element.Click();
        }
    }
}