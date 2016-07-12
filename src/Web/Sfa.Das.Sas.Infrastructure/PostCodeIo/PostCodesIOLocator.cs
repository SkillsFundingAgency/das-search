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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using Core.Configuration;
    using Logging;
    public class PostCodesIoLocator : ILookupLocations
    {
        private readonly IRetryWebRequests _retryService;
        private readonly ILog _logger;

        private readonly IConfigurationSettings _applicationSettings;

        public PostCodesIoLocator(IRetryWebRequests retryService, ILog logger, IConfigurationSettings applicationSettings)
        {
            _retryService = retryService;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public async Task<Coordinate> GetLatLongFromPostCode(string postcode)
        {
            var coordinates = new Coordinate();
            var uri = new Uri(_applicationSettings.PostcodeUrl, postcode.Replace(" ", string.Empty));

            try
            {
                var stopwatch = Stopwatch.StartNew();
                var response = await _retryService.RetryWeb(() => MakeRequestAsync(uri.ToString()), CouldntConnect);
                stopwatch.Stop();
                var responseTime = stopwatch.ElapsedMilliseconds;

                if (response.IsSuccessStatusCode)
                {
                    var value = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PostCodeResponse>(value);
                    coordinates.Lat = result.Result.Latitude;
                    coordinates.Lon = result.Result.Longitude;

                    SendDependencyLog(response.StatusCode, uri, responseTime);

                    return coordinates;
                }

                var dir = new Dictionary<string, object>
                                  {
                                      { "Identifier ", "Postcodes.IO-PostCodeNotFound" },
                                      { "Postcode", postcode },
                                      { "Url", uri.ToString() }
                                  };

                _logger.Info($"Unable to find coordinates for postcode: {postcode}. Service url:{uri}", dir);
                SendDependencyLog(response.StatusCode, uri, responseTime);

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unable to connect to Postcode lookup servce. Url: {uri}");

                throw new SearchException("Unable to connect to Post Code Lookup service", ex);
            }
        }

        private void SendDependencyLog(HttpStatusCode statusCode, Uri uri, long responseTime)
        {
            var logEntry = new DependencyLogEntry
            {
                Identifier = "PostcodeIo Postcode Search",
                ResponseCode = (int)statusCode,
                ResponseTime = responseTime,
                Url = uri.ToString()
            };

            _logger.Debug("Dependency PostCodeIo", logEntry);
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
