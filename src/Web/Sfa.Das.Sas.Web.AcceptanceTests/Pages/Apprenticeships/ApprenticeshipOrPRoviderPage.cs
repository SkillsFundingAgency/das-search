using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Sfa.Das.Sas.Web.AcceptanceTests.Helpers;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages.Apprenticeships
{
    class ApprenticeshipOrProviderPage : FatBasePage
    {
        public ApprenticeshipOrProviderPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageTitle => "Apprenticeship Or Provider - Find apprenticeship training";
      
        public override bool Verify()
        {
           return PageInteractionHelper.VerifyPageHeading(Driver.Title, PageTitle);
        }

        public void Navigate(Uri baseUrl, string frameworkId, string keywords)
        {
            Driver.Navigate().GoToUrl(baseUrl.Combine($"Apprenticeship/ApprenticeshipOrProvider"));
        }
      
    }
}
