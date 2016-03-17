using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    class ProviderDetailPage : BasePage
    {
        By providerDetailName = By.XPath(".//*[@id='content']/div/div[1]/div/h1");
        By providerDetailLsatisfaction = By.XPath(".//*[@id='content']/section/dl/dd[2]");
        By providerDetailEsatisfaction = By.XPath(".//*[@id='content']/div/div[1]/div/div/p");
        By providerDetaillength = By.XPath(".//*[@id='content']/section/dl/dd[3]");
        By standardDetailEntryReq = By.XPath(".//*[@id='content']/section/dl/dd[4]");
        By standardDetailApplearn = By.XPath(".//*[@id='content']/section/dl/dd[5]");
        By standardDetailQualification = By.XPath(".//*[@id='content']/section/dl/dd[6]/p");
        By standardDetailProfReg = By.XPath(".//*[@id='content']/section/dl/dd[7]/p");
        By standardDetailOverviewRole = By.XPath(".//*[@id='content']/section/dl/dd[1]/p");

    }
}
