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

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, List<int> selectedLevels)
        {
            var formattedKeywords = QueryHelper.FormatQuery(keywords);
            var searchDescriptor = GetKeywordSearchDescriptor(page, take, formattedKeywords, selectedLevels?.ToList());

            var results = _elasticsearchCustomClient.Search<ApprenticeshipSearchResultsItem>(s => searchDescriptor);

            var levelAggregation = new Dictionary<int, long?>();
            if (results.Aggs.Terms("level") != null)
            {
                foreach (var item in results.Aggs.Terms("level").Buckets)
                {
                    int iKey;
                    if (int.TryParse(item.Key, out iKey))
                    {
                        levelAggregation.Add(iKey, item.DocCount);
                    }
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
            var qryStr = CreateProviderQuery("standardCode", code.ToString(), geoPoint, deliveryModes);

            using (_profiler.CreateStep("Search for providers for standard"))
            {
                var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr);

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

                var trainingOptionsAggregation = new Dictionary<string, long?>();

                foreach (var item in results.Aggs.Terms("training_type").Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }

                return new SearchResult<StandardProviderSearchResultsItem> { Hits = documents, Total = results.Total, TrainingOptionsAggregation = trainingOptionsAggregation };
            }
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int code, Coordinate geoPoint, IEnumerable<string> deliveryModes)
        {
            using (_profiler.CreateStep("Search for providers for framework"))
            {
                var qryStr = CreateProviderQuery("frameworkId", code.ToString(), geoPoint, deliveryModes);

                var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr);

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

                var trainingOptionsAggregation = new Dictionary<string, long?>();

                foreach (var item in results.Aggs.Terms("training_type").Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }

                return new SearchResult<FrameworkProviderSearchResultsItem> { Hits = documents, Total = results.Total, TrainingOptionsAggregation = trainingOptionsAggregation };
            }
        }

        private SearchDescriptor<ApprenticeshipSearchResultsItem> GetKeywordSearchDescriptor(int page, int take, string formattedKeywords, List<int> selectedLevels)
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
                    .Aggregations(agg => agg.Terms("level", t => t.Field(f => f.Level).MinimumDocumentCount(0)));
        }

        private SearchDescriptor<StandardProviderSearchResultsItem> CreateProviderQuery(string apprenticeshipField, string code, Coordinate location, IEnumerable<string> deliveryModes)
        {
            var dm = deliveryModes == null || !deliveryModes.Any() ? "*" : string.Join(" ", deliveryModes);
            var boold =
                new BoolQueryDescriptor<StandardProviderSearchResultsItem>().Must(
                    must => must
                        .QueryString(qs => qs
                            .DefaultField(apprenticeshipField).Query(code)), null)
                    .Filter(f => f.GeoShapePoint(gp => gp.Coordinates(new GeoCoordinate(location.Lat, location.Lon))), null);

            var des =
                new SearchDescriptor<StandardProviderSearchResultsItem>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(
                        q => q
                        .ConstantScore(
                            cs => cs
                                .Filter(filter => filter
                                    .Bool(_ => boold))))
                     .Sort(ss => ss.GeoDistance(g => g.Field("locationPoint").PinTo(new GeoLocation(location.Lat, location.Lon)).Unit(DistanceUnit.Miles).Ascending()))
                     .Aggregations(aggs => aggs.Terms("training_type", tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0)))
                     .PostFilter(pf => pf
                       .Bool(b => b
                         .Must(m => m
                           .QueryString(qs => qs
                            .DefaultField("deliveryModes").Query(dm)))));

            return des;
        }
    }
}