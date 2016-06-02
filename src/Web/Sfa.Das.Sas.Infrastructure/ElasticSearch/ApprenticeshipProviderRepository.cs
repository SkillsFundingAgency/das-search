using System;
using System.Linq;
using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using Sfa.Das.Sas.ApplicationServices;

    public sealed class ApprenticeshipProviderRepository : IApprenticeshipProviderRepository
    {
        private readonly ILog _applicationLogger;

        private readonly IConfigurationSettings _applicationSettings;

        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;

        private readonly IProviderMapping _providerMapping;

        private readonly IProfileAStep _profiler;

        public ApprenticeshipProviderRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IProviderMapping providerMapping,
            IProfileAStep profiler)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _providerMapping = providerMapping;
            _profiler = profiler;
        }

        public ApprenticeshipDetails GetCourseByStandardCode(string providerid, string locationId, string standardCode)
        {
            try
            {
                var document =
                    GetProvider<StandardProviderSearchResultsItem>(
                        q =>
                        q.Term(t => t.Id, providerid) && q.Term(t => t.LocationId, locationId)
                        && q.Term(t => t.StandardCode, standardCode));
                return _providerMapping.MapToProvider(document);
            }
            catch (Exception ex)
            {
                _applicationLogger.Error(
                    ex,
                    $"Trying to get standard with provider id {providerid}, standard code {standardCode} and location id {locationId}");
                throw;
            }
        }

        public ApprenticeshipDetails GetCourseByFrameworkId(string providerid, string locationId, string frameworkId)
        {
            try
            {
                var document =
                    GetProvider<FrameworkProviderSearchResultsItem>(
                        q =>
                        q.Term(t => t.Id, providerid) && q.Term(t => t.LocationId, locationId)
                        && q.Term(t => t.FrameworkId, frameworkId));
                return _providerMapping.MapToProvider(document);
            }
            catch (Exception ex)
            {
                _applicationLogger.Error(
                    ex,
                    $"Trying to get standard with provider id {providerid}, framework id {frameworkId} and location id {locationId}");
                throw;
            }
        }

        private T GetProvider<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query)
            where T : class
        {
            using (_profiler.CreateStep("Get Provider"))
            {
                var results = _elasticsearchCustomClient.Search<T>(s => s.Index(_applicationSettings.ProviderIndexAlias).From(0).Size(1).Query(query));

                if (results.ApiCall.HttpStatusCode != 200)
                {
                    throw new ApplicationException($"Failed query standard with provider");
                }

                var document = results.Documents.Any() ? results.Documents.First() : null;

                if (document == null)
                {
                    return null;
                }

                return document;
            }
        }
    }
}