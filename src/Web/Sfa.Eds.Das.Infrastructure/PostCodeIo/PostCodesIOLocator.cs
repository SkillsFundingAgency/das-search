using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.ApplicationServices.Exceptions;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;
using Sfa.Eds.Das.Core.Logging;

namespace Sfa.Eds.Das.Infrastructure.PostCodeIo
{
    using Sfa.Das.ApplicationServices;

    public class PostCodesIOLocator : ILookupLocations
    {
        private readonly IRetryWebRequests _retryService;
        private readonly ILog _logger;

        public PostCodesIOLocator(IRetryWebRequests retryService, ILog logger)
        {
            _retryService = retryService;
            _logger = logger;
        }

        public async Task<Coordinate> GetLatLongFromPostCode(string postcode)
        {
            var coordinates = new Coordinate();
            var sUrl = "http://api.postcodes.io/postcodes/" + postcode.Replace(" ", string.Empty);

            try
            {
                    var response = await _retryService.RetryWeb(() => MakeRequestAsync(sUrl), CouldntConnect);

                    if (response.IsSuccessStatusCode)
                    {
                        var value = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<PostCodeResponse>(value);
                        coordinates.Lat = result.Result.Latitude;
                        coordinates.Lon = result.Result.Longitude;

                        return coordinates;
                    }

                    return null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to connect to Post Code Lookup service: {sUrl}");

                throw new SearchException("Unable to connect to Post Code Lookup service", ex);
            }
        }

        private void CouldntConnect(Exception ex)
        {
            _logger.Warn("Couldn't connect to postcode service, retrying...");
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(string url)
        {
            using (var client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    return await client.SendAsync(request);
                }
            }
        }
    }
}
