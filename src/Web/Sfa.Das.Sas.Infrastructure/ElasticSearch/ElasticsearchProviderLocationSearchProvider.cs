namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using ApplicationServices;
    using ApplicationServices.Exceptions;
    using ApplicationServices.Models;
    using Core.Configuration;
    using Core.Domain.Model;
    using FeatureToggle.Core.Fluent;
    using FeatureToggles;
    using Nest;
    using SFA.DAS.NLog.Logger;

    public sealed class ElasticsearchProviderLocationSearchProvider : IProviderLocationSearchProvider
    {
        private const string TrainingTypeAggregateName = "training_type";
        private const string NationalProviderAggregateName = "national_provider";
        private const string BlockRelease = "BlockRelease";
        private const string DayRelease = "DayRelease";
        private const string HundredPercentEmployer = "100PercentEmployer";

        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchProviderLocationSearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public SearchResult<StandardProviderSearchResultsItem> SearchStandardProviders(string standardId, Coordinate coordinates, int page, int take, ProviderSearchFilter filter)
        {
            var qryStr = CreateStandardProviderSearchQuery(standardId, coordinates, filter);
            return PerformStandardProviderSearchWithQuery(page, take, qryStr);
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchFrameworkProviders(string frameworkId, Coordinate coordinates, int page, int take, ProviderSearchFilter filter)
        {
            var qryStr = CreateFrameworkProviderSearchQuery(frameworkId, coordinates, filter);
            return PerformFrameworkProviderSearchWithQuery(page, take, qryStr);
        }

        private static StandardProviderSearchResultsItem MapToStandardProviderSearchResultsItem(IHit<StandardProviderSearchResultsItem> hit)
        {
            return new StandardProviderSearchResultsItem
            {
                Ukprn = hit.Source.Ukprn,
                ContactUsUrl = hit.Source.ContactUsUrl,
                DeliveryModes = hit.Source.DeliveryModes,
                Email = hit.Source.Email,
                EmployerSatisfaction = hit.Source.EmployerSatisfaction,
                LearnerSatisfaction = hit.Source.LearnerSatisfaction,
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
                NationalProvider = hit.Source.NationalProvider,
                IsHigherEducationInstitute = hit.Source.IsHigherEducationInstitute,
                HasNonLevyContract = hit.Source.HasNonLevyContract,
                CurrentlyNotStartingNewApprentices = hit.Source.CurrentlyNotStartingNewApprentices
            };
        }

        private static FrameworkProviderSearchResultsItem MapToFrameworkProviderSearhResultsItem(IHit<FrameworkProviderSearchResultsItem> hit)
        {
            return new FrameworkProviderSearchResultsItem
            {
                Ukprn = hit.Source.Ukprn,
                IsHigherEducationInstitute = hit.Source.IsHigherEducationInstitute,
                ContactUsUrl = hit.Source.ContactUsUrl,
                DeliveryModes = hit.Source.DeliveryModes,
                Email = hit.Source.Email,
                EmployerSatisfaction = hit.Source.EmployerSatisfaction,
                LearnerSatisfaction = hit.Source.LearnerSatisfaction,
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
                NationalProvider = hit.Source.NationalProvider,
                HasNonLevyContract = hit.Source.HasNonLevyContract,
                CurrentlyNotStartingNewApprentices = hit.Source.CurrentlyNotStartingNewApprentices
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
                    .Field(x => x.DeliveryModesKeywords)
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

        private static Dictionary<string, long?> RetrieveAggregationElements(TermsAggregate<string> termsAggregate)
        {
            var aggregationResult = new Dictionary<string, long?>();

            if (termsAggregate.Buckets != null)
            {
                foreach (var item in termsAggregate.Buckets)
                {
                    aggregationResult.Add(item.Key, item.DocCount);
                }
            }

            return aggregationResult;
        }

        private SearchDescriptor<StandardProviderSearchResultsItem> CreateStandardProviderSearchQuery(string standardId, Coordinate coordinates, ProviderSearchFilter filter)
        {
            switch (filter.SearchOption)
            {
                case ProviderFilterOptions.ApprenticeshipId:
                    return CreateProviderQueryWithoutLocationLimit<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
                case ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly:
                    return CreateProviderQueryWithNationalProvider<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
                case ProviderFilterOptions.ApprenticeshipIdWithNationalProviderOnly:
                    return CreateProviderQueryWithNationalProviderWithoutLocationLimit<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
                default:
                    return CreateProviderQuery<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
            }
        }

        private SearchDescriptor<FrameworkProviderSearchResultsItem> CreateFrameworkProviderSearchQuery(string frameworkId, Coordinate coordinates, ProviderSearchFilter filter)
        {
            switch (filter.SearchOption)
            {
                case ProviderFilterOptions.ApprenticeshipId:
                    return CreateProviderQueryWithoutLocationLimit<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
                case ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly:
                    return CreateProviderQueryWithNationalProvider<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
                case ProviderFilterOptions.ApprenticeshipIdWithNationalProviderOnly:
                    return CreateProviderQueryWithNationalProviderWithoutLocationLimit<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
                default:
                    return CreateProviderQuery<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId, coordinates, MapDeliveryModes(filter.DeliveryModes), filter.HasNonLevyContract);
            }
        }

        private SearchResult<StandardProviderSearchResultsItem> PerformStandardProviderSearchWithQuery(int page, int take, SearchDescriptor<StandardProviderSearchResultsItem> qryStr)
        {
            var skip = CalculateSkip(page, take);

            var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToStandardProviderSearchResultsItem).ToList();

            var trainingOptionsAggregation = RetrieveAggregationElements(results.Aggs.Terms(TrainingTypeAggregateName));

            var nationalProvidersAggregation = RetrieveAggregationElements(results.Aggs.Terms(NationalProviderAggregateName));

            return new SearchResult<StandardProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation,
                NationalProvidersAggregation = nationalProvidersAggregation
            };
        }

        private SearchResult<FrameworkProviderSearchResultsItem> PerformFrameworkProviderSearchWithQuery(int page, int take, SearchDescriptor<FrameworkProviderSearchResultsItem> qryStr)
        {
            var skip = CalculateSkip(page, take);

            var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr.Skip(skip).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                throw new SearchException($"Search returned a status code of {results.ApiCall?.HttpStatusCode}");
            }

            var documents = results.Hits.Select(MapToFrameworkProviderSearhResultsItem).ToList();

            var trainingOptionsAggregation = RetrieveAggregationElements(results.Aggs.Terms(TrainingTypeAggregateName));

            var nationalProvidersAggregation = RetrieveAggregationElements(results.Aggs.Terms(NationalProviderAggregateName));

            return new SearchResult<FrameworkProviderSearchResultsItem>
            {
                Hits = documents,
                Total = results.Total,
                TrainingOptionsAggregation = trainingOptionsAggregation,
                NationalProvidersAggregation = nationalProvidersAggregation
            };
        }

        private SearchDescriptor<T> CreateProviderQuery<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes, bool hasNonLevyContract)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor = new SearchDescriptor<T>()
                .Index(_applicationSettings.ProviderIndexAlias)
                .Size(1000)
                .Query(q => q
                                .Bool(ft => ft
                                    .Filter(FilterByApprenticeshipId(selector, code))
                                    .Must(NestedLocationsQuery<T>(location)))
                            && q.Bool(ft => ft.Must(m => m.Term(f => f.HasNonLevyContract, hasNonLevyContract))))
                .Sort(SortByDistanceFromGivenLocation<T>(location))
                .Aggregations(GetProviderSearchAggregationsSelector<T>())
                .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private SearchDescriptor<T> CreateProviderQueryWithoutLocationLimit<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes, bool hasNonLevyContract)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor =
                new SearchDescriptor<T>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(q => q
                                    .Bool(b => b
                                        .Filter(FilterByApprenticeshipId(selector, code))
                                        .Must(NestedLocationsQueryWithoutLocationMatch<T>(location)))
                                && q.Bool(ft => ft.Must(m => m.Term(f => f.HasNonLevyContract, hasNonLevyContract))))
                    .Sort(SortByDistanceFromGivenLocation<T>(location))
                    .Aggregations(GetProviderSearchAggregationsSelector<T>())
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private SearchDescriptor<T> CreateProviderQueryWithNationalProvider<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes, bool hasNonLevyContract)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor =
                new SearchDescriptor<T>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(q => q
                                    .Bool(ft => ft
                                        .Filter(FilterByApprenticeshipId(selector, code), FilterByNationalProvider<T>(x => x.NationalProvider))
                                        .Must(NestedLocationsQuery<T>(location)))
                                && q.Bool(ft => ft.Must(m => m.Term(f => f.HasNonLevyContract, hasNonLevyContract))))
                    .Sort(SortByDistanceFromGivenLocation<T>(location))
                    .Aggregations(GetProviderSearchAggregationsSelector<T>())
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private SearchDescriptor<T> CreateProviderQueryWithNationalProviderWithoutLocationLimit<T>(Expression<Func<T, object>> selector, string code, Coordinate location, IEnumerable<string> deliveryModes, bool hasNonLevyContract)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            var descriptor =
                new SearchDescriptor<T>()
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .Size(1000)
                    .Query(q => q
                                    .Bool(b => b
                                        .Filter(FilterByApprenticeshipId(selector, code), FilterByNationalProvider<T>(x => x.NationalProvider))
                                        .Must(NestedLocationsQueryWithoutLocationMatch<T>(location)))
                                && q.Bool(ft => ft.Must(m => m.Term(f => f.HasNonLevyContract, hasNonLevyContract))))
                    .Sort(SortByDistanceFromGivenLocation<T>(location))
                    .Aggregations(GetProviderSearchAggregationsSelector<T>())
                    .PostFilter(pf => FilterByDeliveryModes(pf, deliveryModes));

            return descriptor;
        }

        private Func<AggregationContainerDescriptor<T>, IAggregationContainer> GetProviderSearchAggregationsSelector<T>()
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return aggs => aggs
                .Terms(TrainingTypeAggregateName, tt => tt.Field(fi => fi.DeliveryModesKeywords).MinimumDocumentCount(0))
                .Terms(NationalProviderAggregateName, tt => tt.Field(fi => fi.NationalProvider));
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

        private IEnumerable<string> MapDeliveryModes(IEnumerable<string> deliveryModes)
        {
            var response = new List<string>();

            if (deliveryModes == null)
            {
                return response;
            }

            foreach (var deliveryMode in deliveryModes)
            {
                switch (deliveryMode.ToLower())
                {
                    case "blockrelease":
                        response.Add(BlockRelease);
                        break;
                    case "dayrelease":
                        response.Add(DayRelease);
                        break;
                    case "100percentemployer":
                        response.Add(HundredPercentEmployer);
                        break;
                    default:
                        break;
                }
            }

            return response;
        }

        private int CalculateSkip(int page, int take)
        {
            var skip = (page - 1) * take;
            return skip < 0 ? 0 : skip;
        }
    }
}