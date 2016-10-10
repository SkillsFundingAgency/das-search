namespace SFA.DAS.Apprenticeships.Api.Client
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public class ProviderApiClient : ApiClientBase, IProviderApiClient
    {
        public ProviderApiClient(string baseUri = null) : base(baseUri)
        {
        }

        /// <summary>
        /// Get a list of provider details
        /// GET /providers/{provider-ukprn}
        /// </summary>
        /// <param name="providerUkprn">an integer for the provider ukprn</param>
        /// <returns>a framework details based on pathway and level</returns>
        public IEnumerable<Provider> Get(int providerUkprn)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/providers/{providerUkprn}");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<Provider>>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
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