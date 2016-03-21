
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

        /// <summary>
        /// Purpose of this class is to 
        /// Create and maintain all Search Page objects and methods.
        /// Any changes to search functionality can be maintained under this page.
        /// </summary>

        // private IWebDriver driver;
        By searchBox = By.Id("keywords");
        By searchButton = By.XPath("//input[@type='submit']");
        public String title = "Google";
        By searchresult = By.XPath(".//*[@id='results']/div[1]/p");
        By searchkeywordresult = By.XPath(".//*[@id='results']/div[1]/article/header/h2/a");
        By searchResultcount = By.XPath(".//*[@id='results']/div[1]/p");
        By typicallength = By.XPath(".//*[@id='results']/div[1]/article/dl/dd[2]");

        By selectStaandard = By.XPath(".//*[@id='results']/div[1]/article[1]/header/h2/a");
        By searchProviderbutton = By.XPath(".//*[@id='submit-keywords']");

        public void launchLandingPage()
        {
            Launch("Home Page - Employer Apprenticeship Search");
            Thread.Sleep(3000);
        }

        public void OpenStandarDetails(String standard)
        {
            Open(standard);
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
            Thread.Sleep(3000);
        }

        public void clickProviderSearch()
        {
            click(searchProviderbutton);
            Thread.Sleep(3000);
        }

        


        public void chooseStandard()
        {
            click(selectStaandard);
            Thread.Sleep(3000);
        }




        public void verifyresultsPages()
        {

            Thread.Sleep(4000);
            Assert.True(isDisplayed(searchresult));
        }
       
        public void verifyStandardFoundinResultPage(String keyword)
        {
            Thread.Sleep(4000);
           // Console.WriteLine(getText(searchresult));
          //  Console.WriteLine(getText(searchkeywordresult));
            //Console.WriteLine(keyword);
            //Assert.True(getText(searchkeywordresult).Contains(keyword));
           Assert.True(isDisplayed(searchresult));
           // Assert.True(isElementPresent(searchkeywordresult, keyword));
        }

        public void verifySearchedStandardFoundinResultPage(String keyword)
        {
            Thread.Sleep(4000);
             Assert.True(isElementPresent(searchkeywordresult, keyword));
        }


        public void VerifyresultCount()
        {

            Assert.True(getText(searchResultcount).Contains("Total results found:"));
        }

        public void Verifylength()
        {

            Assert.True(getText(typicallength).Contains("24 to 36 months"));
        }


        public void VerifyresultCount_invalidSearch()
        {

            Assert.True(getText(searchResultcount).Contains("Total results found: 0"));
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
