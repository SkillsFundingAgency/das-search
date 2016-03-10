using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.ApplicationServices.Exceptions;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.ApplicationServices;
using Sfa.Eds.Das.Core.Domain.Model;
using Sfa.Eds.Das.Core.Logging;

namespace Sfa.Eds.Das.Infrastructure.PostCodeIo
{
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
            var sURL = "http://api.postcodes.io/postcodes/" + postcode.Replace(" ", string.Empty);

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await _retryService.RetryWeb(
                        () => { return MakeRequestAsync(sURL); },
                        (ex) => _logger.Warn("Couldn't connect to postcode service, retrying..."));

                    if (response.IsSuccessStatusCode)
                    {
                        var value = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<PostCodeResponse>(value);
                        coordinates.Lat = result.Result.Latitude;
                        coordinates.Lon = result.Result.Longitude;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to connect to Post Code Lookup service: {sURL}");

                throw new SearchException("Unable to connect to Post Code Lookup service", ex);
            }

            return coordinates;
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(string url)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(
                    HttpMethod.Get,
                    url);

                HttpResponseMessage response = await client.SendAsync(request);

                return response;
            }
        }
    }
}
