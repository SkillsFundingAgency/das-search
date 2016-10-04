using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Client;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using System;
    using System.Linq;

    using Nest;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Configuration;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Core.Domain.Services;
    using Sfa.Das.Sas.Infrastructure.Mapping;

    public sealed class StandardApiRepository : IGetStandards
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IStandardMapping _standardMapping;
        private readonly IStandardApiClient _standardApiClient;

        public StandardApiRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            IConfigurationSettings applicationSettings,
            IStandardMapping standardMapping,
            IStandardApiClient standardApiClient)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationSettings = applicationSettings;
            _standardMapping = standardMapping;
            _standardApiClient = standardApiClient;
        }

        public Standard GetStandardById(int id)
        {
            var result = _standardApiClient.Get(id);

            if (result == null)
            {
                throw new ApplicationException($"Failed to get standard with id {id}");
            }

            return _standardMapping.MapToStandard(result);
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
    }
}