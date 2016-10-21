using System;
using System.Collections.Generic;
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
    public sealed class FrameworkElasticRepository : IGetFrameworks
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IFrameworkMapping _frameworkMapping;

        public FrameworkElasticRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IFrameworkMapping frameworkMapping)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _frameworkMapping = frameworkMapping;
        }

        public Framework GetFrameworkById(string id)
        {
            var results =
                _elasticsearchCustomClient.Search<FrameworkSearchResultsItem>(
                    s =>
                    s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                        .Type(Types.Parse("frameworkdocument"))
                        .From(0)
                        .Size(1)
                        .Query(q => q.QueryString(qs => qs.Fields(fs => fs.Field(e => e.FrameworkId)).Query(id.ToString()))));

            if (results.ApiCall.HttpStatusCode != 200)
            {
                throw new ApplicationException($"Failed query provider with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            return document != null ? _frameworkMapping.MapToFramework(document) : null;
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

                var tmp = document.Documents.GroupBy(x => x.FrameworkId).Count();

                var expiringElements = (from item in document.Documents where item.ExpiryDate != null let span = item.ExpiryDate.Value.Subtract(DateTime.Now) let daysDifference = (int) span.TotalDays where daysDifference <= daysToExpire select item).ToList();

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
