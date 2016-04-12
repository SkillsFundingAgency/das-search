namespace Sfa.Das.WebTest.Infrastructure.Settings
{
    using System.Configuration;

    public class BrowserSettings : IBrowserSettings
    {
        public string BaseUrl => ConfigurationManager.AppSettings["service.url"];

        public string Browser => ConfigurationManager.AppSettings["browser"];

        public string RemoteUrl => ConfigurationManager.AppSettings["RemoteUrl"];

        public string BrowserStackUser => ConfigurationManager.AppSettings["browserstack.user"];

        public string BrowserStackKey => ConfigurationManager.AppSettings["browserstack.key"];

        public string OS => ConfigurationManager.AppSettings["os"];

        public string OSVersion => ConfigurationManager.AppSettings["os_version"];

        public string Version => ConfigurationManager.AppSettings["version"];
    }
}