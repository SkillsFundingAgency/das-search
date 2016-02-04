using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.DedsService;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public class DedsService : IDedsService
    {
        private static IStandardIndexSettings _standardIndexSettings;

        public DedsService(IStandardIndexSettings standardIndexSettings)
        {
            _standardIndexSettings = standardIndexSettings;
        }

        public int GetNotationLevelFromLars(int standardId)
        {
            var queryDescriptorStandard =
                GetQueryDescriptors(_standardIndexSettings.DatasetName).Single(qd => qd.Name == _standardIndexSettings.StandardDescriptorName);
            var result = RunQuery(queryDescriptorStandard, standardId);

            return GetNotationLevelFromResponse(result[1]);
        }

        private int GetNotationLevelFromResponse(QueryResults result)
        {
            return result.Results.Length != 0 ? int.Parse(result.Results[0][5]) : 0;
        }

        private QueryDescriptor[] GetQueryDescriptors(string dataSetName)
        {
            using (var client = new DedsSearchServiceClient(_standardIndexSettings.SearchEndpointConfigurationName))
            {
                var dataSetVersionDescriptor = client.GetLatestPublishedDataSetVersion(dataSetName);

                var queryDescriptors = client.DiscoverQueries(new DiscoverQueriesCriteria { DataSetVersionId = dataSetVersionDescriptor.Id });
                return queryDescriptors;
            }
        }

        private Dictionary<string, string> QueryFilterValuesFromConsole(QueryDescriptor queryDescriptor, int larsCode)
        {
            return queryDescriptor.FilterDescriptors.ToDictionary(filter => filter.FieldName, filter => larsCode.ToString());
        }

        private QueryExecution GetQueryExecution(Dictionary<string, string> queryFilterValues, int? page, int? itemsPerPage)
        {
            var filterValues = new List<FilterValue>();

            filterValues.AddRange(queryFilterValues.Select(queryFilterValue => new FilterValue
            {
                FieldName = queryFilterValue.Key,
                FieldValue = queryFilterValue.Value
            }));

            var queryExecution = new QueryExecution
            {
                FilterValues = filterValues.Where(x => !string.IsNullOrEmpty(x.FieldValue)).ToArray(),
                SortValues = new SortValue[0]
            };

            if (page.HasValue && itemsPerPage.HasValue)
            {
                queryExecution.PageNumber = page;
                queryExecution.PageSize = itemsPerPage;
            }

            return queryExecution;
        }

        private IList<QueryResults> ExecuteQuery(QueryDescriptor queryDescriptor, QueryExecution queryExecution)
        {
            using (var client = new DedsSearchServiceClient(_standardIndexSettings.SearchEndpointConfigurationName))
            {
                return client.ExecuteQuery((Guid)queryDescriptor.Id, queryExecution);
            }
        }

        private IList<QueryResults> RunQuery(QueryDescriptor qds, int larsCode)
        {
            var queryFilterValues = QueryFilterValuesFromConsole(qds, larsCode);

            var queryExecution = GetQueryExecution(queryFilterValues, null, null);

            return ExecuteQuery(qds, queryExecution);
        }
    }
}