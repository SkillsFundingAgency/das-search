namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Nest;
    using Sfa.Das.Sas.ApplicationServices;
    using Sfa.Das.Sas.ApplicationServices.Exceptions;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Configuration;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Core.Logging;

    public sealed class ElasticsearchProviderLocationSearchProvider : IProviderLocationSearchProvider
    {
        private const string TrainingTypeAggregateName = "training_type";
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;

        private readonly IProfileAStep _profiler;

        public ElasticsearchProviderLocationSearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings, IProfileAStep profiler)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
            _profiler = profiler;
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandard(int code, Coordinate coordinates, int page, int take, IEnumerable<string> deliveryModes)
        {
            var qryStr = CreateProviderQueryWithoutLocationLimit<StandardProviderSearchResultsItem>(x => x.StandardCode, code.ToString(), coordinates, deliveryModes);

            using (_profiler.CreateStep("Search for providers for standard"))
            {
                var skip = (page - 1) * take;
                var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

                if (results.ApiCall?.HttpStatusCode != 200)
                {
                    throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
                }

                var documents = results.Hits.Select(MapToStandardProviderSearchResultsItem).ToList();

                var trainingOptionsAggregation = new Dictionary<string, long?>();

                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }

                return new SearchResult<StandardProviderSearchResultsItem> { Hits = documents, Total = results.Total, TrainingOptionsAggregation = trainingOptionsAggregation };
            }
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocation(int code, Coordinate coordinates, int page, int take, IEnumerable<string> deliveryModes)
        {
            var qryStr = CreateProviderQuery<StandardProviderSearchResultsItem>(x => x.StandardCode, code.ToString(), coordinates, deliveryModes);

            using (_profiler.CreateStep("Search for providers for standard"))
            {
                var skip = (page - 1) * take;
                var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

                if (results.ApiCall?.HttpStatusCode != 200)
                {
                    throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
                }

                var documents = results.Hits.Select(hit => MapToStandardProviderSearchResultsItem(hit)).ToList();

                var trainingOptionsAggregation = new Dictionary<string, long?>();

                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }

                return new SearchResult<StandardProviderSearchResultsItem> { Hits = documents, Total = results.Total, TrainingOptionsAggregation = trainingOptionsAggregation };
            }
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int code, Coordinate geoPoint, int page, int take, IEnumerable<string> deliveryModes)
        {
            using (_profiler.CreateStep("Search for providers for framework"))
            {
                var skip = (page - 1) * take;

                var qryStr = CreateProviderQuery<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, code.ToString(), geoPoint, deliveryModes);

                var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

                if (results.ApiCall?.HttpStatusCode != 200)
                {
                    throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
                }

                var documents = results.Hits.Select(hit => MapToFrameworkProviderSearhResultsItem(hit)).ToList();

                var trainingOptionsAggregation = new Dictionary<string, long?>();

                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }

                return new SearchResult<FrameworkProviderSearchResultsItem> { Hits = documents, Total = results.Total, TrainingOptionsAggregation = trainingOptionsAggregation };
            }
        }

        private static StandardProviderSearchResultsItem MapToStandardProviderSearchResultsItem(IHit<StandardProviderSearchResultsItem> hit)
        {
            return new StandardProviderSearchResultsItem
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
            };
        }

        private static FrameworkProviderSearchResultsItem MapToFrameworkProviderSearhResultsItem(IHit<FrameworkProviderSearchResultsItem> hit)
        {
            return new FrameworkProviderSearchResultsItem
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
            };
        }

        private static QueryContainer QueryForProvidersOfApprenticeshipCoveringLocation<T>(Expression<Func<T, object>> selector, string apprenticeshipIdentifier, Coordinate location, QueryContainerDescriptor<T> q)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return q.Bool(b => b.Filter(FilterByApprenticeshipId(selector, apprenticeshipIdentifier), FilterByLocation<T>(location)));
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> FilterByLocation<T>(Coordinate location)
            where T : class
        {
            return f => f.GeoShapePoint(gp => gp.Coordinates(location.Lon, location.Lat));
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> FilterByApprenticeshipId<T>(Expression<Func<T, object>> selector, string apprenticeshipIdentifier)
            where T : class
        {
            return f => f.Term(t => t.Field(selector).Value(apprenticeshipIdentifier));
        }

        private static QueryContainer FilterByDeliveryModes<T>(QueryContainerDescriptor<T> descriptor, IEnumerable<string> deliveryModes)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            if (deliveryModes == null || !deliveryModes.Any())
            {
                return descriptor;
            }

            return descriptor
                    .Terms(t => t
                        .Field(x => x.DeliveryModes)
                        .Terms(deliveryModes));
        }

        private static SortDescriptor<T> SortByDistanceFromGivenLocation<T>(Coordinate location, SortDescriptor<T> descriptor)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return descriptor.GeoDistance(g => g
                .Field("locationPoint")
                .PinTo(new GeoLocation(location.Lat, location.Lon))
                .Unit(DistanceUnit.Miles)
                .Ascending());
        }

        private SearchDescriptor<T> CreateProviderQuery<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor =
                new SearchDescriptor<T>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(q => QueryForProvidersOfApprenticeshipCoveringLocation(selector, code, location, q))
                    .Sort(ss => SortByDistanceFromGivenLocation(location, ss))
                    .Aggregations(aggs => aggs.Terms(TrainingTypeAggregateName, tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0)))
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private SearchDescriptor<T> CreateProviderQueryWithoutLocationLimit<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor =
                new SearchDescriptor<T>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(q => q.Bool(b => b.Filter(FilterByApprenticeshipId(selector, code))))
                    .Sort(ss => SortByDistanceFromGivenLocation(location, ss))
                    .Aggregations(aggs => aggs.Terms(TrainingTypeAggregateName, tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0)))
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }
    }
}