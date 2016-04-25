namespace Sfa.Das.WebTest.Infrastructure.Steps
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class EnteringDataSteps
    {
        private readonly IPageContext _pageContext;

        private readonly IWebDriver _driver;

        public EnteringDataSteps(IPageContext pageContext, IWebDriver driver)
        {
            _pageContext = pageContext;
            _driver = driver;
        }

        [Given("I entered data")]
        [When("I enter data")]
        [Then("I enter data")]
        public void EnterData(Table dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var selector = _pageContext.FindSelector(row[0]);
                var input = row[1];
                foreach (var match in Regex.Matches(input, "(?<={).*(?=})"))
                {
                    input = input.Replace("{" + match + "}", ScenarioContext.Current[match.ToString()].ToString());
                }

                Console.WriteLine("-> Entering \"" +  input + "\" into " + row[0]);
                _driver.FindElement(selector).SendKeys(input);
            }
        }
    }
}