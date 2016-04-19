using System;
using TechTalk.SpecFlow;

namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{
    using Sfa.Das.WebTest.AcceptanceTests.Pages;

    /// <summary>
    /// Purpose of this Step definition class is to 
    /// Create and maintain all step definitions related provider search functionality.
    /// Any change to business features under provider search can be modified here.
    ///  
    /// </summary>

    [Binding]
    public class ProviderResultsSteps
    {
        ProviderResultPage providerResultPage;

        public ProviderResultsSteps()
        {
            providerResultPage = new ProviderResultPage();
        }

        [Then(@"I am on the provider results page")]
        public void ThenIAmOnTheProviderResultsPage()
        {
            providerResultPage.WaitToLoad();
        }

        
        [Then(@"I should all providers in result page")]
        public void ThenIShouldAllProvidersInResultPage()
        {
            providerResultPage.verifyProviderResultsPage();
        }

        [Then(@"I should see all providers in result page")]
        public void ThenIShouldSeeAllProvidersInResultPage()
        {
            providerResultPage.verifyProviderResultsPage();
        }

        [Then(@"under each provider I should see provider ""(.*)""")]
        public void ThenUnderEachProviderIShouldSeeProvider(string p0)
        {
            providerResultPage.verifyProvidersearchResultsInfo(p0);
        }

        [Then(@"I should see provider ""(.*)""")]
        public void ThenIShouldSeeProvider(string p0)
        {
            providerResultPage.verifyProvidersearchResultsInfo(p0);
        }
    }
}
