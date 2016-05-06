using System;
using System.Linq;
using Nest;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Exceptions;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public sealed class ElasticsearchProvider : ISearchProvider
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take)
        {
            var formattedKeywords = QueryHelper.FormatQuery(keywords);
            var searchDescriptor = GetKeywordSearchDescriptor(page, take, formattedKeywords);

            var results = _elasticsearchCustomClient.Search<ApprenticeshipSearchResultsItem>(s => searchDescriptor);

            return new ApprenticeshipSearchResults
            {
                TotalResults = results.HitsMetaData.Total,
                ResultsToTake = take,
                SearchTerm = formattedKeywords,
                Results = results.Documents,
                HasError = results.ApiCall.HttpStatusCode != 200
            };
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocation(int code, Coordinate geoPoint)
        {
            var qryStr = CreateStandardProviderRawQuery(code.ToString(), geoPoint);

            var results = _elasticsearchCustomClient
                .Search<StandardProviderSearchResultsItem>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .From(0)
                .Size(1000)
                .Query(q => q
                    .Raw(qryStr))
                .Sort(ss => ss
                    .GeoDistance(g => g
                        .Field("locationPoint")
                        .PinTo(new GeoLocation(geoPoint.Lat, geoPoint.Lon))
                        .Unit(DistanceUnit.Miles)
                        .Ascending())));

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
                ApprenticeshipMarketingInfo = hit.Source.ApprenticeshipMarketingInfo,
                Name = hit.Source.Name,
                Phone = hit.Source.Phone,
                StandardCode = hit.Source.StandardCode,
                ApprenticeshipInfoUrl = hit.Source.ApprenticeshipInfoUrl,
                Website = hit.Source.Website,
                Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0
            }).ToList();

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            return new SearchResult<StandardProviderSearchResultsItem> { Hits = documents, Total = results.Total };
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int code, Coordinate geoPoint)
        {
            var qryStr = CreateFrameworkProviderRawQuery(code.ToString(), geoPoint);

            var results = _elasticsearchCustomClient
                .Search<FrameworkProviderSearchResultsItem>(s => s
                .Index(_applicationSettings.ProviderIndexAlias)
                .From(0)
                .Size(1000)
                .Query(q => q
                    .Raw(qryStr))
                .Sort(ss => ss
                    .GeoDistance(g => g
                        .Field("locationPoint")
                        .PinTo(new GeoLocation(geoPoint.Lat, geoPoint.Lon))
                        .Unit(DistanceUnit.Miles)
                        .Ascending())));

            var documents = results.Hits.Select(hit => new FrameworkProviderSearchResultsItem
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
                ApprenticeshipMarketingInfo = hit.Source.ApprenticeshipMarketingInfo,
                Name = hit.Source.Name,
                Phone = hit.Source.Phone,
                FrameworkId = hit.Source.FrameworkId,
                FrameworkCode = hit.Source.FrameworkCode,
                PathwayCode = hit.Source.PathwayCode,
                ApprenticeshipInfoUrl = hit.Source.ApprenticeshipInfoUrl,
                Level = hit.Source.Level,
                Website = hit.Source.Website,
                Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0
            }).ToList();

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            return new SearchResult<FrameworkProviderSearchResultsItem> { Hits = documents, Total = results.Total };
        }

        private SearchDescriptor<ApprenticeshipSearchResultsItem> GetKeywordSearchDescriptor(int page, int take, string formattedKeywords)
        {
            var skip = page * take;
            return new SearchDescriptor<ApprenticeshipSearchResultsItem>()
                    .Index(_applicationSettings.ApprenticeshipIndexAlias)
                    .Type(Types.Parse("standarddocument,frameworkdocument"))
                    .Skip(skip)
                    .Take(take)
                    .Query(q => q
                        .QueryString(qs => qs
                            .Fields(fs => fs
                                .Field(f => f.Title)
                                .Field(p => p.JobRoles)
                                .Field(p => p.Keywords)
                                .Field(p => p.FrameworkName)
                                .Field(p => p.PathwayName))
                            .Query(formattedKeywords)));
        }

        private string CreateStandardProviderRawQuery(string code, Coordinate location)
        {
            return CreateFullQuery("standardCode", code, location);
        }

        private string CreateFrameworkProviderRawQuery(string code, Coordinate location)
        {
            return CreateFullQuery("frameworkId", code, location);
        }

        private string CreateFullQuery(string specificPart, string code, Coordinate location)
        {
            return string.Concat(
                @"{""filtered"": { ""query"": { ""match"": { """,
                specificPart,
                @""": """,
                code,
                @""" }}, ""filter"": { ""geo_shape"": { ""location"": { ""shape"": { ""type"": ""point"", ""coordinates"": [",
                location.Lon,
                ", ",
                location.Lat,
                "] }}}}}}");
        }
    }
}