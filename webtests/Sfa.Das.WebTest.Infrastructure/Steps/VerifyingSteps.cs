namespace Sfa.Das.WebTest.Infrastructure.Steps
{
    using System.Linq;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class VerifyingSteps
    {
        private readonly IPageContext _pageContext;

        private readonly IWebDriver _driver;

        public VerifyingSteps(IPageContext pageContext, IWebDriver driver)
        {
            _pageContext = pageContext;
            _driver = driver;
        }

        [Given(@"I saw")]
        [Then(@"I see")]
        public void ThenISee(Table table)
        {
            foreach (var row in table.Rows)
            {
                var selector = _pageContext.FindSelector(row[0]);
                var element = _driver.FindElements(selector).FirstOrDefault();
                switch (row[1].ToLower())
                {
                    case "exists":
                        Assert.IsNotNull(element, "Couldn't find element " + row[0]);
                        break;
                    case "has text":
                        Assert.IsNotNull(element, "Couldn't find element " + row[0]);
                        Assert.AreEqual(element.Text, row[2], $"{row[0]} element text is {element.Text} and not {row[2]}");
                        break;
                }
            }
        }
    }
}