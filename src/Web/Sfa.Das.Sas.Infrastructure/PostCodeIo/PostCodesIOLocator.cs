using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Exceptions;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Infrastructure.PostCodeIo
{
    public class PostCodesIOLocator : ILookupLocations
    {
        private readonly IRetryWebRequests _retryService;
        private readonly ILog _logger;

        private readonly IProfileAStep _profiler;

        public PostCodesIOLocator(IRetryWebRequests retryService, ILog logger, IProfileAStep profiler)
        {
            _retryService = retryService;
            _logger = logger;
            _profiler = profiler;
        }

        public async Task<Coordinate> GetLatLongFromPostCode(string postcode)
        {
            var coordinates = new Coordinate();
            var sUrl = "http://api.postcodes.io/postcodes/" + postcode.Replace(" ", string.Empty);

            try
            {
                using (_profiler.CreateStep("Postcode.IO lookup"))
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
            }
            catch (Exception ex)
            {
                throw new SearchException("Unable to connect to Post Code Lookup service", ex);
            }
        }

        private void CouldntConnect(Exception ex)
        {
            _logger.Warn(string.Concat("Couldn't connect to postcode service, retrying...", ex.Message));
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
