namespace Sfa.Infrastructure.Services
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public class HttpService : IHttpGet, IHttpGetFile, IHttpPost
    {
        private readonly ILog logger;

        public HttpService(ILog logger)
        {
            this.logger = logger;
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

                    return client.DownloadString(url);
                }
                catch (WebException exception)
                {
                    logger.Warn($"Cannot download string from {url} - Error: {exception.Message}", exception);
                    throw;
                }

                return string.Empty;
            }
        }

        public Stream GetFile(string url)
        {
            using (var client = new WebClient())
            {
                var content = client.DownloadData(url);
                return new MemoryStream(content);
            }
        }

        public void Post(string url, string body, string user, string password)
        {
            using (var client = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
                client.PostAsync(url, content).Wait();
            }
        }
    }
}