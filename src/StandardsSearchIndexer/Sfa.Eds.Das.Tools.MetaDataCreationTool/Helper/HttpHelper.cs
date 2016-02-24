namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper
{
    using System;
    using System.Net;
    using System.Text;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    public static class HttpHelper
    {
        public static string DownloadString(string streamUrl, string username, string pwd)
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
    }
}
