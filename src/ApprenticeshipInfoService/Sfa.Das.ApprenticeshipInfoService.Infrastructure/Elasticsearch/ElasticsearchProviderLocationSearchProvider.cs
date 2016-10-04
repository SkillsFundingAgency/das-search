using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Sfa.Das.ApprenticeshipInfoService.Core.Configuration;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Elasticsearch
{
    public class ElasticsearchProviderLocationSearchProvider : IProviderLocationSearchProvider
    {
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private const string TrainingTypeAggregateName = "training_type";
        private const string NationalProviderAggregateName = "national_provider";

        public ElasticsearchProviderLocationSearchProvider(
            IConfigurationSettings applicationSettings,
            IElasticsearchCustomClient elasticsearchCustomClient)
        {
            _applicationSettings = applicationSettings;
            _elasticsearchCustomClient = elasticsearchCustomClient;
        }

        public List<StandardProviderSearchResultsItem> SearchStandardProviders(int standardId, Coordinate coordinates, int page)
        {
            var qryStr = CreateStandardProviderSearchQuery(standardId.ToString(), coordinates);
            return PerformStandardProviderSearchWithQuery(qryStr, page);
        }

        public List<FrameworkProviderSearchResultsItem> SearchFrameworkProviders(int frameworkId, Coordinate coordinates, int page)
        {
            var qryStr = CreateFrameworkProviderSearchQuery(frameworkId.ToString(), coordinates);
            return PerformFrameworkProviderSearchWithQuery(qryStr, page);
        }

        private SearchDescriptor<StandardProviderSearchResultsItem> CreateStandardProviderSearchQuery(string standardId, Coordinate coordinates)
        {
            return CreateProviderQuery<StandardProviderSearchResultsItem>(x => x.StandardCode, standardId, coordinates);
        }

        private SearchDescriptor<FrameworkProviderSearchResultsItem> CreateFrameworkProviderSearchQuery(string frameworkId, Coordinate coordinates)
        {
            return CreateProviderQuery<FrameworkProviderSearchResultsItem>(x => x.FrameworkId, frameworkId, coordinates);
        }

        private List<StandardProviderSearchResultsItem> PerformStandardProviderSearchWithQuery(SearchDescriptor<StandardProviderSearchResultsItem> qryStr, int page)
        {
            var take = _applicationSettings.ApprenticeshipProviderElements;

            var skipAmount = take * (page - 1);

            var results = _elasticsearchCustomClient.Search<StandardProviderSearchResultsItem>(_ => qryStr.Skip(skipAmount).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                return new List<StandardProviderSearchResultsItem>();
            }

            return results.Hits.Select(MapToStandardProviderSearchResultsItem).ToList();
        }

        private List<FrameworkProviderSearchResultsItem> PerformFrameworkProviderSearchWithQuery(SearchDescriptor<FrameworkProviderSearchResultsItem> qryStr, int page)
        {
            var take = _applicationSettings.ApprenticeshipProviderElements;

            var skipAmount = take * (page - 1);

            var results = _elasticsearchCustomClient.Search<FrameworkProviderSearchResultsItem>(_ => qryStr.Skip(skipAmount).Take(take));

            if (results.ApiCall?.HttpStatusCode != 200)
            {
                return new List<FrameworkProviderSearchResultsItem>();
            }

            return results.Hits.Select(MapToFrameworkProviderSearhResultsItem).ToList();
        }

        private SearchDescriptor<T> CreateProviderQuery<T>(Expression<Func<T, object>> selector, string code, Coordinate location)
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
                    .Sort(SortByDistanceFromGivenLocation<T>(location));

            return descriptor;
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> FilterByApprenticeshipId<T>(Expression<Func<T, object>> selector, string apprenticeshipIdentifier)
            where T : class
        {
            return f => f.Term(t => t.Field(selector).Value(apprenticeshipIdentifier));
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

        private static Func<SortDescriptor<T>, IPromise<IList<ISort>>> NestedSortByDistanceFromGivenLocation<T>(Coordinate location)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return f => f.GeoDistance(g => g
                .Field(fd => fd.TrainingLocations.First().LocationPoint)
                .PinTo(new GeoLocation(location.Lat, location.Lon))
                .Unit(DistanceUnit.Miles)
                .Ascending());
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> FilterByLocation<T>(Coordinate location)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            return f => f.GeoShapePoint(gp => gp.Field(fd => fd.TrainingLocations.First().Location).Coordinates(location.Lon, location.Lat));
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
                NationalProvider = hit.Source.NationalProvider
            };
        }
    }
}
