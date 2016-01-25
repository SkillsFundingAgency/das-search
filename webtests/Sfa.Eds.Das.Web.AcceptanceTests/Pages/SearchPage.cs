
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.AcceptanceTests.Pages
{
    class SearchPage : BasePage

    {

       // private IWebDriver driver;
        By searchBox = By.Id("keywords");
        By searchButton = By.XPath("//input[@type='submit']");
        public String title = "Google";
        By searchresult = By.XPath("html/body/div[1]/div[2]/div[2]/div[3]/h3/strong");
        By searchResultcount = By.XPath("html/body/div[1]/div[2]/div[2]/div[1]/strong");

        public void launchLandingPage()
        {
            Launch("http://das-searchwebsystemtest.azurewebsites.net/", "Home Page - Employer Apprenticeship Search");
            Thread.Sleep(3000);
        }

        public void SearchKeyword(String keyword)
        {
            type(keyword,searchBox);

        }

 
        public void ValidateTitle()
        {
            Thread.Sleep(2000);
            verifyPage(title);
        }

        public void clickSearchBox()
        {
           click(searchButton);
        }



        public void verifyresultsPages()
        {

            Thread.Sleep(4000);
            Assert.True(isDisplayed(searchresult));
        }
       
        public void verifyStandardFoundinResultPage(String keyword)
        {
            Thread.Sleep(4000);
            Console.WriteLine(getText(searchresult));
           //Assert.True(getText(searchresult).Contains(keyword));
            Assert.True(isDisplayed(searchresult));
        }
       
        public void VerifyresultCount()
        {

            Assert.True(isDisplayed(searchResultcount));
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

       

        
    }

    
}
