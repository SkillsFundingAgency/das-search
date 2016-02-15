using Nest;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;

namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System.Linq;
    using Sfa.Eds.Das.Core.Configuration;

    public sealed class ElasticsearchProvider : ISearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILog _logger;
        private readonly IStandardRepository _standardRepository;
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchProvider(IElasticsearchClientFactory elasticsearchClientFactory, ILog logger, IStandardRepository standardRepository, IConfigurationSettings applicationSettings)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
            _standardRepository = standardRepository;
            _applicationSettings = applicationSettings;
        }

        public StandardSearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            keywords = QueryHelper.FormatQuery(keywords);

            var client = this._elasticsearchClientFactory.Create();
            var results = client.Search<StandardSearchResultsItem>(s => s.Skip(skip).Take(take).QueryString(keywords));

            return new StandardSearchResults
            {
                TotalResults = results.Total,
                SearchTerm = keywords,
                Results = results.Documents,
                HasError = results.ConnectionStatus.HttpStatusCode != 200
            };
        }

        public ProviderSearchResults SearchByLatLon(string standardId, int skip, int take, string postcode = null)
        {
            var client = this._elasticsearchClientFactory.Create();

            Nest.ISearchResponse<ProviderSearchResultsItem> results;

            if (string.IsNullOrEmpty(postcode))
            {
                results = client
                    .Search<ProviderSearchResultsItem>(s => s
                        .Index(_applicationSettings.ProviderIndexAlias)
                        .MatchAll()
                        .Filter(f => f
                            .Term(y => y.StandardsId, standardId)));
                if (results.ConnectionStatus.HttpStatusCode != 200)
                {
                    _logger.Error("1. Something failed");
                    _logger.Error("2. " + _applicationSettings.ProviderIndexAlias);
                    _logger.Error("3. " + results.RequestInformation.RequestUrl);
                    _logger.Error("4. " + results.RequestInformation);
                }
            }
            else
            {
                var qryStr = CreateRawQuery(standardId, postcode);

                results = client
                    .Search<ProviderSearchResultsItem>(s => s
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .QueryRaw(qryStr));
                if (results.ConnectionStatus.HttpStatusCode != 200)
                {
                    _logger.Error("1. Something failed");
                    _logger.Error("2. " + _applicationSettings.ProviderIndexAlias);
                    _logger.Error("3. " + qryStr);
                    _logger.Error("4. " + results.RequestInformation.RequestUrl);
                    _logger.Error("5. " + results.RequestInformation);
                }
            }

            var documents = results.Documents.Where(i => !string.IsNullOrEmpty(i.UkPrn)).OrderBy(x => x.ProviderName);

            string standardName = string.Empty;

            var standard = _standardRepository.GetById(standardId);

            if (standard != null)
            {
                standardName = standard.Title;
            }

            return new ProviderSearchResults
            {
                TotalResults = results.Total,
                StandardId = int.Parse(standardId),
                StandardName = standardName,
                Results = documents,
                HasError = results.ConnectionStatus.HttpStatusCode != 200
            };
        }

        private string CreateRawQuery(string standardId, string postcode)
        {
            // TODO: transform postcode to latlon
            var lat = 52.4006274;
            var lon = -1.5104302;
            return string.Concat(@"{""filtered"": { ""query"": { ""match"": { ""standardsId"": """, standardId, @""" }}, ""filter"": { ""geo_shape"": { ""location"": { ""shape"": { ""type"": ""point"", ""coordinates"": [", lon, ", ", lat, "] }}}}}}");
        }
    }
}