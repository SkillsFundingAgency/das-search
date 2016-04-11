namespace Sfa.Das.WebTest.Infrastructure.Settings
{
    using System.Configuration;

    public class BrowserSettings : IBrowserSettings
    {
        public string BaseUrl => ConfigurationManager.AppSettings["service.url"];

        public string Browser => ConfigurationManager.AppSettings["browser"];
    }
}