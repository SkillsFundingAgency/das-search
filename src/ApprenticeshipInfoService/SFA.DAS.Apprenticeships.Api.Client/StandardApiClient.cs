using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using SFA.DAS.Apprenticeships.Api.Client.Models;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public class StandardApiClient : ApiClientBase, IStandardApiClient
    {
        public StandardApiClient(string baseUri = null) : base(baseUri)
        {
        }

        public Standard Get(int standardCode)
        {
            return Get(standardCode.ToString());
        }

        public Standard Get(string standardCode)
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

        public bool Exists(int standardCode)
        {
            return Exists(standardCode.ToString());
        }

        public bool Exists(string standardCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, $"/standards/{standardCode}");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return false;
        }

        /// <summary>
        /// Get a collection of standards
        /// GET /standards
        /// </summary>
        /// <returns>a collection of standard summaries</returns>
        public IEnumerable<StandardSummary> FindAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/standards");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<StandardSummary>>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }
    }
}