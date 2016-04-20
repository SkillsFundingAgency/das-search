namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    using Sfa.Das.WebTest.Infrastructure.Settings;

    public class BrowserStackApi : IBrowserStackApi
    {
        private readonly IBrowserSettings _settings;

        private readonly IWebDriver _driver;

        public BrowserStackApi(IBrowserSettings settings, IWebDriver driver)
        {
            _settings = settings;
            _driver = driver;
        }

        public void FailTestSession(string reason)
        {
            try
            {
                var reqString = "{\"status\":\"error\", \"reason\":\"" + reason + "\"}";

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

                myWebRequest.GetResponse().Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToNiceString());
            }
        }

        public void FailTestSession(Exception testError)
        {
            FailTestSession(testError.Message);
        }

        public string FindSessionId()
        {
            var sessionId = (_driver as RemoteWebDriver)?.SessionId;
            //var sessionIdProperty = typeof(RemoteWebDriver).GetProperty("SessionId", BindingFlags.Instance | BindingFlags.NonPublic);
            //var sessionId = sessionIdProperty.GetValue(_driver, null) as SessionId;
            Console.WriteLine("##### Session: " + sessionId);

            return sessionId?.ToString();
        }
    }
}