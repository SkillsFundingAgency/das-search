using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Infrastructure.Mapping;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    public sealed class FrameworkApiRepository : IGetFrameworks
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IFrameworkMapping _frameworkMapping;
        private readonly ILog _applicationLogger;
        private readonly IFrameworkApiClient _frameworkApiClient;
        private readonly IElasticsearchHelper _elasticsearchHelper;

        public FrameworkApiRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            IConfigurationSettings applicationSettings,
            IFrameworkMapping frameworkMapping,
            ILog applicationLogger,
            IFrameworkApiClient frameworkApiClient,
            IElasticsearchHelper elasticsearchHelper)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationSettings = applicationSettings;
            _frameworkMapping = frameworkMapping;
            _applicationLogger = applicationLogger;
            _frameworkApiClient = frameworkApiClient;
            _elasticsearchHelper = elasticsearchHelper;
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
                throw new ApplicationException($"Failed to get framework with id {id}", ex);
            }
        }

        public List<Framework> GetAllFrameworks()
        {
            return _elasticsearchHelper.GetAllDocumentsFromIndex<Framework>(_applicationSettings.ApprenticeshipIndexAlias, "frameworkdocument");
        }

        public long GetFrameworksAmount()
        {
            var results =
                   _elasticsearchCustomClient.Search<FrameworkSearchResultsItem>(
                       s =>
                       s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                           .Type(Types.Parse("frameworkdocument"))
                           .From(0)
                           .MatchAll());
            return results.HitsMetaData.Total;
        }

        public int GetFrameworksOffer()
        {
            var documents = _elasticsearchHelper.GetAllDocumentsFromIndex<FrameworkProviderSearchResultsItem>(_applicationSettings.ProviderIndexAlias, "frameworkprovider");
            var frameworkIdUkprnList = documents.Select(doc => string.Concat((object) doc.FrameworkId, doc.Ukprn));

            return frameworkIdUkprnList.Distinct().Count();
        }

        public int GetFrameworksExpiringSoon(int daysToExpire)
        {
            try
            {
                var take = (int)GetFrameworksAmount();
                var document =
                    _elasticsearchCustomClient.Search<FrameworkSearchResultsItem>(s => s
                        .Index(_applicationSettings.ApprenticeshipIndexAlias)
                        .Type("frameworkdocument")
                        .From(0)
                        .Take(take)
                        .Query(q => q
                            .Bool(b => b
                                .Filter(f => f
                                    .Exists(e => e
                                        .Field(field => field.ExpiryDate))))));

                var expiringElements = (from item in document.Documents where item.ExpiryDate != null let span = item.ExpiryDate.Value.Subtract(DateTime.Now.AddDays(1)) let daysDifference = (int)span.TotalDays where daysDifference <= daysToExpire select item).ToList();

                var response = expiringElements.GroupBy(x => x.FrameworkId).Count();
                return response;
            }
            catch (Exception ex)
            {
                _applicationLogger.Error(
                    ex,
                    $"Error retrieving amount of frameworks with provider");
                throw;
            }
        }
    }
}
