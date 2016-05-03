namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    using Sfa.Das.WebTest.Infrastructure.Services;
    using Sfa.Das.WebTest.Infrastructure.Settings;

    public class BrowserStackApi : IBrowserStackApi
    {
        private readonly IBrowserSettings _settings;

        private readonly IWebDriver _driver;

        private readonly IRetryWebRequests _retryService;

        private readonly ILog _logger;

        public BrowserStackApi(IBrowserSettings settings, IWebDriver driver, IRetryWebRequests retryService, ILog logger)
        {
            _settings = settings;
            _driver = driver;
            _retryService = retryService;
            _logger = logger;
        }

        public void FailTestSession(Exception testError)
        {
            try
            {
                var reqString = "{\"status\":\"error\", \"reason\":\"\"}";

                var requestData = Encoding.UTF8.GetBytes(reqString);
                var myUri = new Uri($"https://www.browserstack.com/automate/sessions/{FindSessionId()}.json");
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myWebRequest.ContentType = "application/json";
                myWebRequest.Method = "PUT";
                myWebRequest.ContentLength = requestData.Length;
                using (var st = myWebRequest.GetRequestStream())
                {
                    st.Write(requestData, 0, requestData.Length);
                }

                var myNetworkCredential = new NetworkCredential(_settings.BrowserStackUser, _settings.BrowserStackKey);
                var myCredentialCache = new CredentialCache();
                myCredentialCache.Add(myUri, "Basic", myNetworkCredential);
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Credentials = myCredentialCache;


                _retryService.RetryWeb(() => myWebRequest.GetResponse().Close(), x => _logger.Error(string.Empty, x));

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to connect to browser stack api", ex);
            }
        }

        public string FindSessionId()
        {
            var sessionId = (_driver as RemoteWebDriver)?.SessionId;

            return sessionId?.ToString();
        }
    }
}