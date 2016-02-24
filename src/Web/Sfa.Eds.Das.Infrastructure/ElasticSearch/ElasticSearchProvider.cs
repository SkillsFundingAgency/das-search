using System;
using System.Collections.Generic;
using Nest;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;
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

        public ProviderSearchResults SearchByLocation(string standardId, int skip, int take, string location = null)
        {
            var client = this._elasticsearchClientFactory.Create();

            Nest.ISearchResponse<ProviderSearchResultsItem> results;

            if (string.IsNullOrEmpty(location))
            {
                results = client
                    .Search<ProviderSearchResultsItem>(s => s
                        .Index(_applicationSettings.ProviderIndexAlias)
                        .MatchAll()
                        .Filter(f => f
                            .Term(y => y.StandardsId, standardId)));
            }
            else
            {
                var qryStr = CreateRawQuery(standardId, location);

                var coordinate = new Coordinate();
                if (IsLatLong(location))
                {
                    var coordinates = location.Split(',');
                    coordinate.Lat = double.Parse(coordinates[0]);
                    coordinate.Lon= double.Parse(coordinates[1]);
                }

                results = client
                    .Search<ProviderSearchResultsItem>(s => s
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .QueryRaw(qryStr)
                    .SortGeoDistance(g =>
                    {
                        g.PinTo(coordinate.Lat, coordinate.Lon)
                            .Unit(GeoUnit.Miles).OnField("locationPoint").Ascending();
                        return g;
                    }));
            }

            var documents = results.Hits.Select(hit => new ProviderSearchResultsItem
            {
                ProviderName = hit.Source.ProviderName, PostCode = hit.Source.PostCode, UkPrn = hit.Source.UkPrn, VenueName = hit.Source.VenueName, StandardsId = hit.Source.StandardsId, Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0,
            }).ToList();
            
            var standardName = string.Empty;

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

        private string CreateRawQuery(string standardId, string location)
        {
            var lat = string.Empty;
            var lon = string.Empty;
            if (IsLatLong(location))
            {
                var coordinates = location.Split(',');
                lat = coordinates[0];
                lon = coordinates[1];
            }
            else if ((location.Length >= 5) && (location.Length <= 7))
            {
                // It is full postcode
                // TODO: transform postcode to latlon
                var coordinates = location.Split(',');
                lat = coordinates[0];
                lon = coordinates[1];
            }

            return string.Concat(
                @"{""filtered"": { ""query"": { ""match"": { ""standardsId"": """,
                standardId,
                @""" }}, ""filter"": { ""geo_shape"": { ""location"": { ""shape"": { ""type"": ""point"", ""coordinates"": [",
                lon,
                ", ",
                lat,
                "] }}}}}}");
        }

        private bool IsLatLong(string location)
        {
            var index = location.IndexOf(',');

            if (index == -1) return false;

            var coordinates = location.Split(',');
            var lat = coordinates[0].Trim();
            var lon = coordinates[1].Trim();
            double latDouble;
            double lonDouble;

            return double.TryParse(lat, out latDouble) && double.TryParse(lon, out lonDouble);
        }
    }
}