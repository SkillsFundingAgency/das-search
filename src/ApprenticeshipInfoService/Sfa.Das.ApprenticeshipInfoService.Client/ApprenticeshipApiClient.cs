using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sfa.Das.ApprenticeshipInfoService.Client.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Client
{
    public class ApprenticeshipApiClient : IApprenticeshipApiClient
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public ApprenticeshipApiClient(string baseUri = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://my.api/v2/capture") };
        }

        /// <summary>
        /// Get a single standard details
        /// GET /standards/{standard-code}
        /// </summary>
        /// <param name="standardCode">An integer for the standard id (LARS code) ie: 12</param>
        /// <returns>a standard</returns>
        public Standard GetStandard(int standardCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/standards/{standardCode}");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Standard>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }

        /// <summary>
        /// Get a single framework details
        /// GET /frameworks/{framework-id}
        /// </summary>
        /// <param name="frameworkId">an integer for the composite id {frameworkId}{pathway}{progType}</param>
        /// <returns>a framework details based on pathway and level</returns>
        public Framework GetFramework(int frameworkId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/frameworks/{frameworkId}");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Framework>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }

        private static void RaiseResponseError(HttpRequestMessage failedRequest, HttpResponseMessage failedResponse)
        {
            throw new HttpRequestException(
                String.Format($"The {typeof(ApprenticeshipApiClient).Name.Replace("Client", string.Empty)} request for {0} {1} failed. Response Status: {2}, Response Body: {3}",
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
