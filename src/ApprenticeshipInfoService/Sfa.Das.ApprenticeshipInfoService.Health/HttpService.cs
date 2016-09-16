namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;

    public class HttpService : IHttpServer
    {
        public string ResponseCode(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url);
                Task.WaitAll(response);
                return response.Result.StatusCode.ToString();
            }
        }

        public string Ping(string url)
        {
            var ping = new Ping();
            try
            {
                var response = ping.Send(url);
                return response?.Status.ToString();
            }
            catch (Exception exception)
            {
                return "Error";
            }
        }

        public string GetData(string url)
        {
            using (var client = new WebClient())
            {
                client.OpenRead(url);
                return client.ResponseHeaders.GetValues("Status Code")?.FirstOrDefault();
                //return client.DownloadString(url);
            }
        }
    }

    public interface IHttpServer
    {
        string ResponseCode(string url);

        string Ping(string url);

        string GetData(string url);
    }
}
