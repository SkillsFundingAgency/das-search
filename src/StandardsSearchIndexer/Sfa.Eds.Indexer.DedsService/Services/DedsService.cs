using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Eds.Indexer.DedsService.DedsService;
using Sfa.Eds.Indexer.Settings.Settings;

namespace Sfa.Eds.Indexer.DedsService.Services
{
    public class DedsService : IDedsService
    {
        private IStandardIndexSettings _standardIndexSettings;

        public DedsService(IStandardIndexSettings standardIndexSettings)
        {
            _standardIndexSettings = standardIndexSettings;
        }

        public int GetNotationLevelFromLars(int standardId)
        {
            var queryDescriptorStandard =
                this.GetQueryDescriptors(_standardIndexSettings.DatasetName).Single(qd => qd.Name == _standardIndexSettings.StandardDescriptorName);
            var result = this.RunQuery(queryDescriptorStandard, standardId);

            return this.GetNotationLevelFromResponse(result[1]);
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

        private IList<QueryResults> RunQuery(QueryDescriptor qds, int larsCode)
        {
            var queryFilterValues = this.QueryFilterValuesFromConsole(qds, larsCode);

            var queryExecution = this.GetQueryExecution(queryFilterValues, null, null);

            return this.ExecuteQuery(qds, queryExecution);
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

        private int GetNotationLevelFromResponse(QueryResults result)
        {
            return result.Results.Length != 0 ? int.Parse(result.Results[0][5]) : 0;
        }
    }
}