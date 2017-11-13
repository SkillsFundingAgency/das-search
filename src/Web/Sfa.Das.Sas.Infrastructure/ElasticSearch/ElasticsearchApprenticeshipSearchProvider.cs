using System;
using FeatureToggle.Core.Fluent;
using Sfa.Das.Sas.Infrastructure.FeatureToggles;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using System.Collections.Generic;
    using System.Linq;
    using Nest;
    using SFA.DAS.NLog.Logger;
    using Sfa.Das.Sas.ApplicationServices;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Configuration;

    public sealed class ElasticsearchApprenticeshipSearchProvider : IApprenticeshipSearchProvider
    {
        private const string LevelAggregateName = "level";
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _logger;
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchApprenticeshipSearchProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger, IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, int order, List<int> selectedLevels)
        {
            var formattedKeywords = QueryHelper.FormatQuery(keywords);

            var searchDescriptor = GetSearchDescriptor(page, take, formattedKeywords, order, selectedLevels?.ToList());

            var results = _elasticsearchCustomClient.Search<ApprenticeshipSearchResultsItem>(s => searchDescriptor);

            var levelAggregation = BuildLevelAggregationResult(results);

            return MapToApprenticeshipSearchResults(take, selectedLevels, formattedKeywords, results, levelAggregation);
        }

        private static ApprenticeshipSearchResults MapToApprenticeshipSearchResults(
            int take,
            IEnumerable<int> selectedLevels,
            string formattedKeywords,
            ISearchResponse<ApprenticeshipSearchResultsItem> results,
            Dictionary<int, long?> levelAggregation)
        {
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

        private static QueryContainer FilterBySelectedLevels(
            QueryContainerDescriptor<ApprenticeshipSearchResultsItem> descriptor,
            IList<int> selectedLevels)
        {
            if (selectedLevels == null || selectedLevels.Count == 0)
            {
                return descriptor;
            }

            return descriptor
                .Terms(t => t
                    .Field(s => s.Level)
                    .Terms(selectedLevels));
        }

        private static Dictionary<int, long?> BuildLevelAggregationResult(ISearchResponse<ApprenticeshipSearchResultsItem> results)
        {
            var levelAggregation = new Dictionary<int, long?>();

            if (results.Aggs.Terms(LevelAggregateName) != null)
            {
                foreach (var item in results.Aggs.Terms(LevelAggregateName).Buckets)
                {
                    int iKey;
                    if (int.TryParse(item.Key, out iKey))
                    {
                        levelAggregation.Add(iKey, item.DocCount);
                    }
                }
            }

            return levelAggregation;
        }

        private static void GetSortingOrder(SearchDescriptor<ApprenticeshipSearchResultsItem> searchDescriptor, int order)
        {
            if (order == 0 || order == 1)
            {
	            if (Is<Elk5Feature>.Enabled)
	            {
					searchDescriptor.Sort(s => s
						.Descending(SortSpecialField.Score)
						.Descending(f => f.TitleKeyword)
						.Descending(f => f.Level));
				}
	            else
	            {
					searchDescriptor.Sort(s => s
						.Descending(SortSpecialField.Score)
						.Descending(f => f.Title)
						.Descending(f => f.Level));
				}
            }

            if (order == 2)
            {
                searchDescriptor.Sort(s => s.Descending(p => p.Level));
            }

            if (order == 3)
            {
                searchDescriptor.Sort(s => s.Ascending(p => p.Level));
            }
        }

        private SearchDescriptor<ApprenticeshipSearchResultsItem> GetSearchDescriptor(
            int page, int take, string formattedKeywords, int order, IList<int> selectedLevels)
        {
            return formattedKeywords == "*"
                ? GetAllSearchDescriptor(page, take, formattedKeywords, order, selectedLevels)
                : GetKeywordSearchDescriptor(page, take, formattedKeywords, order, selectedLevels);
        }

        private SearchDescriptor<ApprenticeshipSearchResultsItem> GetAllSearchDescriptor(
            int page, int take, string formattedKeywords, int order, IList<int> selectedLevels)
        {
            var skip = (page - 1) * take;

            var searchDescriptor = new SearchDescriptor<ApprenticeshipSearchResultsItem>()
                .Index(_applicationSettings.ApprenticeshipIndexAlias)
                .AllTypes()
                .Skip(skip)
                .Query(q => q
                    .QueryString(qs => qs
                        .Fields(fs => fs
                            .Field(f => f.Title)
                            .Field(p => p.JobRoles)
                            .Field(p => p.Keywords)
                            .Field(p => p.FrameworkName)
                            .Field(p => p.PathwayName)
                            .Field(p => p.JobRoleItems.First().Title)
                            .Field(p => p.JobRoleItems.First().Description))
                        .Query(formattedKeywords)))
                .PostFilter(GetPostFilter(selectedLevels))
                .Aggregations(agg => agg
                    .Terms(LevelAggregateName, t => t
                        .Field(f => f.Level).MinimumDocumentCount(0)));

            GetSortingOrder(searchDescriptor, order);

            return searchDescriptor;
        }

        private SearchDescriptor<ApprenticeshipSearchResultsItem> GetKeywordSearchDescriptor(
            int page, int take, string formattedKeywords, int order, IList<int> selectedLevels)
        {
            var skip = (page - 1) * take;
            var searchDescriptor = new SearchDescriptor<ApprenticeshipSearchResultsItem>()
                    .Index(_applicationSettings.ApprenticeshipIndexAlias)
                    .AllTypes()
                    .Skip(skip)
                    .Take(take)
                    .Query(q => q
                        .Bool(b => b
                            .Should(
                                bs1 => bs1
                                    .MultiMatch(mm => mm
                                    .Type(TextQueryType.BestFields)
                                    .Fields(fi => fi
                                        .Field(f => f.Title)
                                        .Field(f => f.Keywords)
                                        .Field(f => f.JobRoles)
                                        .Field(f => f.JobRoleItems.First().Title)
                                        .Field(f => f.JobRoleItems.First().Description))
                                    .Query(formattedKeywords)
                                    .MinimumShouldMatch("2<70%")
                                    .PrefixLength(3)),
                                bs2 => bs2
                                    .MultiMatch(mm => mm
                                    .Type(TextQueryType.CrossFields)
                                    .Fields(fi => fi
                                        .Field(f => f.Title, 2)
                                        .Field(f => f.Keywords))
                                    .Query(formattedKeywords)
                                    .PrefixLength(3)))))
                    .PostFilter(GetPostFilter(selectedLevels))
                    .Aggregations(agg => agg
                        .Terms(LevelAggregateName, t => t
                            .Field(f => f.Level).MinimumDocumentCount(0)));

            GetSortingOrder(searchDescriptor, order);

            return searchDescriptor;
        }

        private Func<QueryContainerDescriptor<ApprenticeshipSearchResultsItem>, QueryContainer> GetPostFilter(IList<int> selectedLevels)
        {
            return m => FilterBySelectedLevels(m, selectedLevels)
                        && m.Bool(b => b.Should(n => n
                            .DateRange(r => r
                                .GreaterThanOrEquals(DateTime.Now)
                                .Field(f => f.EffectiveTo))
                            || n.Missing(mi => mi
                                .Field(f => f.EffectiveTo))))
                        && m.Bool(b => b.Should(n => n.DateRange(r => r
                            .LessThanOrEquals(DateTime.Now)
                            .Field(f => f.EffectiveFrom))
                            || n.Missing(mi => mi
                                .Field(f => f.EffectiveFrom))))
                        && m.Bool(b2 => b2
                            .Filter(f => f
                                .Term(t => t
                                    .Field(fi => fi.Published).Value(true))));
        }
    }
}