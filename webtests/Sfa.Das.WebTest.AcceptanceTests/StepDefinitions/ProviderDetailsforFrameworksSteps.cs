using System;
using TechTalk.SpecFlow;

namespace Sfa.Das.WebTest.AcceptanceTests.StepDefinitions
{
    using Sfa.Das.WebTest.AcceptanceTests.Pages;

    [Binding]
    public class ProviderDetailsforFrameworksSteps
    {
        ProviderDetailPage pdetailPage;

        public ProviderDetailsforFrameworksSteps()
        {
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
        
        [Then(@"I should see provider name ""(.*)""")]
        [Then(@"I should see websitecontactpage ""(.*)""")]
        [Then(@"I should see framework and pathway ""(.*)""")]
        [Then(@"I should see phone ""(.*)""")]
        [Then(@"I should see email ""(.*)""")]
        [Then(@"I should see trainingStructure ""(.*)""")]
        [Then(@"I should see training location ""(.*)""")]
        public void ThenIShouldSeeProviderName(string p0)
        {
            pdetailPage.verifyProviderDetailsPageInfo(p0);
        }
        
      
       
    }
}
