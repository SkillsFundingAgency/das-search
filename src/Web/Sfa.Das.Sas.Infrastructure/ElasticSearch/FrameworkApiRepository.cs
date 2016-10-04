using System;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Infrastructure.Mapping;
using SFA.DAS.Apprenticeships.Api.Client;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public sealed class FrameworkApiRepository : IGetFrameworks
    {
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IFrameworkMapping _frameworkMapping;
        private readonly IHttpGet _httpService;
        private readonly IFrameworkApiClient _frameworkApiClient;

        public FrameworkApiRepository(IConfigurationSettings applicationSettings,
            IFrameworkMapping frameworkMapping,
            IHttpGet httpService,
            IFrameworkApiClient frameworkApiClient)
        {
            _applicationSettings = applicationSettings;
            _frameworkMapping = frameworkMapping;
            _httpService = httpService;
            _frameworkApiClient = frameworkApiClient;
        }

        public Framework GetFrameworkById(int id)
        {
            var result = _frameworkApiClient.Get(id);

            if (result == null)
            {
                throw new ApplicationException($"Failed to get framework with id {id}");
            }

            return _frameworkMapping.MapToFramework(result);
        }
    }
}
