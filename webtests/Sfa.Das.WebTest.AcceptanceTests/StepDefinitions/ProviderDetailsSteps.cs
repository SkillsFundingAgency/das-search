using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ProviderDetailsSteps
    {
        SearchPage srchPage;
        ProviderResultPage prvdrPage;
        ProviderDetailPage pdetailPage;


        public ProviderDetailsSteps()
        {
            srchPage = new SearchPage();
            prvdrPage = new ProviderResultPage();
            pdetailPage = new ProviderDetailPage();

        }


        [Given(@"I have a provider with more than on location")]
        public void GivenIHaveAProviderWithMoreThanOnLocation()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have provider with full training at employer location")]
        public void GivenIHaveProviderWithFullTrainingAtEmployerLocation()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have provider '(.*)' with all of traning modes")]
        public void GivenIHaveProviderWithAllOfTraningModes(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have a provider with updated info in course directory")]
        public void GivenIHaveAProviderWithUpdatedInfoInCourseDirectory()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I am on provider detail page")]
        public void GivenIAmOnProviderDetailPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I chose a provider from result page")]
        public void WhenIChoseAProviderFromResultPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I choose this provider from result page")]
        public void WhenIChooseThisProviderFromResultPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I select any of the provider from the list")]
        public void WhenISelectAnyOfTheProviderFromTheList()
        {
            prvdrPage.chooseProvider();
        }
        
        [When(@"I open provider detail page")]
        public void WhenIOpenProviderDetailPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I click on back link")]
        public void WhenIClickOnBackLink()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see only one location on provider detail page")]
        public void ThenIShouldSeeOnlyOneLocationOnProviderDetailPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see provider detail page with no veunue details")]
        public void ThenIShouldSeeProviderDetailPageWithNoVeunueDetails()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string p0)
        {
            pdetailPage.verifyProviderDetailPage(p0);
        }
        
       
        
        [Then(@"under training modes I should see ""(.*)""")]
        public void ThenUnderTrainingModesIShouldSee(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
       
        
        [Then(@"I should see location name\.")]
        public void ThenIShouldSeeLocationName_()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see updated provider info in provider detail page\.")]
        public void ThenIShouldSeeUpdatedProviderInfoInProviderDetailPage_()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I shoudl be able to return back to provider list page\.")]
        public void ThenIShoudlBeAbleToReturnBackToProviderListPage_()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
