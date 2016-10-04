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

        /// <summary>
        /// Get a single standard details
        /// GET /standards/{standard-code}
        /// </summary>
        /// <param name="standardCode">An integer for the standard id (LARS code) ie: 12</param>
        /// <returns>a standard</returns>
        public Standard Get(int standardCode)
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
    }
}