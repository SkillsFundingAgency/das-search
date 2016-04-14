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

        [When(@"I select any of the provider from the list")]
        public void WhenISelectAnyOfTheProviderFromTheList()
        {
            prvdrPage.chooseProvider();
        }
       
        
        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string p0)
        {
            pdetailPage.verifyProviderDetailPageFields(p0);
        }
    }
}
