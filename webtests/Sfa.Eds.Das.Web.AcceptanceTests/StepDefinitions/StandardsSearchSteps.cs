
using Sfa.Eds.Das.Web.AcceptanceTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using TechTalk.SpecFlow;


namespace Sfa.Eds.Das.Web.AcceptanceTests.StepDefinitions
{

    
   
  [Binding]
    public class StandardsSearchSteps
    {

        SearchPage srchPage;
       

         

        [Given(@"I am on Search landing page")]
        public void GivenIAmOnSearchLandingPage()
        {

            srchPage = new SearchPage();
            srchPage.StartSelenium();
            srchPage.launchLandingPage();
            
            
            
      

           // ScenarioContext.Current.Pending();
        }
        
        [Given(@"I enter keyword '(.*)' in search box")]
        public void GivenIEnterKeywordInSearchBox(string p0)
        {
            // srchPage.enterSearchBox(p0);
            //  ScenarioContext.Current.Pending();

            srchPage.SearchKeyword(p0);

        }
        
        [When(@"I click on search button")]
        public void WhenIClickOnSearchButton()
        {
            srchPage.clickSearchBox();
        }


        [Then(@"I should see matching '(.*)' standards on result page")]
        public void ThenIShouldSeeMatchingStandardsOnResultPage(string p0)
        {
            srchPage.verifyPage("- Employer Apprenticeship Search");
            srchPage.verifyresultsPages();
            srchPage.verifyStandardFoundinResultPage(p0);
           
        }

[AfterScenario]

public void closeBrowser()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                IWebDriver driver = null;
                TakeScreenshot(driver);
            }
            srchPage.closeBrowser();


        }
        private static void TakeScreenshot(IWebDriver driver)
        {
            try
            {
                string fileNameBase = string.Format("error_{0}_{1}_{2}",
                    FeatureContext.Current.FeatureInfo.Title,
                    ScenarioContext.Current.ScenarioInfo.Title,
                    DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                var artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "testresults");
                if (!Directory.Exists(artifactDirectory))
                    Directory.CreateDirectory(artifactDirectory);

                string pageSource = driver.PageSource;
                string sourceFilePath = Path.Combine(artifactDirectory, fileNameBase + "_source.html");
                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
                Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));

                ITakesScreenshot takesScreenshot = driver as ITakesScreenshot;

                if (takesScreenshot != null)
                {
                    var screenshot = takesScreenshot.GetScreenshot();
                    string screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + "_screenshot.png");
                    screenshot.SaveAsFile(screenshotFilePath, ImageFormat.Png);
                    Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: {0}", ex);
            }
        }
    }




}

