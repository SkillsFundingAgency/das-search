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
        By providerlist1 = By.XPath(".//*[@id='results']/div[1]/article/header/h2/a");
        By selectprovider = By.XPath(".//*[@id='results']/div[1]/article[1]/header/h2/a");
        By postCodeValidation = By.XPath(".//*[@id='content']/div/div[2]/form/div/aside/div/label/div/p");
        By SearchProvidermsg = By.XPath(".//*[@id='results']/div[1]/div[2]/p");
        By providerwebsite = By.XPath(".//*[@id='results']/div[1]/article[1]/dl/dt[2]");
        By providereSatisfaction = By.XPath(".//*[@id='results']/div[1]/article[8]/dl/dt[3]");
        By providerlSatisfaction = By.XPath(".//*[@id='results']/div[1]/article[8]/dl/dt[4]");
        By providerLocation = By.XPath(".//*[@id='results']/div[1]/article[3]/dl/dd[2]");



        public void verifyProviderResultsPage()
        {

            Assert.True(isDisplayed(providerlist));
            Sleep(4000);
        }

        public void verifyProvidersearchResultsInfo(String info)
        {

            switch(info)
            {
                case "website":
                    Assert.True(isDisplayed(providerwebsite));
                    break;

                case "Employer satisfaction":
                    Assert.True(isDisplayed(providereSatisfaction));
                    break;
                case "Learner satisfaction":
                    Assert.True(isDisplayed(providerlSatisfaction));
                    break;


            }

            Sleep(4000);
            Assert.True(isDisplayed(providerlist));
        }

        public void chooseProvider()
        {
            click(selectprovider);
            Sleep(3000);
        }

        public void enterlocation(String location)
        {
            type(location, providersearchbox);
           

        }


        public void validateErrorMessage_postcodefield(String errmsg)
        {
            Assert.True(verifyTextMessage(postCodeValidation, errmsg));
        }
        public void validateProviderSrchResultMsg(String msg)
        {
            Assert.True(verifyTextMessage(SearchProvidermsg, msg));
        }


        public void verifyProviderinSearchResults(String p0)
        {
            Assert.True(isElementPresent(providerlist1, p0));
        }

        public void verifyProviderLocationinSearchResults(String p0)
        {
            verifyTextMessage(providerLocation, p0);
        }

        public void verifyProviderNotinSearchResults(String p0)
        {
            Assert.True(isElementNotPresent(providerlist1, p0));
        }



    }
}
