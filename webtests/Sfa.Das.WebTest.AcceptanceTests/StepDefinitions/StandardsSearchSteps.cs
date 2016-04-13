
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
        


        [Given(@"I'm on Search landing page")]
        [When(@"I'm on Search landing page")]
        public void GivenIAmOnSearchLandingPage()
        {
            srchPage.Navigate();
            srchPage.WaitForSearchPage();
        }




        /// <summary>
        /// Standard search
        /// </summary>
        /// <param name="p0"></param>
        [Given(@"I enter keyword '(.*)' in search box")]
        [When(@"I enter keyword '(.*)' in search box")]
        public void GivenIEnterKeywordInSearchBox(string p0)
        {
            srchPage.SearchKeyword(p0);
        }

        /// <summary>
        /// Framework search 
        /// </summary>
        /// <param name="p0"></param>
        [Given(@"I enter framework '(.*)' in search box")]
        public void GivenIEnterFrameworkInSearchBox(string p0)
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
            srchPage.VerifyTitle("Home Page - Employer Apprenticeship Search");
        }



        [Then(@"I should see  best match '(.*)' is on top of the search list")]
        public void ThenIShouldSeeBestMatchIsOnTopOfTheSearchList(string p0)
        {
            srchPage.verifyStandardinTopofList(p0);
        }



        [Then(@"I should see matching '(.*)' standards on result page")]
        public void ThenIShouldSeeMatchingStandardsOnResultPage(string p0)
        {
            srchPage.VerifyTitle("Search Results - Employer Apprenticeship Search");
            srchPage.verifyresultsPages();
            srchPage.verifyStandardFoundinResultPage(p0);
            srchPage.verifySearchedStandardFoundinResultPage(p0);
        }

        [Then(@"I should see matching '(.*)' frameworks on result page")]
        public void ThenIShouldSeeMatchingFrameworksOnResultPage(string p0)
        {
            srchPage.VerifyTitle("Search Results - Employer Apprenticeship Search");
            srchPage.verifyresultsPages();
            srchPage.verifyStandardFoundinResultPage(p0);
            srchPage.verifySearchedStandardFoundinResultPage(p0);
        }


        //Typical length on standard search result page

        [Then(@"I should see typical length on result page\.")]
        public void ThenIShouldSeeTypicalLengthOnResultPage_()
        {
            srchPage.Verifylength();
        }


        [Then(@"I should see matching Standard '(.*)' standards on result page")]
        public void ThenIShouldSeeMatchingStandardStandardsOnResultPage(string p0)
        {
            srchPage.verifySearchedStandardFoundinResultPage(p0);
        }


        [Then(@"I should see standards count on result page")]
        public void ThenIShouldSeeStandardsCountOnResultPage()
        {
            srchPage.VerifyTitle("Search Results - Employer Apprenticeship Search");
            srchPage.verifyresultsPages();
            srchPage.VerifyresultCount();


        }


        [Then(@"I should see message ""(.*)""")]
        public void ThenIShouldSeeMessage(string p0)
        {
            srchPage.verifySearchresultMessage(p0);
           
        }




    }
}






