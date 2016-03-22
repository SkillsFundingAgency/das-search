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

        public StandardSearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            keywords = QueryHelper.FormatQuery(keywords);

            var client = this._elasticsearchClientFactory.Create();
            var results = client.Search<StandardSearchResultsItem>(s => s
                .Skip(skip)
                .Take(take)
                .Query(q => q
                    .QueryString(qs => qs
                        .OnFields(f => f.Title, p => p.JobRoles, p => p.Keywords)
                        .Query(keywords)
                    ))
                );

            return new StandardSearchResults
            {
                TotalResults = results.Total,
                SearchTerm = keywords,
                Results = results.Documents,
                HasError = results.ConnectionStatus.HttpStatusCode != 200
            };
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByLocation(int code, Coordinate geoPoint)
        {
            var client = _elasticsearchClientFactory.Create();
            var qryStr = CreateStandardProviderRawQuery(code.ToString(), geoPoint);

            var results = client
                .Search<StandardProviderSearchResultsItem>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .From(0)
                .Size(1000)
                .QueryRaw(qryStr)
                .SortGeoDistance(g =>
                {
                    g.PinTo(geoPoint.Lat, geoPoint.Lon)
                        .Unit(GeoUnit.Miles).OnField("locationPoint").Ascending();
                    return g;
                }));

            var documents = results.Hits.Select(hit => new StandardProviderSearchResultsItem
            {
                Id = hit.Source.Id,
                UkPrn = hit.Source.UkPrn,
                Address = hit.Source.Address,
                ContactUsUrl = hit.Source.ContactUsUrl,
                DeliveryModes = hit.Source.DeliveryModes,
                Email = hit.Source.Email,
                EmployerSatisfaction = hit.Source.EmployerSatisfaction * 10,
                LearnerSatisfaction = hit.Source.LearnerSatisfaction * 10,
                LocationId = hit.Source.LocationId,
                LocationName = hit.Source.LocationName,
                MarketingName = hit.Source.MarketingName,
                Name = hit.Source.Name,
                Phone = hit.Source.Phone,
                StandardCode = hit.Source.StandardCode,
                StandardInfoUrl = hit.Source.StandardInfoUrl,
                Website = hit.Source.Website,
                Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0
            }).OrderByDescending(x => x.DeliveryModes?.Contains("100PercentEmployer")).ToList();

            if (results?.ConnectionStatus?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results?.ConnectionStatus?.HttpStatusCode}");
            }

            return new SearchResult<StandardProviderSearchResultsItem> { Hits = documents, Total = results.Total };
        }

        private string CreateStandardProviderRawQuery(string code, Coordinate location)
        {
            return string.Concat(
                @"{""filtered"": { ""query"": { ""match"": { ""standardCode"": """,
                code,
                @""" }}, ""filter"": { ""geo_shape"": { ""location"": { ""shape"": { ""type"": ""point"", ""coordinates"": [",
                location.Lon,
                ", ",
                location.Lat,
                "] }}}}}}");
        }

        private string CreateFrameworkProviderRawQuery(string code, Coordinate location)
        {
            return string.Concat(
                @"{""filtered"": { ""query"": { ""match"": { ""frameworkCode"": """,
                code,
                @""" }}, ""filter"": { ""geo_shape"": { ""location"": { ""shape"": { ""type"": ""point"", ""coordinates"": [",
                location.Lon,
                ", ",
                location.Lat,
                "] }}}}}}");
        }
    }
}