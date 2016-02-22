﻿
using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using TechTalk.SpecFlow;


namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{

    /// <summary>
    /// Purpose of this Step definition class is to 
    /// Create and maintain all step definitions related Standard search functionality.
    /// Any change to business features under Standard search can be modified here.
    /// </summary>

    [Binding]
    
    public class StandardsSearchSteps
    {




        SearchPage srchPage;
        public StandardsSearchSteps()
        {
           
            srchPage = new SearchPage();
        }
        


        [Given(@"I am on Search landing page")]
        public void GivenIAmOnSearchLandingPage()
        {


           // srchPage = new SearchPage();

           srchPage.launchLandingPage();





            // ScenarioContext.Current.Pending();
        }

        [When(@"I am on Search landing page")]
        public void WhenIAmOnSearchLandingPage()
        {
           // srchPage = new SearchPage();

           srchPage.launchLandingPage();
        }


        [Given(@"I enter keyword '(.*)' in search box")]
        public void GivenIEnterKeywordInSearchBox(string p0)
        {


            srchPage.SearchKeyword(p0);

        }

        [Given(@"I click on search button")]
        public void GivenIClickOnSearchButton()
        {
            srchPage.clickSearchBox();
        }

        [When(@"I click on search button")]
        public void WhenIClickOnSearchButton()
        {
            srchPage.clickSearchBox();
        }


        [Then(@"I should be able to see home page with title as ""(.*)""")]
        public void ThenIShouldBeAbleToSeeHomePageWithTitleAs(string p0)
        {
            srchPage.verifyPage("Home Page - Employer Apprenticeship Search");
        }



        [Then(@"I should see matching '(.*)' standards on result page")]
        public void ThenIShouldSeeMatchingStandardsOnResultPage(string p0)
        {
            srchPage.verifyPage("Search Results - Employer Apprenticeship Search");
            srchPage.verifyresultsPages();
            srchPage.verifyStandardFoundinResultPage(p0);

        }

        [Then(@"I should see standards count on result page")]
        public void ThenIShouldSeeStandardsCountOnResultPage()
        {
            srchPage.verifyPage("Search Results - Employer Apprenticeship Search");
            srchPage.verifyresultsPages();
            srchPage.VerifyresultCount();


        }


        [Then(@"I should see message ""(.*)""")]
        public void ThenIShouldSeeMessage(string p0)
        {
            srchPage.VerifyresultCount_invalidSearch();
        }




    }
}






