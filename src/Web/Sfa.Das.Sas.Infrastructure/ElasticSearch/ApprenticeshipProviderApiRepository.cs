using System;
using System.Linq;
using Nest;
using Newtonsoft.Json;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public sealed class ApprenticeshipProviderApiRepository : IApprenticeshipProviderRepository
    {
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IProviderMapping _providerMapping;
        private readonly IHttpGet _httpService;

        public ApprenticeshipProviderApiRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IProviderMapping providerMapping,
            IHttpGet httpService)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _providerMapping = providerMapping;
            _httpService = httpService;
        }

        public ApprenticeshipDetails GetCourseByStandardCode(int ukprn, int locationId, string standardCode)
        {
            var url = string.Format(
                "{0}standards/{1}/providers?ukprn={2}&location={3}",
                _applicationSettings.ApprenticeshipApiBaseUrl,
                standardCode,
                ukprn,
                locationId);

            var result = JsonConvert.DeserializeObject<ApprenticeshipDetails>(_httpService.Get(url, null, null));

            if (result == null)
            {
                throw new ApplicationException($"Failed to get framework with id {standardCode}");
            }

            return result;
        }

        public ApprenticeshipDetails GetCourseByFrameworkId(int ukprn, int locationId, string frameworkId)
        {
            var url = string.Format(
                "{0}frameworks/{1}/providers?ukprn={2}&location={3}",
                _applicationSettings.ApprenticeshipApiBaseUrl,
                frameworkId,
                ukprn,
                locationId);

            var result = JsonConvert.DeserializeObject<ApprenticeshipDetails>(_httpService.Get(url, null, null));

            if (result == null)
            {
                throw new ApplicationException($"Failed to get framework with id {frameworkId}");
            }

            return result;
        }
    }
}