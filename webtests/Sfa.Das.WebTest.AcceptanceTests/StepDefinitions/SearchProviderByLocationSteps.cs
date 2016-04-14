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


        [Given(@"I have entered '(.*)' with provider radius (.*) miles")]
        public void GivenIHaveEnteredWithProviderRadiusMiles(string p0, int p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have entered '(.*)' which is on boundary of provider radius\.")]
        public void GivenIHaveEnteredWhichIsOnBoundaryOfProviderRadius_(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have entered '(.*)' which is inside radius of provider\.")]
        public void GivenIHaveEnteredWhichIsInsideRadiusOfProvider_(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have entered '(.*)' which is inside radius of two providers\.")]
        public void GivenIHaveEnteredWhichIsInsideRadiusOfTwoProviders_(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have entered '(.*)' which is intersecting between two providers radius")]
        public void GivenIHaveEnteredWhichIsIntersectingBetweenTwoProvidersRadius(string p0)
        {
            ScenarioContext.Current.Pending();
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
        
        [Then(@"I should see total of ""(.*)"" providers found\.")]
        public void ThenIShouldSeeTotalOfProvidersFound_(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see provider ""(.*)"" in provider result page\.")]
        public void ThenIShouldSeeProviderInProviderResultPage_(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
