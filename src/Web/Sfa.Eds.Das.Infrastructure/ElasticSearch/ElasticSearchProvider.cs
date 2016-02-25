using System;
using System.Threading.Tasks;
using Nest;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Eds.Das.Infrastructure.Location;

namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System.Linq;
    using Core.Configuration;

    public sealed class ElasticsearchProvider : ISearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILog _logger;
        private readonly IStandardRepository _standardRepository;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly ILocator _locatorService;

        public ElasticsearchProvider(IElasticsearchClientFactory elasticsearchClientFactory, ILog logger, IStandardRepository standardRepository, IConfigurationSettings applicationSettings, ILocator locatorService)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
            _standardRepository = standardRepository;
            _applicationSettings = applicationSettings;
            _locatorService = locatorService;
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

        public async Task<ProviderSearchResults> SearchByLocation(string standardId, int skip, int take, string location = null)
        {
            if (string.IsNullOrEmpty(location))
            {
                return new ProviderSearchResults
                {
                    StandardId = int.Parse(standardId),
                    PostCodeMissing = true
                };
            }

            var client = _elasticsearchClientFactory.Create();

            var standardName = string.Empty;

            var standard = _standardRepository.GetById(standardId);

            if (standard != null)
            {
                standardName = standard.Title;
            }

            ISearchResponse<ProviderSearchResultsItem> results = new SearchResponse<ProviderSearchResultsItem>();

            var coordinates = await _locatorService.GetLatLongFromPostCode(location);
            if (coordinates.Lat != 0 && coordinates.Lon != 0)
            {
                var qryStr = CreateRawQuery(standardId, coordinates);

                results = client
                    .Search<ProviderSearchResultsItem>(s => s
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .QueryRaw(qryStr)
                    .SortGeoDistance(g =>
                    {
                        g.PinTo(coordinates.Lat, coordinates.Lon)
                            .Unit(GeoUnit.Miles).OnField("locationPoint").Ascending();
                        return g;
                    }));
            }

            var documents = results.Hits.Select(hit => new ProviderSearchResultsItem
            {
                ProviderName = hit.Source.ProviderName, PostCode = hit.Source.PostCode, UkPrn = hit.Source.UkPrn, VenueName = hit.Source.VenueName, StandardsId = hit.Source.StandardsId, Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0,
            }).ToList();

            var result = new ProviderSearchResults
            {
                TotalResults = results.Total,
                StandardId = int.Parse(standardId),
                StandardName = standardName,
                PostCode = location,
                Results = documents
            };

            if (results.ConnectionStatus != null)
            {
                result.HasError = results.ConnectionStatus.HttpStatusCode != 200;
            }
            else
            {
                result.HasError = false;
            }

            return result;
        }

        private string CreateRawQuery(string standardId, Coordinate location)
        {
            return string.Concat(
                @"{""filtered"": { ""query"": { ""match"": { ""standardsId"": """,
                standardId,
                @""" }}, ""filter"": { ""geo_shape"": { ""location"": { ""shape"": { ""type"": ""point"", ""coordinates"": [",
                location.Lon,
                ", ",
                location.Lat,
                "] }}}}}}");
        }
    }
}