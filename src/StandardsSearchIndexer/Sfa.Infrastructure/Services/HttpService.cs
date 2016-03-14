namespace Sfa.Infrastructure.Services
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Eds.Das.Indexer.Core.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;

    public class HttpService : IHttpGet, IHttpGetFile, IHttpPost
    {
        private readonly ILog logger;

        public HttpService(ILog logger)
        {
            this.logger = logger;
        }

        public string Get(string url, string username, string pwd)
        {
            using (WebClient client = new WebClient())
            {
                if (!string.IsNullOrEmpty(username))
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{pwd}"));
                    client.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
                }

                try
                {
                    return client.DownloadString(url);
                }
                catch (WebException exception)
                {
                    logger.Warn($"Can download string from {url} - Error: {exception.Message}", exception);
                }

                return string.Empty;
            }
        }

        public void Post(string url, string body, string user, string password)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
                client.PostAsync(url, content).Wait();
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
    }
}
