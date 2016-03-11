using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ViewStandardDetailsSteps
    {
        SearchPage srchPage;
        StandardDetailsPage stndDetailsPge;

        public ViewStandardDetailsSteps()
        {
            srchPage = new SearchPage();
            stndDetailsPge = new StandardDetailsPage();
        }
        [Given(@"I am on standard search result page")]
        public void GivenIAmOnStandardSearchResultPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have choosen a standard which has no progressional registration data populated")]
        public void GivenIHaveChoosenAStandardWhichHasNoProgressionalRegistrationDataPopulated()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I click on any standard link")]
        public void WhenIClickOnAnyStandardLink()
        {
            ScenarioContext.Current.Pending();
        }
        
       
        
        [When(@"I click on the standard title")]
        public void WhenIClickOnTheStandardTitle()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should be able to navigate to Standard detail page")]
        public void ThenIShouldBeAbleToNavigateToStandardDetailPage()
        {
            ScenarioContext.Current.Pending();
        }
        
           
        
        [Then(@"I see Standard title displayed")]
        public void ThenISeeStandardTitleDisplayed()
        {
            stndDetailsPge.verifyStandardtitle();
        }

        [Then(@"I should see ""(.*)"" on standard detail page")]
        public void ThenIShouldSeeOnStandardDetailPage(string p0)
        {
            stndDetailsPge.verifyBespokeContentfields(p0);

        }


        [Then(@"I see level is displayed\.")]
        public void ThenISeeLevelIsDisplayed_()
        {
            stndDetailsPge.verifyStandardlevel();
        }
        
        [Then(@"is should see Standard page")]
        public void ThenIsShouldSeeStandardPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see typical length is displayed in months only\.")]
        public void ThenIShouldSeeTypicalLengthIsDisplayedInMonthsOnly_()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see standard detail page")]
        public void ThenIShouldSeeStandardDetailPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should not see professional registration field on detail page\.")]
        public void ThenIShouldNotSeeProfessionalRegistrationFieldOnDetailPage_()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
