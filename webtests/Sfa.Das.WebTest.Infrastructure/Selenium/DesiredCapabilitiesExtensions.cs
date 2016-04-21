namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using OpenQA.Selenium.Remote;

    public static class DesiredCapabilitiesExtensions
    {
        public static DesiredCapabilities SafeSet(this DesiredCapabilities capabilities, string key, object value)
        {
            if (value != null)
            {
                capabilities.SetCapability(key, value);
            }

            return capabilities;
        }
    }
}