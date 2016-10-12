using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public class FrameworkApiClient : ApiClientBase, IFrameworkApiClient
    {
        public FrameworkApiClient(string baseUri = null) : base(baseUri)
        {
        }

        public Framework Get(string frameworkId)
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

        public Framework Get(int frameworkCode, int pathwayCode, int progamType)
        {
            return Get(ConvertToCompositeId(frameworkCode, pathwayCode, progamType));
        }

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

        public bool Exists(string frameworkId)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, $"/frameworks/{frameworkId}");
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

        public bool Exists(int frameworkCode, int pathwayCode, int progamType)
        {
            return Exists(ConvertToCompositeId(frameworkCode, pathwayCode, progamType));
        }

        private static string ConvertToCompositeId(int frameworkCode, int pathwayCode, int progamType)
        {
            return $"{frameworkCode}{pathwayCode}{progamType}";
        }
    }
}
