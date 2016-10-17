using System;
using System.Linq;
using Nest;
using Sfa.Das.ApprenticeshipInfoService.Core.Configuration;
using Sfa.Das.ApprenticeshipInfoService.Core.Logging;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;
using Sfa.Das.ApprenticeshipInfoService.Core.Services;
using Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Elasticsearch
{
    public sealed class ApprenticeshipProviderRepository : IApprenticeshipProviderRepository
    {
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IProviderMapping _providerMapping;

        public ApprenticeshipProviderRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IProviderMapping providerMapping)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _providerMapping = providerMapping;
        }

        public ApprenticeshipDetails GetCourseByStandardCode(int ukprn, int locationId, string standardCode)
        {
            try
            {
                var document =
                    GetProvider<StandardProviderSearchResultsItem>(
                        q =>
                        q.Term(t => t.Ukprn, ukprn)
                        && q.Term(t => t.StandardCode, standardCode)
                        && q.Nested(n => n
                            .Path(p => p.TrainingLocations)
                            .Query(nq => nq.Term(nt => nt.TrainingLocations.First().LocationId, locationId))));
                return document != null ? _providerMapping.MapToProvider(document, locationId) : null;
            }
            catch (Exception ex)
            {
                _applicationLogger.Error(
                    ex,
                    $"Trying to get standard with provider id {ukprn}, standard code {standardCode} and location id {locationId}");
                throw;
            }
        }

        public ApprenticeshipDetails GetCourseByFrameworkId(int ukprn, int locationId, string frameworkId)
        {
            try
            {
                var document =
                    GetProvider<FrameworkProviderSearchResultsItem>(
                        q =>
                        q.Term(t => t.Ukprn, ukprn)
                        && q.Term(t => t.FrameworkId, frameworkId)
                        && q.Nested(n => n
                            .Path(p => p.TrainingLocations)
                            .Query(nq => nq.Term(nt => nt.TrainingLocations.First().LocationId, locationId))));
                return document != null ? _providerMapping.MapToProvider(document, locationId) : null;
            }
            catch (Exception ex)
            {
                _applicationLogger.Error(
                    ex,
                    $"Trying to get standard with provider id {ukprn}, framework id {frameworkId} and location id {locationId}");
                throw;
            }
        }

        private T GetProvider<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query)
            where T : class
        {
            var results = _elasticsearchCustomClient.Search<T>(s => s.Index(_applicationSettings.ProviderIndexAlias).From(0).Size(1).Query(query));

            if (results.ApiCall.HttpStatusCode != 200)
            {
                throw new ApplicationException($"Failed query standard with provider");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            return document;
        }
    }
}
