using System;
using System.IO;
using System.Net;
using System.Text;

namespace LARSMetaDataToolBox.Web
{
    public class HttpClient : IHttpClient
    {
        public Stream GetFile(string url)
        {
            using (var client = new WebClient())
            {
                var content = client.DownloadData(url);
                return new MemoryStream(content);
            }
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
                catch (WebException ex)
                {
                    throw;
                }
            }
        }
    }
}
