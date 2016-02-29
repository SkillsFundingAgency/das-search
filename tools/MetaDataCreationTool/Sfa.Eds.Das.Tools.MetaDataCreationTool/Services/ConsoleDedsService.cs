namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.DedsService;

    public class ConsoleDedsService : IDedsService
    {

        private readonly string datasetName;

        private readonly string standardDescriptorName;

        private readonly string searchEndpointConfigurationName;
        public ConsoleDedsService()
        {
            this.datasetName = "LARS";
            this.standardDescriptorName = "GetStandardCommonComponent";
            this.searchEndpointConfigurationName = "BasicHttpBinding_IDedsSearchService";
        }

        public int GetNotationLevelFromLars(int standardId)
        {
            var queryDescriptorStandard =
                this.GetQueryDescriptors(this.datasetName).Single(qd => qd.Name == this.standardDescriptorName);
            var result = this.RunQuery(queryDescriptorStandard, standardId);

            return this.GetNotationLevelFromResponse(result[1]);
        }

        public IList<QueryResults> GetStandard(int standardId)
        {
            var queryDescriptorStandard =
                this.GetQueryDescriptors(this.datasetName).Single(qd => qd.Name == this.standardDescriptorName);
            var result = this.RunQuery(queryDescriptorStandard, standardId);

            return result;
        }

        private QueryDescriptor[] GetQueryDescriptors(string dataSetName)
        {
            using (var client = new DedsSearchServiceClient(this.searchEndpointConfigurationName))
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
            using (var client = new DedsSearchServiceClient(this.searchEndpointConfigurationName))
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