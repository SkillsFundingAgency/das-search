using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using SFA.DAS.Apprenticeships.Api.Client.Models;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public class FrameworkApiClient : ApiClientBase, IFrameworkApiClient
    {
        public FrameworkApiClient(string baseUri = null) : base(baseUri)
        {
        }

        /// <summary>
        /// Get a single framework details
        /// GET /frameworks/{framework-id}
        /// </summary>
        /// <param name="frameworkId">an integer for the composite id {frameworkId}{pathway}{progType}</param>
        /// <returns>a framework details based on pathway and level</returns>
        public Framework Get(int frameworkId)
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

        /// <summary>
        /// Get a collection of frameworks
        /// GET /frameworks
        /// </summary>
        /// <returns>a collection of framework summaries</returns>
        public IEnumerable<FrameworkSummary> FindAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/frameworks");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<FrameworkSummary>>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
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
