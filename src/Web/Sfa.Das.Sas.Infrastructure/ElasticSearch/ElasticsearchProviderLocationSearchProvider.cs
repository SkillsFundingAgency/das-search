using System.Runtime.CompilerServices;

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
        private const string NationalProviderAggregateName = "national_provider";
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchProviderLocationSearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandard(int standardId, Coordinate coordinates, int page, int take, IEnumerable<string> deliveryModes)
        {
            var qryStr = CreateProviderQueryWithoutLocationLimit<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId.ToString(), coordinates, deliveryModes);

            var skip = CalculateSkip(page, take);

            var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToStandardProviderSearchResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<StandardProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandardAndNationalProvider(int standardId, Coordinate coordinates, int page, int take, IEnumerable<string> deliveryModes)
        {
            var qryStr = CreateProviderQueryWithNationalProviderWithoutLocationLimit<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId.ToString(), coordinates, deliveryModes);

            var skip = CalculateSkip(page, take);

            var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToStandardProviderSearchResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<StandardProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocation(int standardId, Coordinate coordinates, int page, int take, IEnumerable<string> deliveryModes)
        {
            var qryStr = CreateProviderQuery<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId.ToString(), coordinates, deliveryModes);

            var skip = CalculateSkip(page, take);

            var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToStandardProviderSearchResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<StandardProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocationAndNationalProvider(int standardId, Coordinate coordinates, int page, int take, IEnumerable<string> deliveryModes)
        {
            var qryStr = CreateProviderQueryWithNationalProvider<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId.ToString(), coordinates, deliveryModes);

            var skip = CalculateSkip(page, take);

            var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToStandardProviderSearchResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<StandardProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFramework(int frameworkId, Coordinate geoPoint, int page, int take, IEnumerable<string> deliveryModes)
        {
            var skip = CalculateSkip(page, take);

            var qryStr = CreateProviderQueryWithoutLocationLimit<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId.ToString(), geoPoint, deliveryModes);

            var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToFrameworkProviderSearhResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<FrameworkProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkAndNationalProvider(int frameworkId, Coordinate geoPoint, int page, int take, IEnumerable<string> deliveryModes)
        {
            var skip = CalculateSkip(page, take);

            var qryStr = CreateProviderQueryWithNationalProviderWithoutLocationLimit<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId.ToString(), geoPoint, deliveryModes);

            var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToFrameworkProviderSearhResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<FrameworkProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int frameworkId, Coordinate geoPoint, int page, int take, IEnumerable<string> deliveryModes)
        {
            var skip = CalculateSkip(page, take);

            var qryStr = CreateProviderQuery<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId.ToString(), geoPoint, deliveryModes);

            var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToFrameworkProviderSearhResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<FrameworkProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocationAndNationalProvider(int frameworkId, Coordinate geoPoint, int page, int take, IEnumerable<string> deliveryModes)
        {
            var skip = CalculateSkip(page, take);

            var qryStr = CreateProviderQueryWithNationalProvider<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId.ToString(), geoPoint, deliveryModes);

            var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToFrameworkProviderSearhResultsItem).ToList();

            var trainingOptionsAggregation = new Dictionary<string, long?>();

            if (results.Aggs.Terms(TrainingTypeAggregateName).Buckets != null)
            {
                foreach (var item in results.Aggs.Terms(TrainingTypeAggregateName).Buckets)
                {
                    trainingOptionsAggregation.Add(item.Key, item.DocCount);
                }
            }

            return new SearchResult<FrameworkProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation
            };
        }

        private static StandardProviderSearchResultsItem MapToStandardProviderSearchResultsItem(IHit<StandardProviderSearchResultsItem> hit)
        {
            return new StandardProviderSearchResultsItem
            {
                Ukprn = hit.Source.Ukprn,
                ContactUsUrl = hit.Source.ContactUsUrl,
                DeliveryModes = hit.Source.DeliveryModes,
                Email = hit.Source.Email,
                EmployerSatisfaction = hit.Source.EmployerSatisfaction * 10,
                LearnerSatisfaction = hit.Source.LearnerSatisfaction * 10,
                OverallAchievementRate = hit.Source.OverallAchievementRate,
                ApprenticeshipMarketingInfo = hit.Source.ApprenticeshipMarketingInfo,
                ProviderName = hit.Source.ProviderName,
                Phone = hit.Source.Phone,
                StandardCode = hit.Source.StandardCode,
                ApprenticeshipInfoUrl = hit.Source.ApprenticeshipInfoUrl,
                Website = hit.Source.Website,
                Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0,
                TrainingLocations = hit.Source.TrainingLocations,
                MatchingLocationId = hit.InnerHits.First().Value.Hits.Hits.First().Source.As<TrainingLocation>().LocationId,
                NationalProvider = hit.Source.NationalProvider
            };
        }

        private static FrameworkProviderSearchResultsItem MapToFrameworkProviderSearhResultsItem(IHit<FrameworkProviderSearchResultsItem> hit)
        {
            return new FrameworkProviderSearchResultsItem
            {
                Ukprn = hit.Source.Ukprn,
                ContactUsUrl = hit.Source.ContactUsUrl,
                DeliveryModes = hit.Source.DeliveryModes,
                Email = hit.Source.Email,
                EmployerSatisfaction = hit.Source.EmployerSatisfaction * 10,
                LearnerSatisfaction = hit.Source.LearnerSatisfaction * 10,
                OverallAchievementRate = hit.Source.OverallAchievementRate,
                ApprenticeshipMarketingInfo = hit.Source.ApprenticeshipMarketingInfo,
                ProviderName = hit.Source.ProviderName,
                Phone = hit.Source.Phone,
                FrameworkId = hit.Source.FrameworkId,
                FrameworkCode = hit.Source.FrameworkCode,
                PathwayCode = hit.Source.PathwayCode,
                ApprenticeshipInfoUrl = hit.Source.ApprenticeshipInfoUrl,
                Level = hit.Source.Level,
                Website = hit.Source.Website,
                Distance = hit.Sorts != null ? Math.Round(double.Parse(hit.Sorts.DefaultIfEmpty(0).First().ToString()), 1) : 0,
                TrainingLocations = hit.Source.TrainingLocations,
                MatchingLocationId = hit?.InnerHits != null ? hit.InnerHits.First().Value.Hits.Hits.First().Source.As<TrainingLocation>().LocationId : (int?)null,
                NationalProvider = hit.Source.NationalProvider
            };
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> FilterByLocation<T>(Coordinate location)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return f => f.GeoShapePoint(gp => gp.Field(fd => fd.TrainingLocations.First().Location).Coordinates(location.Lon, location.Lat));
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> FilterByApprenticeshipId<T>(Expression<Func<T, object>> selector, string apprenticeshipIdentifier)
            where T : class
        {
            return f => f.Term(t => t.Field(selector).Value(apprenticeshipIdentifier));
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> FilterByNationalProvider<T>(Expression<Func<T, object>> selector)
            where T : class
        {
            return f => f.Term(t => t.Field(selector).Value(true));
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

        private static Func<SortDescriptor<T>, IPromise<IList<ISort>>> NestedSortByDistanceFromGivenLocation<T>(Coordinate location)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return f => f.GeoDistance(g => g
                .Field(fd => fd.TrainingLocations.First().LocationPoint)
                .PinTo(new GeoLocation(location.Lat, location.Lon))
                .Unit(DistanceUnit.Miles)
                .Ascending());
        }

        private static Func<SortDescriptor<T>, IPromise<IList<ISort>>> SortByDistanceFromGivenLocation<T>(Coordinate location)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return f => f.GeoDistance(g => g
                .NestedPath(x => x.TrainingLocations)
                .Field(fd => fd.TrainingLocations.First().LocationPoint)
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
                    .Query(q => q
                        .Bool(ft => ft
                            .Filter(FilterByApprenticeshipId(selector, code))
                            .Must(NestedLocationsQuery<T>(location))))
                    .Sort(SortByDistanceFromGivenLocation<T>(location))
                    .Aggregations(aggs => aggs
                        .Terms(TrainingTypeAggregateName, tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0))
                        .Terms(NationalProviderAggregateName, tt => tt.Field(fi => fi.NationalProvider)))
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
                    .Query(q => q
                        .Bool(b => b
                            .Filter(FilterByApprenticeshipId(selector, code))
                            .Must(NestedLocationsQueryWithoutLocationMatch<T>(location))))
                    .Sort(SortByDistanceFromGivenLocation<T>(location))
                    .Aggregations(aggs => aggs.Terms(TrainingTypeAggregateName, tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0)))
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private SearchDescriptor<T> CreateProviderQueryWithNationalProvider<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor =
                new SearchDescriptor<T>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(q => q
                        .Bool(ft => ft
                            .Filter(FilterByApprenticeshipId(selector, code), FilterByNationalProvider<T>(x => x.NationalProvider))
                            .Must(NestedLocationsQuery<T>(location))))
                    .Sort(SortByDistanceFromGivenLocation<T>(location))
                    .Aggregations(aggs => aggs
                        .Terms(TrainingTypeAggregateName, tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0))
                        .Terms(NationalProviderAggregateName, tt => tt.Field(fi => fi.NationalProvider)))
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private SearchDescriptor<T> CreateProviderQueryWithNationalProviderWithoutLocationLimit<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor =
                new SearchDescriptor<T>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(q => q
                        .Bool(b => b
                            .Filter(FilterByApprenticeshipId(selector, code), FilterByNationalProvider<T>(x => x.NationalProvider))
                            .Must(NestedLocationsQueryWithoutLocationMatch<T>(location))))
                    .Sort(SortByDistanceFromGivenLocation<T>(location))
                    .Aggregations(aggs => aggs.Terms(TrainingTypeAggregateName, tt => tt.Field(fi => fi.DeliveryModes).MinimumDocumentCount(0)))
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private Func<QueryContainerDescriptor<T>, QueryContainer> NestedLocationsQuery<T>(Coordinate location)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return f => f.Nested(n => n
                .InnerHits(h => h
                    .Size(1)
                    .Sort(NestedSortByDistanceFromGivenLocation<T>(location)))
                .Path(p => p.TrainingLocations)
                .Query(q => q
                    .Bool(b => b
                        .Filter(FilterByLocation<T>(location)))));
        }

        private Func<QueryContainerDescriptor<T>, QueryContainer> NestedLocationsQueryWithoutLocationMatch<T>(Coordinate location)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return f => f.Nested(n => n
                .InnerHits(h => h
                    .Size(1)
                    .Sort(NestedSortByDistanceFromGivenLocation<T>(location)))
                .Path(p => p.TrainingLocations)
                .Query(q => q.MatchAll()));
        }

        private int CalculateSkip(int page, int take)
        {
            var skip = (page - 1) * take;
            return skip < 0 ? 0 : skip;
        }
    }
}