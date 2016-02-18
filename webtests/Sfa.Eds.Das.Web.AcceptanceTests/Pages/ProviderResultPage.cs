using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    class ProviderResultPage : BasePage
    {
        /// <summary>
        /// Purpose of this class is to 
        /// Create and maintain all Provider Page objects 
        /// Create and maintain business methods  associated to this page.
        /// </summary>


        By selectStaandard = By.XPath(".//*[@id='results']/div[1]/ol/li[1]/div/h2/a");
        By searchProviderbutton = By.XPath(".//*[@id='submit-keywords']");
        By providerlist = By.XPath(".//*[@id='results']/div[1]/ol/li[1]/div/h2/a");

        public void verifyProviderResultsPage()
        {

            Thread.Sleep(4000);
            Assert.True(isDisplayed(providerlist));
        }



    }
}
