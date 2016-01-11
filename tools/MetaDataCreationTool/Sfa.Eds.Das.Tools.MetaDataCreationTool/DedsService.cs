using Sfa.Eds.Das.Tools.MetaDataCreationTool.DedsService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    public static class DedsClient
    {
        private static readonly string _searchEndpointConfiguration = ConfigurationManager.AppSettings["SearchEndpointConfigurationName"];
        const string datasetName = "LARS";

        private static Dictionary<string, string> QueryFilterValuesFromConsole(QueryDescriptor queryDescriptor, int larsCode)
        {
            var queryFilterValues = new Dictionary<string, string>();

            foreach (var filter in queryDescriptor.FilterDescriptors)
            {
                Console.WriteLine("Enter {0} - {1}", filter.FieldName, (bool)filter.IsRequired ? "Required" : "Optional");
                queryFilterValues.Add(filter.FieldName, larsCode.ToString());
            }
            return queryFilterValues;
        }

        private static QueryExecution GetQueryExecution(Dictionary<string, string> queryFilterValues, int? page, int? itemsPerPage)
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

        private static void PrintResultsToConsole()
        {
            //for (int i = 0; i < _results.Count; i++)
            //{
            //    Logger.Info("========================================================");

            //    var fieldsNames = string.Join(", ", _results[i].FieldNames);
            //    Logger.Info(string.Format("Fields Names: {0}, with {1} results", fieldsNames, _results[i].Results.Count()));

            //    foreach (var result in _results[i].Results)
            //    {
            //        var fieldscount = _results[i].FieldNames.Length;
            //        for (var j = 0; j < fieldscount; j++)
            //        {
            //            var value = string.Format("{0}:{1}", _results[i].FieldNames[j], result[j]);
            //            Logger.Info(value);
            //        }

            //        Logger.Info("========================================================");
            //    }

            //    Logger.Info("========================================================");
            //}

            //Logger.Info("Process Completed");
            //Console.WriteLine("Press any key to start a new query");
            //Console.ReadLine();
        }


        /// <summary>
        /// Query Descriptors contain parameter listings under the FilterDescriptors property
        /// </summary>
        /// <param name="dataSetName"></param>
        /// <returns></returns>
        private static QueryDescriptor[] GetQueryDescriptors(string dataSetName)
        {
            using (var client = new DedsSearchServiceClient(_searchEndpointConfiguration))
            {
                var dataSetVersionDescriptor = client.GetLatestPublishedDataSetVersion(dataSetName);

                var queryDescriptors = client.DiscoverQueries(new DiscoverQueriesCriteria() { DataSetVersionId = dataSetVersionDescriptor.Id });
                return queryDescriptors;
            }
        }

        private static IList<QueryResults> ExecuteQuery(QueryDescriptor queryDescriptor, QueryExecution queryExecution)
        {
            //Logger.Info("Process Started");

            using (var client = new DedsSearchServiceClient(_searchEndpointConfiguration))
            {
                return client.ExecuteQuery((Guid)queryDescriptor.Id, queryExecution);
            }
        }

        private static IList<QueryResults> RunQuery(QueryDescriptor qds, int larsCode)
        {
            var queryFilterValues = QueryFilterValuesFromConsole(qds, larsCode);

            var queryExecution = GetQueryExecution(queryFilterValues, null, null);

            return ExecuteQuery(qds, queryExecution);
        }

        public static IList<QueryResults> GetLarsData(int larsCode)
        {
            // Summary info: GetStandardCommonComponent
            var queryDescriptors = GetQueryDescriptors(datasetName);
            var qdsFundingValues = queryDescriptors.Single(qd => qd.Name == "GetStandardCommonComponent");
            return RunQuery(qdsFundingValues, larsCode);
        }
    }
}
