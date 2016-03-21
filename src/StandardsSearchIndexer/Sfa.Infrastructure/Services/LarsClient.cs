namespace Sfa.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.DedsService;
    using Sfa.Infrastructure.Settings;

    public class LarsClient : IGetStandardLevel
    {
        private readonly ILarsSettings _larsSettings;

        public LarsClient(ILarsSettings larsSettings)
        {
            _larsSettings = larsSettings;
        }

        public int GetNotationLevel(int standardId)
        {
            var queryDescriptorStandard = GetQueryDescriptors(_larsSettings.DatasetName).Single(qd => qd.Name == _larsSettings.StandardDescriptorName);
            var result = RunQuery(queryDescriptorStandard, standardId);

            return GetNotationLevelFromResponse(result[1]);
        }

        private QueryDescriptor[] GetQueryDescriptors(string dataSetName)
        {
            using (var client = new DedsSearchServiceClient(_larsSettings.SearchEndpointConfigurationName))
            {
                var dataSetVersionDescriptor = client.GetLatestPublishedDataSetVersion(dataSetName);

                var queryDescriptors = client.DiscoverQueries(new DiscoverQueriesCriteria { DataSetVersionId = dataSetVersionDescriptor.Id });
                return queryDescriptors;
            }
        }

        private IList<QueryResults> RunQuery(QueryDescriptor qds, int larsCode)
        {
            var queryFilterValues = QueryFilterValuesFromConsole(qds, larsCode);

            var queryExecution = GetQueryExecution(queryFilterValues, null, null);

            return ExecuteQuery(qds, queryExecution);
        }

        private Dictionary<string, string> QueryFilterValuesFromConsole(QueryDescriptor queryDescriptor, int larsCode)
        {
            return queryDescriptor.FilterDescriptors.ToDictionary(filter => filter.FieldName, filter => larsCode.ToString());
        }

        private QueryExecution GetQueryExecution(Dictionary<string, string> queryFilterValues, int? page, int? itemsPerPage)
        {
            var filterValues = new List<FilterValue>();

            filterValues.AddRange(queryFilterValues.Select(queryFilterValue => new FilterValue { FieldName = queryFilterValue.Key, FieldValue = queryFilterValue.Value }));

            var queryExecution = new QueryExecution { FilterValues = filterValues.Where(x => !string.IsNullOrEmpty(x.FieldValue)).ToArray(), SortValues = new SortValue[0] };

            if (page.HasValue && itemsPerPage.HasValue)
            {
                queryExecution.PageNumber = page;
                queryExecution.PageSize = itemsPerPage;
            }

            return queryExecution;
        }

        private IList<QueryResults> ExecuteQuery(QueryDescriptor queryDescriptor, QueryExecution queryExecution)
        {
            using (var client = new DedsSearchServiceClient(_larsSettings.SearchEndpointConfigurationName))
            {
                return client.ExecuteQuery((Guid)queryDescriptor.Id, queryExecution);
            }
        }

        private int GetNotationLevelFromResponse(QueryResults result)
        {
            return result.Results.Length != 0 ? int.Parse(result.Results[0][5]) : 0;
        }
    }
}