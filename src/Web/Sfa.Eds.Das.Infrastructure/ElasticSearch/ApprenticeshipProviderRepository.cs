namespace Sfa.Eds.Das.Infrastructure.Elasticsearch
{
    using System;
    using System.Linq;

    using Nest;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Configuration;
    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Core.Domain.Services;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Infrastructure.Mapping;

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

        public Provider GetByStandardCode(string providerid, string locationId, string standardCode)
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

        public Provider GetByFrameworkId(string providerid, string locationId, string frameworkId)
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
            var results =
                _elasticsearchCustomClient.Search<T>(
                    s => s.Index(_applicationSettings.ProviderIndexAlias).From(0).Size(1).Query(query));

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