namespace CircuitBreaker.Core
{
    using System;
    using System.Net;

    public class HttpClient
    {
        public string Get(string url)
        {
            Console.WriteLine("GET " + url);
            var wc = new WebClient();
            return wc.DownloadString(url);
        }
    }
}