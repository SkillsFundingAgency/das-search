using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    using Sfa.Das.WebTest.Infrastructure;

    class ProviderDetailPage : BasePage
    {
        By providerDetailName = By.XPath(".//*[@id='content']/div/div[1]/div/h1");
        By providerDetailStandardtitle = By.XPath(".//*[@id='content']/section/header/h2");
        By providerDetailLsatisfaction = By.XPath(".//*[@id='content']/div/div[2]/form/div/aside/h3[1]");
        By providerDetailEsatisfaction = By.XPath(".//*[@id='content']/div/div[2]/form/div/aside/h3[2]");
        By websiteCoursepage = By.XPath(".//*[@id='content']/section/dl/dd[3]");
        By websitecontactpage = By.XPath(".//*[@id='content']/section/dl/dd[4]");
        By trainingStructure = By.XPath(".//*[@id='content']/section/dl/dt[5]");
        By trainingLocation = By.XPath(".//*[@id='content']/section/dl/dt[6]");
        By standardDetailProfReg = By.XPath(".//*[@id='content']/section/dl/dd[7]/p");
        By standardDetailOverviewRole = By.XPath(".//*[@id='content']/section/dl/dd[1]/p");

        SearchPage srchPage;



        public void verifyProviderDetailPage(String info)
        {

            switch (info)
            {
                case "Provider name":
                    Assert.True(isDisplayed(providerDetailName));
                    break;

                case "Standard name":
                    Assert.True(isDisplayed(providerDetailStandardtitle));
                    break;
                case "Learner satisfaction":
                    Assert.True(isDisplayed(providerDetailLsatisfaction));
                    break;

                case "Employer satisfaction":
                    Assert.True(isDisplayed(providerDetailLsatisfaction));
                    break;

                case "Website course page":
                    Assert.True(isDisplayed(websiteCoursepage));
                    break;


                case "Website contact page":
                    Assert.True(isDisplayed(websitecontactpage));
                    break;


            }
        }





    }
}
