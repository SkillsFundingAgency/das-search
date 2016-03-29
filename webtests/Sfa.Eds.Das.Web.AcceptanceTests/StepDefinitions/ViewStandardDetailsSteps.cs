using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ViewStandardDetailsSteps
    {
        StandardDetailsPage stndDetailsPge;

        public ViewStandardDetailsSteps()
        {
            stndDetailsPge = new StandardDetailsPage();
        }

        [Then(@"I am on a Standard details page")]
        public void ThenIAmOnAStandardDetailsPage()
        {
            stndDetailsPge.WaitToLoad();
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
    }
}
