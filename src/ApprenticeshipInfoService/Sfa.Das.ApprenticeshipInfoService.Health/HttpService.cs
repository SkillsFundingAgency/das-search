namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public class HttpService : IHttpServer
    {
        public Status ResponseCode(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url);
                Task.WaitAll(response);
                return response.Result.StatusCode.ToString().ToLower() == "ok"
                    ? Status.Green : Status.Red;
            }
        }

        public Status GetStatus(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = false;
            request.Method = "HEAD";
            try
            {
                using (request.GetResponse())
                {
                    return Status.Green;
                }
            }
            catch (WebException)
            {
                return Status.Red;
            }
        }
    }
}
