namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Remote;

    using Sfa.Das.WebTest.Infrastructure.Settings;

    using TechTalk.SpecFlow;

    public class SeleniumContext : IDisposable
    {
        private BrowserSettings _settings;

        static string host = ConfigurationManager.AppSettings["host"];
        static string testExecution = ConfigurationManager.AppSettings["testExecutionType"];
        static string saucelabsAccountName = ConfigurationManager.AppSettings["sauce_labs_account_name"];
        static string saucelabsAccountKey = ConfigurationManager.AppSettings["sauce_labs_account_key"];
        static String browserStackKey = ConfigurationManager.AppSettings["browserstack_key"];
        static String browserStacUser = ConfigurationManager.AppSettings["browserstack_user"];
        static String mDevice = ConfigurationManager.AppSettings["mobile_device"];

        public IWebDriver WebDriver { get; private set; }

        public SeleniumContext()
        {
            _settings = new BrowserSettings();

            WebDriver = CreateDriver();
            FeatureContext.Current["driver"] = WebDriver;
        }

        private IWebDriver CreateDriver()
        {
            if (!string.IsNullOrEmpty(_settings.RemoteUrl))
            {
                var capabilities =
                    FindBrowserCapability()
                        .SafeSet("os", _settings.OS)
                        .SafeSet("os_version", _settings.OSVersion)
                        .SafeSet("browser_version", _settings.BrowserVersion)
                        .SafeSet("browserstack.debug", "true")
                        .SafeSet("browserstack.user", _settings.BrowserStackUser)
                        .SafeSet("browserstack.key", _settings.BrowserStackKey)
                        .SafeSet("project", _settings.Project)
                        .SafeSet("device", _settings.Device)
                        .SafeSet("resolution", _settings.Resolution)
                        .SafeSet("build", _settings.Build);

                capabilities.SetCapability("name", GenerateTestName());

                Console.WriteLine($"##### Driver: Browserstack - {_settings.OS} {_settings.OSVersion} - {_settings.Device ?? _settings.Browser} {_settings.BrowserVersion}");
                var remoteWebDriver = new RemoteWebDriver(new Uri(_settings.RemoteUrl), capabilities);
                return remoteWebDriver;
            }
            else
            {
                switch (_settings.Browser.ToLower())
                {
                    case "phantomjs":
                        return CreatePhantomJsDriver();
                }
            }

            return null;
        }

        private string GenerateTestName()
        {
            var arr = TestContext.CurrentContext.Test.Name.Replace(")", "").Replace(",null", "").Split('(');
            return FeatureContext.Current.FeatureInfo.Title + " - " + SplitCamelCase(arr[0]) + " " + arr[1];
        }

        private string SplitCamelCase(string input)
        {
            return Regex.Replace(input, "(\\B[A-Z])", " $1");
        }

        private DesiredCapabilities FindBrowserCapability()
        {
            switch (_settings.Browser.ToLower())
            {
                case "internet explorer":
                    return DesiredCapabilities.InternetExplorer();
                case "chrome":
                    return DesiredCapabilities.Chrome();
                case "firefox":
                    return DesiredCapabilities.Firefox();
                case "android":
                    return DesiredCapabilities.Android();
                case "edge":
                    return DesiredCapabilities.Edge();
                case "safari":
                    return DesiredCapabilities.Safari();
                case "iphone":
                    return DesiredCapabilities.IPhone();
                case "ipad":
                    return DesiredCapabilities.IPad();
            }

            throw new WebDriverException($"Browser Type '{_settings.Browser}' is not supported as a remote driver.");
        }

        private static IWebDriver CreatePhantomJsDriver()
        {
            var driverService = PhantomJSDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            Console.WriteLine($"##### Driver: PhantomJs");
            var driver = new PhantomJSDriver(driverService);
            driver.Manage().Window.Maximize();
            return driver;
        }

        public void Dispose()
        {
            WebDriver?.Quit();
        }
    }
}