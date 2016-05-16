namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using OpenQA.Selenium;

    public interface IWebDriverFactory
    {
        IWebDriver CreateBrowserStackDriver();

        IWebDriver CreatePhantomJsDriver();
    }
}