using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework.Interfaces;
using TechTalk.SpecFlow;

namespace Specflow_Selenium_PO_Example2.Utils
{
    [Binding]
    public sealed class Hooks
    {
        static IWebDriver localDriver;
        static CustomRemoteDriver driver;
        //load from app.config
        static string host = ConfigurationManager.AppSettings["host"];
        static string browser = ConfigurationManager.AppSettings["browser"];
        static string platform = ConfigurationManager.AppSettings["platform"];
        static string browserVersion = ConfigurationManager.AppSettings["browserVersion"];
        static string saucelabsAccountName = ConfigurationManager.AppSettings["sauce_labs_account_name"];
        static string saucelabsAccountKey = ConfigurationManager.AppSettings["sauce_labs_account_key"];

        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario("web")]
        public static void BeforeWebScenario()
        {
            if (host == "localhost")
            {
                localDriver = new FirefoxDriver();
                ScenarioContext.Current["driver"] = localDriver;
            }
            else if (host == "saucelabs")
            {
                DesiredCapabilities capabilities = new DesiredCapabilities();
                capabilities.SetCapability(CapabilityType.BrowserName, browser);
                capabilities.SetCapability(CapabilityType.Platform, platform);
                capabilities.SetCapability(CapabilityType.Version, browserVersion);
                capabilities.SetCapability("username", saucelabsAccountName);
                capabilities.SetCapability("accessKey", saucelabsAccountKey);
                capabilities.SetCapability("name", TestContext.CurrentContext.Test.Name);
                //enables sauce plugin for Jenkins to display results on job page
                capabilities.SetCapability("build", Environment.GetEnvironmentVariable("JENKINS_BUILD_NUMBER"));

                driver = new CustomRemoteDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub/"), capabilities, TimeSpan.FromSeconds(600));
                ScenarioContext.Current["driver"] = driver;
            }  
        }

        [AfterScenario("web")]
        public static void AfterWebScenario()
        {
            if (host == "localhost")
            {

                if (ScenarioContext.Current.TestError != null)
                {
                    TakeScreenshot(driver);
                }
                localDriver.Quit();
            }


            else if (host == "saucelabs")
            {
                bool passed = TestContext.CurrentContext.Result.Outcome == ResultState.Success;
                try
                {
                    // Logs the result to Sauce Labs
                    ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
                    if (ScenarioContext.Current.TestError != null)
                    {
                        TakeScreenshot(driver);
                    }
                    string message = string.Format("SauceOnDemandSessionID=%1$s job-name=%2$s", driver.GetSessionId().ToString(), "some jobs name");
                    Console.Write(message);
                }
                finally
                {
                    driver.Quit();
                }
            }
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

            
      
    

