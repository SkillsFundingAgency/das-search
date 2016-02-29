namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    public class HttpHelper : IHttpHelper
    {
        public string DownloadString(string streamUrl, string username, string pwd)
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
                    return client.DownloadString(streamUrl);
                }
                catch (WebException exception)
                {
                    Log4NetLogger logger = new Log4NetLogger();
                    logger.Warn($"Can download string from {streamUrl} - Error: {exception.Message}");
                }

                return string.Empty;
            }
        }

        public async Task<Stream> DownloadStream(string streamUrl, string username, string pwd)
        {
            using (var client = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{pwd}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                using (HttpResponseMessage response = await client.GetAsync(streamUrl))
                {
                    
                }
                    return client.GetStreamAsync(streamUrl).Result;
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

        public string DownloadFile(string downloadFileUrl, string workingfolder)
        {
            if (string.IsNullOrEmpty(downloadFileUrl))
            {
                return string.Empty;
            }

            FileHelper.EnsureDir(workingfolder);
            var zipFile = Path.Combine(workingfolder, Path.GetRandomFileName());
            using (var client = new WebClient())
            {
                client.DownloadFile(downloadFileUrl, zipFile);
            }

            Console.WriteLine($"File downloaded [{zipFile}]");
            return zipFile;
        }
    }
}
