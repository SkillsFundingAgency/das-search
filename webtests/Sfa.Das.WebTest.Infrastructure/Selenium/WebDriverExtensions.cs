namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using TechTalk.SpecFlow;

    public static class WebDriverExtensions
    {
        [Obsolete("Unless you're waiting on a javascript render you should just wait for the page to load")]
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static void WaitFor(this IWebDriver driver, By by, int timeoutInSeconds = 10)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
        }

        public static string CleanUrl(this IWebDriver driver)
        {
            return driver.Url?.Split('?')[0].ToLower();
        }

        public static void TakeScreenshot(this IWebDriver driver)
        {
            try
            {
                var fileNameBase = string.Format(
                    "error_{0}_{1}_{2}",
                    FeatureContext.Current.FeatureInfo.Title.Replace(" ", "_"),
                    ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", "_"),
                    DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                var artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "testresults");
                if (!Directory.Exists(artifactDirectory))
                {
                    Directory.CreateDirectory(artifactDirectory);
                }

                var pageSource = driver.PageSource;
                var sourceFilePath = Path.Combine(artifactDirectory, fileNameBase + "_source.html");
                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
                Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));

                var takesScreenshot = driver as ITakesScreenshot;

                if (takesScreenshot != null)
                {
                    var screenshot = takesScreenshot.GetScreenshot();
                    var screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + "_screenshot.png");
                    screenshot.SaveAsFile(screenshotFilePath, ImageFormat.Png);
                    Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: {0}", ex);
            }
        }

        public static void WaitFor(this IWebDriver driver, Func<object, bool> func, int timeoutInSeconds = 10)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(func);
            }
        }
    }
}