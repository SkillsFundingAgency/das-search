namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    using Sfa.Eds.Das.Web.AcceptanceTests.Pages;

    using TechTalk.SpecFlow;

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

        [When(@"I pick '(.*)' from search result page")]
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

        [Then(@"I should see error message ""(.*)""")]
        public void ThenIShouldSeeErrorMessage(string p0)
        {
            standardDetailsPage.validateErrorMessage_postcodefield(p0);
        }

        [Then(@"I should see message searchresult ""(.*)""")]
        public void ThenIShouldSeeMessageSearchresult(string p0)
        {
            standardDetailsPage.validateProviderSrchResultMsg(p0);
        }
    }
}