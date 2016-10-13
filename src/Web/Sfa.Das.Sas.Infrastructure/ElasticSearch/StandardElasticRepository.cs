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
    public sealed class StandardElasticRepository : IGetStandards
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IStandardMapping _standardMapping;

        public StandardElasticRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IStandardMapping standardMapping)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _standardMapping = standardMapping;
        }

        public Standard GetStandardById(string id)
        {
            var results =
                _elasticsearchCustomClient.Search<StandardSearchResultsItem>(
                    s =>
                    s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                        .Type(Types.Parse("standarddocument"))
                        .From(0)
                        .Size(1)
                        .Query(q => q.QueryString(qs => qs.Fields(fs => fs.Field(e => e.StandardId)).Query(id.ToString()))));

            if (results.ApiCall.HttpStatusCode != 200)
            {
                throw new ApplicationException($"Failed query standard with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            return document != null ? _standardMapping.MapToStandard(document) : null;
        }

        // TODO: Review this for performance againt using filters instead
        public IEnumerable<Standard> GetStandardsByIds(IEnumerable<int> ids)
        {
            var standardIds = ids as IList<int> ?? ids.ToList();

            if (!standardIds.Any())
            {
                return new List<Standard>();
            }

            var queryString = standardIds.Select(x => x.ToString()).Aggregate((x1, x2) => x1 + " OR " + x2);

            var results =
                   _elasticsearchCustomClient.Search<StandardSearchResultsItem>(
                       s =>
                       s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                           .Type(Types.Parse("standarddocument"))
                           .From(0)
                           .Size(standardIds.Count)
                           .Query(q => q.QueryString(qs => qs.Fields(fs => fs.Field(e => e.StandardId)).Query(queryString))));

            if (!results.Documents.Any())
            {
                return new List<Standard>();
            }

            return results.Documents.Select(x => _standardMapping.MapToStandard(x))
                                    .Where(p => p != null);
        }

        public long GetStandardsAmount()
        {
            var results =
                   _elasticsearchCustomClient.Search<StandardSearchResultsItem>(
                       s =>
                       s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                           .Type(Types.Parse("standarddocument"))
                           .From(0)
                           .MatchAll());
            return results.HitsMetaData.Total;
        }
    }
}
