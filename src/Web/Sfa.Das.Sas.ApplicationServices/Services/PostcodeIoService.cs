using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core;
using Sfa.Das.Sas.Core.Configuration;

using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.ApplicationServices.Services
{
    public class PostcodeIoService : IPostcodeIoService
    {
        private readonly IRetryWebRequests _retryService;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;

        public PostcodeIoService(IRetryWebRequests retryService, ILog logger, IConfigurationSettings applicationSettings)
        {
            _retryService = retryService;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public async Task<string> GetPostcodeCountry(string postcode)
        {
            var uri = new Uri(_applicationSettings.PostcodeUrl, postcode.Replace(" ", string.Empty));

            try
            {
                var response = await _retryService.RetryWeb(() => MakeRequestAsync(uri.ToString()), CouldntConnect);

                if (response.IsSuccessStatusCode)
                {
                    var value = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PostCodeResponse>(value);
                    return result.Result.Country;
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(string.Concat("Couldn't connect to postcode service", ex.Message));
            }

            return "Error";
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

        private void CouldntConnect(Exception ex)
        {
            _logger.Warn(string.Concat("Couldn't connect to postcode service, retrying...", ex.Message));
        }
    }
}
