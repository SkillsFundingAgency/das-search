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

        public SearchProviderSteps()
        {
            srchPage = new SearchPage();
            prvdrPage = new ProviderResultPage();
        }

        [Then(@"I am on the provider results page")]
        public void ThenIAmOnTheProviderResultsPage()
        {
            prvdrPage.WaitToLoad();
        }

        [Given(@"I enter ""(.*)""  in provider search box")]
        public void GivenIEnterInProviderSearchBox(string p0)
        {
            prvdrPage.enterlocation(p0);
        }


        [When(@"I choose any of the standard from search result page")]
        public void WhenIChooseAnyOfTheStandardFromSearchResultPage()
        {
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
            prvdrPage.verifyProviderResultsPage();
        }

        [Then(@"I should see all providers in result page")]
        public void ThenIShouldSeeAllProvidersInResultPage()
        {
            prvdrPage.verifyProviderResultsPage();
        }

        [Then(@"under each provider I should see provider ""(.*)""")]
        public void ThenUnderEachProviderIShouldSeeProvider(string p0)
        {
            prvdrPage.verifyProvidersearchResultsInfo(p0);
        }

        [Then(@"I should see provider ""(.*)""")]
        public void ThenIShouldSeeProvider(string p0)
        {
            prvdrPage.verifyProvidersearchResultsInfo(p0);
        }
    }
}
