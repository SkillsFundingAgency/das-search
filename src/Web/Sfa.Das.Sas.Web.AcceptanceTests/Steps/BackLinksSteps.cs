using FluentAssertions;
using TechTalk.SpecFlow;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    [Binding]
    public class BackLinksSteps : StepsBase
    {
        [Given(@"I'm on the Apprenticeship search results page")]
        public void GivenThatIMOnTheApprenticeshipSearchResultsPage()
        {
            WebSite.PageFactory.ApprenticeshipSearchResultsPage.Navigate(WebSite.BaseUrl, "account");
        }
        
        [Given(@"I'm on the Apprenticeship summary page")]
        public void GivenThatIMOnTheApprenticeshipSummaryPage()
        {
            WebSite.PageFactory.ApprenticeshipSummaryPage.NavigateToFramework(WebSite.BaseUrl, "454-2-1","northampton", null);
        }
        
       
        [Given(@"I have done an apprenticeship search and clicked on first result")]
        public void GivenIHaveDoneAnApprenticeshipSearchAndClickedOnFirstResult()
        {
            WebSite.PageFactory.ApprenticeshipSearchResultsPage.Navigate(WebSite.BaseUrl, "account");
            WebSite.PageFactory.ApprenticeshipSearchResultsPage.ClickOnResult(1);
        }

       

        [Given(@"that I'm on the Find a training provider search page")]
        public void GivenThatIMOnTheFindATrainingProviderSearchPage()
        {
           WebSite.PageFactory.FindATrainingProviderPage.Navigate(WebSite.BaseUrl, "454-2-1","account");
        }
        
        [Given(@"that I'm on the Provider summary page")]
        public void GivenThatIMOnTheProviderSummaryPage()
        {
            WebSite.PageFactory.ApprenticeProviderDetailsPage.Navigate(WebSite.BaseUrl, "454-2-1", "10007455", "129266","account","SW1+2AA");
        }
        
        [Given(@"I have done a postcode search")]
        public void GivenIHaveDoneAPostcodeSearch()
        {
            WebSite.PageFactory.FindATrainingProviderPage.Navigate(WebSite.BaseUrl, "454-2-1", "account");
        }

        [Given(@"that Im on the Provider shop window page")]
        public void GivenThatImOnTheProviderShopWindowPage()
        {
           WebSite.PageFactory.providerDetailsPage.Navigate(WebSite.BaseUrl,"10004733","northampton");
        }

        [Given(@"I have come from the Provider shop window page")]
        public void GivenIHaveComeFromTheProviderShopWindowPage()
        {
            WebSite.PageFactory.providerDetailsPage.Navigate(WebSite.BaseUrl, "10007455");
        }

        [Given(@"that I'm on the Postcode search results page")]
        public void GivenThatIMOnThePostcodeSearchResultsPage()
        {
            WebSite.PageFactory.providerLocationSearchResultPage.Navigate(WebSite.BaseUrl, "454-2-1", "SW1A+2AA");
        }
        
        [Given(@"that I'm on the Find a training provider by name page")]
        public void GivenThatIMOnTheFindATrainingProviderByNamePage()
        {
            WebSite.PageFactory.providerSearchResultsPage.Navigate(WebSite.BaseUrl,"Northampton");
        }
        
        [Given(@"that I'm on the Find apprenticeship search page")]
        public void GivenThatIMOnTheFindApprenticeshipSearchPage()
        {
            WebSite.PageFactory.providerSearchPage.Navigate(WebSite.BaseUrl);
        }
        
        [When(@"I click on the back link")]
        public void WhenIClickOnTheBackLink()
        {
            WebSite.ClickBackLink();

        }
        
        [Then(@"I am taken to the FAT search by keywords page")]
        public void ThenIAmTakenToTheFATSearchByKeywordsPage()
        {
           WebSite.PageFactory.ApprenticeshipSearchPage.Verify().Should().BeTrue();
        }
        
        [Then(@"I am taken to the Apprenticeship search results page")]
        public void ThenIAmTakenToTheApprenticeshipSearchResultsPage()
        {
            WebSite.PageFactory.ApprenticeshipSearchResultsPage.Verify().Should().BeTrue();
        }
        
        [Then(@"I am taken to the Apprenticeship summary page")]
        public void ThenIAmTakenToTheApprenticeshipSummaryPage()
        {
            WebSite.PageFactory.ApprenticeshipSummaryPage.Verify().Should().BeTrue();
        }
        
        [Then(@"I am taken to the Postcode search results page")]
        public void ThenIAmTakenToThePostcodeSearchResultsPage()
        {
            WebSite.PageFactory.postCodeSearchPage.Verify();
        }
        
        [Then(@"I am taken to the Find a training provider by name page")]
        public void ThenIAmTakenToTheFindATrainingProviderByNamePage()
        {
            WebSite.PageFactory.providerSearchResultsPage.Verify();
        }

        [Then(@"I am taken to the Provider shop window page")]
        public void ThenIAmTakenToTheProviderShopWindowPage()
        {
            WebSite.PageFactory.providerDetailsPage.Verify();
        }

        [Then(@"I am taken to the Postcode search page")]
        public void ThenIAmTakenToThePostcodeSearchPage()
        {
            WebSite.PageFactory.FindATrainingProviderPage.Verify().Should().BeTrue();
        }
        [Then(@"I am taken to the provider search results page")]
        public void ThenIAmTakenToTheProviderSearchResultsPage()
        {
            WebSite.PageFactory.providerSearchResultsPage.Verify();
        }

        [Then(@"I am taken to the search type selection page")]
        public void ThenIAmTakenToTheSearchTypeSelectionPage()
        {
            WebSite.PageFactory.ApprenticeshipOrProviderPage.Verify();
        }

        [Then(@"I am taken to the provider search page")]
        public void ThenIAmTakenToTheProviderSearchPage()
        {
            WebSite.PageFactory.providerSearchPage.Verify();
        }

    }
}
