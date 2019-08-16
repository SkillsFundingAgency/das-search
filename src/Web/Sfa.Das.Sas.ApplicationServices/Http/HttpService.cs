using System;
using System.Net;
using System.Text;

namespace Sfa.Das.Sas.ApplicationServices.Http
{
    using Logging;
    using Microsoft.Extensions.Logging;

    public class HttpService : IHttpGet
    {
        private readonly ILogger<HttpService> _logger;

        public HttpService(ILogger<HttpService> logger)
        {
            this._logger = logger;
        }

        public string Get(string url, string username, string pwd)
        {
            using (var client = new WebClient())
            {
                if (!string.IsNullOrEmpty(username))
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{pwd}"));
                    client.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
                }

                try
                {
                    client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                    client.Encoding = Encoding.UTF8;
                    return client.DownloadString(url);
                }
                catch (WebException ex)
                {
                    _logger.LogWarning(ex.ToString()+"{httpError}", new HttpErrorLogEntry { Url = url });

                    HttpWebResponse res = (HttpWebResponse)ex.Response;
                    if (res.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }

                    throw;
                }
            }
        }
    }
}
