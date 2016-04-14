using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    using Sfa.Das.WebTest.Infrastructure;

    class StandardDetailsPage : BasePage
    {
        By standardDetailTitle = By.XPath(".//*[@id='content']/div/div[1]/div/h1");
        By standardDetailLevel = By.XPath(".//*[@id='content']/section/dl/dd[2]");
        By standardDetailIntrotext = By.XPath(".//*[@id='content']/div/div[1]/div/div/p");
        By standardDetaillength = By.XPath(".//*[@id='content']/section/dl/dd[3]");
        By standardDetailEntryReq = By.XPath(".//*[@id='content']/section/dl/dd[4]");
        By standardDetailApplearn = By.XPath(".//*[@id='content']/section/dl/dd[5]");
        By standardDetailQualification = By.XPath(".//*[@id='content']/section/dl/dd[6]/p");
        By standardDetailProfReg = By.XPath(".//*[@id='content']/section/dl/dd[7]/p");
        By standardDetailOverviewRole = By.XPath(".//*[@id='content']/section/dl/dd[1]/p");

        By postCodeValidation = By.Id("postcode-error");
        By SearchProvidermsg = By.Id("standard-provider-search-message");


        private By postCodeTextBox = By.Id("postcode");

        private By searchButton = By.Id("submit-postcode");

        public void WaitToLoad()
        {
            base.WaitFor(searchButton);
        }

        public void verifyStandardtitle()
        {
            Assert.True(isDisplayed(standardDetailTitle));
        }

        public void verifyStandardlevel()
        {
            Assert.True(isDisplayed(standardDetailLevel));
        }

        public void validateErrorMessage_postcodefield(string errmsg)
        {
            AssertContainsText(postCodeValidation, errmsg);
        }
        public void validateProviderSrchResultMsg(string msg)
        {
            AssertContainsText(SearchProvidermsg, msg);
        }

        public void verifyBespokeContentfields(String metadata)
        {
            switch (metadata)
            {
                case "Introductory text":
                    Assert.True(isDisplayed(standardDetailIntrotext));
                    break;

                case "Typical length":
                    Assert.True(isDisplayed(standardDetaillength));
                    break;
                case "What apprentices will learn":
                    Assert.True(isDisplayed(standardDetailApplearn));
                    break;
                case "Entry requirements":
                    Assert.True(isDisplayed(standardDetailEntryReq));
                    break;
                case "Qualifications":
                    Assert.True(isDisplayed(standardDetailQualification));
                    break;

                case "Overview of Role":
                    Assert.True(isDisplayed(standardDetailOverviewRole));
                    break;

                case "Professional registration":
                    Assert.True(isDisplayed(standardDetailProfReg));
                    break;

                default:
                    Assert.True(isDisplayed(standardDetailTitle));
                    break;

            }

        }

        public void ClickButton()
        {
            click(searchButton);
        }

        public void enterlocation(String location)
        {
            type(location, postCodeTextBox);


        }
    }
}
