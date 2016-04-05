using NUnit.Framework;
using OpenQA.Selenium;
using Sfa.Das.WebTest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    class FrameworkDetailPage : BasePage
    {


        By frameworkDetailTitle = By.CssSelector(".heading-xlarge");
        By frameworkDetailLevel = By.XPath(".//*[@id='content']/section/dl/dd[2]");
        By frameworkDetailPathway = By.CssSelector(".heading-large");
        By frameworkDetailContent = By.XPath(".//*[@id='content']/section/dl/dd[3]");
        By standardDetailEntryReq = By.XPath(".//*[@id='content']/section/dl/dd[4]");

        public void verifyFrameworkTitle(String title)
        {
            Assert.True(verifyTextMessage(frameworkDetailTitle,title));
        }

        public void verifyFrameworkPathway(String pathway)
        {
            Assert.True(verifyTextMessage(frameworkDetailPathway, pathway));
        }


    }







}
