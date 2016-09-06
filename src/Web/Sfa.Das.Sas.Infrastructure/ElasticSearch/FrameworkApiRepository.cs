using System;
using System.Linq;
using Nest;
using Newtonsoft.Json;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using Sfa.Das.Sas.ApplicationServices;

    public sealed class FrameworkApiRepository : IGetFrameworks
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IFrameworkMapping _frameworkMapping;
        private readonly IHttpGet _httpService;

        public FrameworkApiRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IFrameworkMapping frameworkMapping,
            IHttpGet httpService)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _frameworkMapping = frameworkMapping;
            _httpService = httpService;
        }

        public Framework GetFrameworkById(int id)
        {
            var url = string.Concat(_applicationSettings.ApprenticeshipApiBaseUrl, "Framework/", id);

            var result = JsonConvert.DeserializeObject<FrameworkSearchResultsItem>(_httpService.Get(url, null, null));

            if (result == null)
            {
                throw new ApplicationException($"Failed to get framework with id {id}");
            }

            return _frameworkMapping.MapToFramework(result);
        }
    }
}
