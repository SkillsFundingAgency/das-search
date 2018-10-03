using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Pages.Apprenticeships;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    public class PageFactory
    {
        private readonly IWebDriver _webDriver;

        public PageFactory(IWebDriver driver)
        {
            this._webDriver = driver;
        }

        internal ApprenticeshipOrProviderPage ApprenticeshipOrProviderPage => new ApprenticeshipOrProviderPage(_webDriver);

        internal ApprenticeshipPage ApprenticeshipSummaryPage => new ApprenticeshipPage(_webDriver);
        internal SearchApprenticeshipPage ApprenticeshipSearchPage => new SearchApprenticeshipPage(_webDriver);
        internal ApprenticeshipSearchResultPage ApprenticeshipSearchResultsPage => new ApprenticeshipSearchResultPage(_webDriver);
        internal ApprenticeshipProviderDetailsPage ApprenticeProviderDetailsPage => new ApprenticeshipProviderDetailsPage(_webDriver);

        internal PostCodeSearchPage postCodeSearchPage => new PostCodeSearchPage(_webDriver);
        internal FindATrainingProviderPage FindATrainingProviderPage => new FindATrainingProviderPage(_webDriver);
        internal ProviderLocationSearchResultPage providerLocationSearchResultPage => new ProviderLocationSearchResultPage(_webDriver);

        internal ProviderSearchPage providerSearchPage => new ProviderSearchPage(_webDriver);
        internal ProviderDetailsPage providerDetailsPage => new ProviderDetailsPage(_webDriver);
        internal ProviderSearchResultsPage providerSearchResultsPage => new ProviderSearchResultsPage(_webDriver);
      

    }
}
