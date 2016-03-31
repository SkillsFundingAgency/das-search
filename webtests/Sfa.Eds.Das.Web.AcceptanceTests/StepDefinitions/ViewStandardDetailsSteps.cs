using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ViewStandardDetailsSteps
    {
        StandardDetailsPage standardDetailsPage;
        SearchResultsPage srchresultPage;


        public ViewStandardDetailsSteps()
        {
            standardDetailsPage = new StandardDetailsPage();
            srchresultPage = new SearchResultsPage();
        }

        [Then(@"I am on a Standard details page")]
        public void ThenIAmOnAStandardDetailsPage()
        {
            standardDetailsPage.WaitToLoad();
        }

        [Then(@"I see Standard title displayed")]
        public void ThenISeeStandardTitleDisplayed()
        {
            standardDetailsPage.verifyStandardtitle();
        }

        [Then(@"I should see ""(.*)"" on standard detail page")]
        public void ThenIShouldSeeOnStandardDetailPage(string p0)
        {
            standardDetailsPage.verifyBespokeContentfields(p0);
        }

        [Then(@"I see level is displayed\.")]
        public void ThenISeeLevelIsDisplayed_()
        {
            standardDetailsPage.verifyStandardlevel();
        }

        [When(@"I search Search for provider")]
        public void WhenISearchSearchForProvider()
        {
            standardDetailsPage.ClickButton();
        }

        [When(@"I choose '(.*)' from search result page")]
        public void WhenIChooseFromSearchResultPage(string p0)
        {
            srchresultPage.searchChooseStandard(p0);
        }


        [When(@"I enter '(.*)' in provider search box")]
        [Given(@"I enter ""(.*)""  in provider search box")]
        [Given(@"I enter '(.*)' in provider search box")]
        public void WhenIEnterInProviderSearchBox(string p0)
        {
            standardDetailsPage.enterlocation(p0);
        }
    }
}
