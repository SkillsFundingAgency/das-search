using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
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

       public void verifyStandardtitle()
        {
            Assert.True(isDisplayed(standardDetailTitle));

        }

        public void verifyStandardlevel()
        {
            Assert.True(isDisplayed(standardDetailLevel));
        }


        public void verifyBespokeContentfields(String metadata)
        {
            switch (metadata)
            {
                case "Introductory text":
                    Assert.True(isDisplayed(standardDetailIntrotext));
                    break;

                case "Typical length":
                    Assert.True(isDisplayed(standardDetailIntrotext));
                    break;
                case "What apprentices will learn":
                    Assert.True(isDisplayed(standardDetailIntrotext));
                    break;
                case "Entry requirements":
                    Assert.True(isDisplayed(standardDetailIntrotext));
                    break;
                case "Qualifications":
                    Assert.True(isDisplayed(standardDetailIntrotext));
                    break;

                case "Professional registration":
                    Assert.True(isDisplayed(standardDetailIntrotext));
                    break;

                default:
                    Assert.True(isDisplayed(standardDetailTitle));
                    break;

            }

        }          



    }
}
