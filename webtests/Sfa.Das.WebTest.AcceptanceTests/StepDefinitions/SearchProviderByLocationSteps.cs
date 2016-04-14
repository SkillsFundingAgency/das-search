using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class SearchProviderByLocationSteps
    {
        SearchPage srchPage;
        ProviderResultPage prvdrPage;

        public SearchProviderByLocationSteps()
        {
            srchPage = new SearchPage();
            prvdrPage = new ProviderResultPage();
        }
        [Given(@"I am on Standard '(.*)' detail page")]
        public void GivenIAmOnStandardDetailPage(string p0)
        {
            srchPage.launchLandingPage();
            srchPage.OpenStandarDetails(p0);
            
        }

        [Given(@"I am on Framework '(.*)' detail page")]
        public void GivenIAmOnFrameworkDetailPage(string p0)
        {
            srchPage.launchLandingPage();
            srchPage.OpenFrameworkDetails(p0);
        }

        [Then(@"I should list of providers on provider search result page\.")]
        public void ThenIShouldListOfProvidersOnProviderSearchResultPage_()
        {
            prvdrPage.verifyProviderResultsPage();
        }
        
        [Then(@"I should see provider ""(.*)"" in provider results page\.")]
        public void ThenIShouldSeeProviderInProviderResultsPage_(string p0)
        {

            prvdrPage.verifyProviderinSearchResults(p0);
        }

        [Then(@"I should see location venue of provider ""(.*)""")]
        public void ThenIShouldSeeLocationVenueOfProvider(string p0)
        {
            prvdrPage.verifyProviderLocationinSearchResults(p0);
        }


        [Then(@"I should not see provider ""(.*)"" in provider results page\.")]
        public void ThenIShouldNotSeeProviderInProviderResultsPage_(string p0)
        {
            prvdrPage.verifyProviderNotinSearchResults(p0);
        }
        
        [Then(@"I should see provider ""(.*)"" listed in top as nearest training provider\.")]
        public void ThenIShouldSeeProviderListedInTopAsNearestTrainingProvider_(string p0)
        {
            prvdrPage.verifyProviderinSearchResults(p0);
        }
    }
}
