namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    using Sfa.Eds.Das.Web.AcceptanceTests.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class StandardSearchResultSteps
    {
        private SearchResultsPage searchResultsPage;

        public StandardSearchResultSteps()
        {
            searchResultsPage = new SearchResultsPage();
        }

        [Then(@"I'm on the Search results page")]
        public void ThenIAmOnTheSearchResultsPage()
        {
            searchResultsPage.WaitForLoad();
        }

        [When(@"I pick any of the standard from search result page")]
        public void WhenIChooseAnyOfTheStandardFromSearchResultPage()
        {
            searchResultsPage.chooseStandard();
        }
    }
}