namespace Sfa.Das.ApprenticeshipInfoService.Api.Helpers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public static class HttpResponseFactory
    {
        public static HttpResponseException RaiseException(HttpStatusCode statusCode, string reason)
        {
            return new HttpResponseException(new HttpResponseMessage(statusCode)
            {
                ReasonPhrase = reason,
                Content = new StringContent(reason)
            });
        }
    }
}