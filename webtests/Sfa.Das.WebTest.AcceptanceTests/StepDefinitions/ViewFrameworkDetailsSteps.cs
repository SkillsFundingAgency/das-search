using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ViewFrameworkDetailsSteps
    {
        SearchResultsPage srchresultPage;
        FrameworkDetailPage fwdetailPage;
        public ViewFrameworkDetailsSteps()
        {
            srchresultPage = new SearchResultsPage();
            fwdetailPage = new FrameworkDetailPage();
        }

        [When(@"I pick framework '(.*)' from search result page")]
        public void WhenIChooseFrameworkFromSearchResultPage(string p0)
        {
            srchresultPage.searchChooseStandard(p0);
        }

        [Then(@"I should see Framework '(.*)' on framework detail page")]
        public void ThenIShouldSeeFrameworkOnFrameworkDetailPage(string p0)
        {
            fwdetailPage.verifyFrameworkTitle(p0);
        }


        [Then(@"I should see Framework pathway '(.*)' on framework detail page")]
        public void ThenIShouldSeeFrameworkPathwayOnFrameworkDetailPage(string p0)
        {
            fwdetailPage.verifyFrameworkPathway(p0);
        }


    }
}
