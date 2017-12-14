
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public sealed class StandardApiRepository : IGetStandards
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IStandardMapping _standardMapping;
        private readonly IStandardApiClient _standardApiClient;
        private readonly IElasticsearchHelper _elasticsearchHelper;
        private readonly ILog _applicationLogger;

        public StandardApiRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            IConfigurationSettings applicationSettings,
            IStandardMapping standardMapping,
            IStandardApiClient standardApiClient,
            IElasticsearchHelper elasticsearchHelper,
            ILog applicationLogger)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationSettings = applicationSettings;
            _standardMapping = standardMapping;
            _standardApiClient = standardApiClient;
            _elasticsearchHelper = elasticsearchHelper;
            _applicationLogger = applicationLogger;
        }

        public Standard GetStandardById(string id)
        {
            try
            {
                var result = _standardApiClient.Get(id);
                return _standardMapping.MapToStandard(result);
            }
            catch (EntityNotFoundException ex)
            {
                _applicationLogger.Info($"404 trying to get standard with id {id}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to get standard with id {id}", ex);
            }
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
        
        public IEnumerable<Standard> GetAllStandards()
        {
            var results = _elasticsearchHelper.GetAllDocumentsFromIndex<StandardSearchResultsItem>(_applicationSettings.ApprenticeshipIndexAlias, "standarddocument");
            return results.Select(s => _standardMapping.MapToStandard(s));
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

        public long GetStandardsOffer()
        {
            var documents = _elasticsearchHelper.GetAllDocumentsFromIndex<StandardProviderSearchResultsItem>(_applicationSettings.ProviderIndexAlias, "standardprovider");
            var standardUkprnList = documents.Select(doc => string.Concat(doc.StandardCode, doc.Ukprn));

            return standardUkprnList.Distinct().Count();
        }
    }
}