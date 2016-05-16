namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;

    using OpenQA.Selenium;
    using OpenQA.Selenium.PhantomJS;

    using Sfa.Das.WebTest.Infrastructure.Settings;

    using TechTalk.SpecFlow;

    public class SeleniumContext : IDisposable
    {
        private readonly IBrowserSettings _settings;

        private readonly IWebDriverFactory _factory;

        public IWebDriver WebDriver { get; private set; }

        public SeleniumContext(IBrowserSettings settings, IWebDriverFactory factory)
        {
            _settings = settings;
            _factory = factory;
        }

        public SeleniumContext InitialiseDriver()
        {
            WebDriver = CreateDriver();
            FeatureContext.Current["driver"] = WebDriver;
            return this;
        }

        private IWebDriver CreateDriver()
        {
            if (!string.IsNullOrEmpty(_settings.RemoteUrl))
            {
                return _factory.CreateBrowserStackDriver();
            }

            if (_settings.Browser.ToLower() == "phantomjs")
            {
                return _factory.CreatePhantomJsDriver();
            }

            return null;
        }

        public void Dispose()
        {
            WebDriver?.Quit();
        }
    }
}