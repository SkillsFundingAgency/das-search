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
        SearchPage srchPage;

        By selectStaandard = By.XPath(".//*[@id='results']/div[1]/ol/li[1]/div/h2/a");
        By searchProviderbutton = By.XPath(".//*[@id='submit-keywords']");
        By providerlist = By.XPath(".//*[@id='results']/div[1]/div[2]/p");
        By providersearchbox = By.XPath(".//*[@id='postcode']");
        By providerlist1 = By.XPath(".//*[@id='results']/div[1]/ol/li/div/h2");
        

        public void verifyProviderResultsPage()
        {

            Thread.Sleep(4000);
            Assert.True(isDisplayed(providerlist));
        }

        public void enterlocation(String location)
        {
            type(location, providersearchbox);
           

        }

        public void verifyProviderinSearchResults(String p0)
        {
            Assert.True(isElementPresent(providerlist1, p0));
        }


        

    }
}
