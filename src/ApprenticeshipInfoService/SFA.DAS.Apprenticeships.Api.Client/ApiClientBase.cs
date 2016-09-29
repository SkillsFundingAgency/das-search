using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public abstract class ApiClientBase
    {
        protected readonly HttpClient _httpClient;

        protected readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        protected ApiClientBase(string baseUri = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://api.apprenticeships.sfa.bis.gov.uk") };
        }

        protected static void RaiseResponseError(HttpRequestMessage failedRequest, HttpResponseMessage failedResponse)
        {
            throw new HttpRequestException(
                String.Format($"The {typeof(FrameworkApiClient).Name.Replace("Client", string.Empty)} request for {0} {1} failed. Response Status: {2}, Response Body: {3}",
                    failedRequest.Method.ToString().ToUpperInvariant(),
                    failedRequest.RequestUri,
                    (int)failedResponse.StatusCode,
                    failedResponse.Content.ReadAsStringAsync().Result));
        }
        public void Dispose()
        {
            Dispose(_httpClient);
        }

        public void Dispose(params IDisposable[] disposables)
        {
            foreach (var disposable in disposables.Where(d => d != null))
            {
                disposable.Dispose();
            }
        }
    }
}