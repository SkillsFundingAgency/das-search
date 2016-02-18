using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    /// <summary>
    /// Purpose of this Step definition class is to 
    /// Create and maintain all step definitions related provider search functionality.
    /// Any change to business features under provider search can be modified here.
    ///  
    /// </summary>

    [Binding]
    public class SearchProviderSteps
    {

        SearchPage srchPage;
        ProviderResultPage prvdrPage;

        [When(@"I choose any of the standard from search result page")]
        public void WhenIChooseAnyOfTheStandardFromSearchResultPage()
        {
            srchPage = new SearchPage();
            srchPage.chooseStandard();

        }
        
        [When(@"I click on search under provider search section")]
        public void WhenIClickOnSearchUnderProviderSearchSection()
        {
            srchPage.clickProviderSearch(); 
        }
        
        [Then(@"I should all providers in result page")]
        public void ThenIShouldAllProvidersInResultPage()
        {
            prvdrPage = new ProviderResultPage();
            prvdrPage.verifyProviderResultsPage();
        }
        
        [Then(@"I should see all providers listed in Alphabetical order")]
        public void ThenIShouldSeeAllProvidersListedInAlphabeticalOrder()
        {
           // ScenarioContext.Current.Pending();
        }
    }
}
