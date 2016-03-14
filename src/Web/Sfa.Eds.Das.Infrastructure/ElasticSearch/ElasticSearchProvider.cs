using System;
using System.Linq;

using Nest;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;
using Sfa.Eds.Das.Core.Logging;

namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using Core.Configuration;
    using Sfa.Das.ApplicationServices.Exceptions;
    public sealed class ElasticsearchProvider : ISearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILog _logger;

        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchProvider(IElasticsearchClientFactory elasticsearchClientFactory, ILog logger, IConfigurationSettings applicationSettings)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            keywords = QueryHelper.FormatQuery(keywords);

            var client = this._elasticsearchClientFactory.Create();
            var results = client.Search<ApprenticeshipSearchResultsItem>(s => s
                .Types("standarddocument", "frameworkdocument")
                .Skip(skip)
                .Take(take)
                .Query(q => q
                    .QueryString(qs => qs
                        .OnFields(f => f.Title, p => p.JobRoles, p => p.Keywords, p => p.FrameworkTitle)
                        .Query(keywords)
                    ))
                );

            return new ApprenticeshipSearchResults
            {
                TotalResults = results.Total,
                SearchTerm = keywords,
                Results = results.Documents,
                HasError = results.ConnectionStatus.HttpStatusCode != 200
            };
        }

        public SearchResult<ProviderSearchResultsItem> SearchByLocation(int standardId, Coordinate geoPoint)
        {
            var client = _elasticsearchClientFactory.Create();

            var qryStr = CreateRawQuery(standardId.ToString(), geoPoint);

            ISearchResponse<ProviderSearchResultsItem> results = client
                .Search<ProviderSearchResultsItem>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .QueryRaw(qryStr)
                .SortGeoDistance(g =>
                {
                    g.PinTo(geoPoint.Lat, geoPoint.Lon)
                        .Unit(GeoUnit.Miles).OnField("locationPoint").Ascending();
                    return g;
                }));

            var documents = results.Hits.Select(hit => new ProviderSearchResultsItem
            {
                Id = hit.Source.Id,
                Name = hit.Source.Name,
                PostCode = hit.Source.PostCode,
                UkPrn = hit.Source.UkPrn,
                LocationName = hit.Source.LocationName,
                Standardscode = hit.Source.Standardscode,
                Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0
            }).ToList();

            if (results?.ConnectionStatus?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results?.ConnectionStatus?.HttpStatusCode}");
            }

            return new SearchResult<ProviderSearchResultsItem> { Hits = documents, Total = results.Total };
        }

        private string CreateRawQuery(string standardId, Coordinate location)
        {
            return string.Concat(
                @"{""filtered"": { ""query"": { ""match"": { ""standardCode"": """,
                standardId,
                @""" }}, ""filter"": { ""geo_shape"": { ""location"": { ""shape"": { ""type"": ""point"", ""coordinates"": [",
                location.Lon,
                ", ",
                location.Lat,
                "] }}}}}}");
        }
    }
}