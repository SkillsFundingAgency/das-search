
namespace Sfa.Das.Sas.WebTest.AcceptanceTests.Steps
{
    using System;
    using TechTalk.SpecFlow;

    [Binding]
    public class StepBindings :Steps
    {
        [When(@"I want to find Provider for (.*) framework near (.*)")]
        public void WhenIWantToFindProviderForFrameworkNear(string provider, string postcode)
        {
            string[] header = { "Field", "Value" };
            string[] providerrow = { "Search Box", provider };
            string[] postcoderow = { "Postcode Search Box", postcode };
            Given("I navigated to the Start page");
            When("I choose Start Button");
            var param = "Search page";
            Then(string.Format("I am on the {0}", param));
            var table = new Table(header);
            table.AddRow(providerrow);
            When("I enter data", table);
            And("I choose Search Button");
            Then("I am on the Search Results page");
            When("I choose First Framework Result");
            Then("I am on the Framework Details page");
            When("I choose Search Page Button");
            Then("I am on the Framework Provider Search page");
            var postcodetable = new Table(header);
            postcodetable.AddRow(postcoderow);
            When("I enter data", postcodetable);
            
        }

        [When(@"I am (.*)")]
        public void WhenIAmYesImLevyPayingEmployer(string levyOrnonLevy)
        {
            When(string.Format("I choose {0}", levyOrnonLevy));
            When("I choose Search Button");
            Then("I am on the Framework Provider Results page");
        }

        [Then(@"I can see Provider details")]
        public void ThenICanSeeProviderDetails()
        {
            string[] header = { "Field", "Rule", "Value" };
            string[] result = { "Provider Name", "Exists", "true" };
            Then("I see Provider Results list contains at least 1 items");
            When("I choose First Provider Link");
            Then("I am on the Provider Details page");
            var table = new Table(header);
            table.AddRow(result);
            And("I see", table);
        }
    }
}
