namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Configuration;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Core.Domain.Services;
    using Sfa.Das.Sas.Infrastructure.Mapping;
    using SFA.DAS.Apprenticeships.Api.Client;
    using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
    using SFA.DAS.NLog.Logger;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public sealed class FrameworkApiRepository : IGetFrameworks
    {
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IFrameworkMapping _frameworkMapping;
        private readonly ILog _applicationLogger;
        private readonly IFrameworkApiClient _frameworkApiClient;

        public FrameworkApiRepository(IConfigurationSettings applicationSettings,
            IFrameworkMapping frameworkMapping,
            ILog applicationLogger,
            IFrameworkApiClient frameworkApiClient)
        {
            _applicationSettings = applicationSettings;
            _frameworkMapping = frameworkMapping;
            _applicationLogger = applicationLogger;
            _frameworkApiClient = frameworkApiClient;
        }

        public Framework GetFrameworkById(string id)
        {
            try
            {
                var result = _frameworkApiClient.Get(id);
                return _frameworkMapping.MapToFramework(result);
            }
            catch (EntityNotFoundException ex)
            {
                _applicationLogger.Info($"404 trying to get framework with id {id}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to get framework with id {id}", ex);
            }
        }

        public List<Framework> GetAllFrameworks()
        {
            throw new NotImplementedException();
        }

        public long GetFrameworksAmount()
        {
            throw new NotImplementedException();
        }

        public int GetFrameworksOffer()
        {
            throw new NotImplementedException();
        }

        public int GetFrameworksExpiringSoon(int daysToExpire)
        {
            throw new NotImplementedException();
        }
    }
}
