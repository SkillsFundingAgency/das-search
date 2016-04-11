/*
Purpose of this class is to 
-Define actions to perform before and after feature/Scenario
-Define local and remote test execution, sauce lab settings and local host browser setting are defined

*/
namespace Sfa.Das.WebTest.Infrastructure.Hooks
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;

    using BoDi;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Remote;

    using Sfa.Das.WebTest.Infrastructure.Selenium;
    using Sfa.Das.WebTest.Infrastructure.Settings;

    using TechTalk.SpecFlow;

    [Binding]
    public sealed class Hooks
    {

        private readonly IObjectContainer _objectContainer;

        private static SeleniumContext seleniumContext;

        public Hooks(IObjectContainer objectContainer)
        {
            this._objectContainer = objectContainer;

            _objectContainer.RegisterTypeAs<BrowserSettings, IBrowserSettings>();
            _objectContainer.RegisterTypeAs<PageContext, IPageContext>();

            //foreach (var type in GetTypesWithPageNavigationAttribute())
            //{
            //    _objectContainer.RegisterInstanceAs();
            //}

        }

        static IEnumerable<Type> GetTypesWithPageNavigationAttribute()
        {
            return from asm in AppDomain.CurrentDomain.GetAssemblies() from type in asm.GetTypes() where type.IsClass && type.GetCustomAttributes(typeof(PageNavigationAttribute), true).Length > 0 select type;
        }

        /// <summary>
        ///  Purupse of this Hooks class is to 
        ///  Maintain shared objects across the feature scenarios
        ///  Declare any standard actions required to run pre and post scneario and feature
        ///  Declare and instantiate driver object which is shared across all features.
        /// </summary>

        //load from app.config
        static string host = ConfigurationManager.AppSettings["host"];
        static string url = ConfigurationManager.AppSettings["service.url"];
        static string testExecution = ConfigurationManager.AppSettings["testExecutionType"];
        static string saucelabsAccountName = ConfigurationManager.AppSettings["sauce_labs_account_name"];
        static string saucelabsAccountKey = ConfigurationManager.AppSettings["sauce_labs_account_key"];
        static String browserStackKey = ConfigurationManager.AppSettings["browserstack_key"];
        static String browserStacUser = ConfigurationManager.AppSettings["browserstack_user"];
        static String mDevice= ConfigurationManager.AppSettings["mobile_device"];
        static string applicationVersion = ConfigurationManager.AppSettings["application.version"];



        //public static CustomRemoteDriver driver { get; private set; }

        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            WaitForDeployment();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            Console.WriteLine("#####################  Feature Run- Started  ######################");
            Console.WriteLine("Feature : " + FeatureContext.Current.FeatureInfo.Title);
            seleniumContext = new SeleniumContext();
        }

        [BeforeScenario]

        public void BeforeWebScenario()
        {
            _objectContainer.RegisterInstanceAs<IWebDriver>(seleniumContext.WebDriver);
            Console.WriteLine("##### Test Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }


        [AfterScenario]
        public void AfterWebScenario()
        {
            if (host == "localhost")
            {

                if (ScenarioContext.Current.TestError != null)
                {
                    TakeScreenshot(seleniumContext.WebDriver);
                }
            }
        }

        [AfterFeature]
        public static void AfterFeatureRun()
        {
            Console.WriteLine("###################### Feature Run-Ended #######################");
            if (host == "localhost")
            {
                seleniumContext.WebDriver.Quit(); // kill driver after feature run.
            }

            if (host == "browserstack" || host == "saucelabs")
            {
                seleniumContext.WebDriver.Quit();
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
        }

        private static void WaitForDeployment()
        {
            if (!string.IsNullOrEmpty(applicationVersion))
            {
                var client = new WebClient();

                var version = client.DownloadString(url + "/api/version").Replace("\"", "");

                if (version != applicationVersion)
                {
                    if (!Retry.DoUntil(() => client.DownloadString(url + "/api/version").Replace("\"", "") == applicationVersion, TimeSpan.FromSeconds(3)))
                    {
                        if (version != applicationVersion)
                        {
                            throw new VersionNotFoundException($"site was version {version} but we expected {applicationVersion}");
                        }
                    }
                }
            }
        }

        private static void TakeScreenshot(IWebDriver driver)
        {
            try
            {
                string fileNameBase = string.Format("error_{0}_{1}_{2}",
                    FeatureContext.Current.FeatureInfo.Title.Replace(" ","_"),
                    ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", "_"),
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
