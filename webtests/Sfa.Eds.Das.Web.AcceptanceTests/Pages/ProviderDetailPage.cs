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
        By providerDetailLsatisfaction = By.CssSelector("#learner-satisfaction");
        By providerDetailEsatisfaction = By.CssSelector("#employer-satisfaction");
        By websiteCoursepage = By.Id("course-link");
        By websitecontactpage = By.Id("contact-link");
        By trainingStructure = By.Id("delivery-modes");
        By trainingLocation = By.Id("training-location");
        

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
