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
    using System.Collections.Generic;

    public sealed class ElasticsearchProvider : ISearchProvider
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;

        private readonly IProfileAStep _profiler;

        public ElasticsearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings, IProfileAStep profiler)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
            _profiler = profiler;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, List<string> selectedLevels)
        {
            var formattedKeywords = QueryHelper.FormatQuery(keywords);
            var searchDescriptor = GetKeywordSearchDescriptor(page, take, formattedKeywords, selectedLevels?.ToList());

            var results = _elasticsearchCustomClient.Search<ApprenticeshipSearchResultsItem>(s => searchDescriptor);

            Dictionary<string, long?> levelAggregation = new Dictionary<string, long?>();
            if (results.Aggs.Terms("level") != null)
            {
                foreach (var item in results.Aggs.Terms("level").Buckets)
                {
                    levelAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new ApprenticeshipSearchResults
            {
                TotalResults = results.HitsMetaData?.Total ?? 0,
                ResultsToTake = take,
                SearchTerm = formattedKeywords,
                Results = results.Documents,
                HasError = results.ApiCall.HttpStatusCode != 200,
                LevelAggregation = levelAggregation,
                SelectedLevels = selectedLevels
            };
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocation(int code, Coordinate geoPoint, IEnumerable<string> deliveryModes)
        {
            var qryStr = CreateStandardProviderRawQuery(code.ToString(), geoPoint, deliveryModes);

            using (_profiler.CreateStep("Search for providers for standard"))
            {
                var results =
                    _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(
                        s =>
                        s.Index(_applicationSettings.ProviderIndexAlias)
                            .From(0)
                            .Size(1000)
                            .Query(q => q.Raw(qryStr))
                            .Sort(ss => ss.GeoDistance(g => g.Field("locationPoint").PinTo(new GeoLocation(geoPoint.Lat, geoPoint.Lon)).Unit(DistanceUnit.Miles).Ascending())));

                var documents =
                    results.Hits.Select(
                        hit =>
                        new StandardProviderSearchResultsItem
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

                var qryStrAggregation = this.CreateStandardProviderRawQuery(code.ToString(), geoPoint, new List<string>());
                var trainingOptionsAggregation = GetTraingOptionsAggregation<StandardProviderSearchResultsItem>(documents.Any(), qryStrAggregation);

                return new SearchResult<StandardProviderSearchResultsItem> { Hits = documents, Total = results.Total, TrainingOptionsAggregation = trainingOptionsAggregation };
            }
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int code, Coordinate geoPoint, IEnumerable<string> deliveryModes)
        {
            using (_profiler.CreateStep("Search for providers for framework")) { 
            var qryStr = CreateFrameworkProviderRawQuery(code.ToString(), geoPoint, deliveryModes);

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

            var qryStrAggregation = CreateFrameworkProviderRawQuery(code.ToString(), geoPoint, new List<string>());
            var trainingOptionsAggregation = GetTraingOptionsAggregation<FrameworkProviderSearchResultsItem>(documents.Any(), qryStrAggregation);

            return new SearchResult<FrameworkProviderSearchResultsItem> { Hits = documents, Total = results.Total, TrainingOptionsAggregation = trainingOptionsAggregation };
            }
        }

        private SearchDescriptor<ApprenticeshipSearchResultsItem> GetKeywordSearchDescriptor(int page, int take, string formattedKeywords, List<string> selectedLevels)
        {
            var levelQuery = selectedLevels != null && selectedLevels.Any() ? string.Join(" ", selectedLevels) : "*";

            var skip = (page - 1) * take;
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
                            .Query(formattedKeywords)))
                     .PostFilter(m => m
                        .Bool(b => b
                            .Must(mu => mu
                                .QueryString( qs => qs
                                    .DefaultField(df => df.Level)
                                    .Query(levelQuery)))))
                    .Aggregations(agg => agg.Terms("level", t => t.Field(f => f.Level)));
        }

        private string CreateStandardProviderRawQuery(string code, Coordinate location, IEnumerable<string> deliveryModes)
        {
            return CreateFullQuery("standardCode", code, location, deliveryModes);
        }

        private string CreateFrameworkProviderRawQuery(string code, Coordinate location, IEnumerable<string> deliveryModes)
        {
            return CreateFullQuery("frameworkId", code, location, deliveryModes);
        }

        private Dictionary<string, long?> GetTraingOptionsAggregation<T>(bool documents, string qryStrAggregation)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var trainingOptionsAggregation = new Dictionary<string, long?>();
            if (documents)
            {
                var resultsAggregation =
                    _elasticsearchCustomClient.Search<T>(
                        s => s.Index(_applicationSettings.ProviderIndexAlias)
                        .Size(0)
                        .Query(q => q
                            .Raw(qryStrAggregation))
                       .Aggregations(aggs => aggs.Terms("training_type", tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0))));

                foreach (var item in resultsAggregation.Aggs.Terms("training_type").Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return trainingOptionsAggregation;
        }

        private string CreateFullQuery(string specificPart, string code, Coordinate location, IEnumerable<string> deliveryModes)
        {
            var dm = deliveryModes == null || !deliveryModes.Any() ? "*" : string.Join(" ", deliveryModes);

            return ToJson(new
            {
                constant_score = new
                {
                    filter = new
                    {
                        bool_field = new
                        {
                            must = new[]
                                           {
                                            new
                                            {
                                                    query_string = new { default_field = "deliveryModes", query = dm, default_operator = "or" }
                                            },
                                            new
                                                {
                                                    query_string = new { default_field = specificPart, query = code, default_operator = "and" }
                                                }
                                           },
                            filter = new
                            {
                                geo_shape = new
                                {
                                    location = new
                                    {
                                        shape = new
                                        {
                                            type = "point",
                                            coordinates = new[] { location.Lon, location.Lat }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }).Replace("bool_field", "bool");
        }

        private static readonly Func<dynamic, string> ToJson = d => Newtonsoft.Json.JsonConvert.SerializeObject(d);

        //private Dictionary<string, long?> GetTraingOptionsAggregation<T>(bool documents, string qryStrAggregation)
        //   where T : ApprenticeshipSearchResultsItem
        //{
        //    var trainingOptionsAggregation = new Dictionary<string, long?>();
        //    if (documents)
        //    {
        //        var resultsAggregation =
        //            _elasticsearchCustomClient.Search<T>(
        //                s => s.Index(_applicationSettings.ProviderIndexAlias)
        //                .Size(0)
        //                .Query(q => q
        //                    .Raw(qryStrAggregation))
        //               .Aggregations(aggs => aggs.Terms("level", tt => tt.Field(fi => fi.Level))));

        //        foreach (var item in resultsAggregation.Aggs.Terms("level").Buckets)
        //        {
        //            trainingOptionsAggregation.Add(item.Key, item.DocCount);
        //        }
        //    }

        //    return trainingOptionsAggregation;
        //}
    }
}