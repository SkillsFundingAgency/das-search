namespace Sfa.Das.Sas.WebTest.Infrastructure.Selenium
{
    using OpenQA.Selenium;

    using SpecBind.BrowserSupport;

    public static class BrowserExtensions
    {
        public static IWebDriver Driver(this IBrowser browser)
        {
            var propertyInfo = browser.GetType().GetProperty("Driver");
            var getter = propertyInfo.GetGetMethod();
            return getter.Invoke(browser, new object[] { }) as IWebDriver;
        }
    }
}