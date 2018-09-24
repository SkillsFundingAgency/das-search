using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages.Apprenticeships
{
    class ApprenticeshipProviderDetailsPage : FatBasePage
    {
        public ApprenticeshipProviderDetailsPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageTitle => " - Apprenticeship Provider - Find apprenticeship training";
       
        public override bool Verify()
        {
           return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

        public void Navigate(Uri baseUrl, string frameworkId, string ukprn, string locationId)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Provider/Detail?ukprn={ukprn}&locationId={locationId}&frameworkId={frameworkId}&isLevyPayingEmployer=True"));
        }
        public void Navigate(Uri baseUrl, string frameworkId, string ukprn, string locationId, string keyword, string postcode)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Provider/Detail?ukprn={ukprn}&locationId={locationId}&frameworkId={frameworkId}&keyword={keyword}&postcode={postcode}&isLevyPayingEmployer=True"));
        }

    }
}
