using System;
using System.Configuration;
using System.Linq;
using Nest;
using Sfa.Eds.Das.Web.Models;

namespace Sfa.Eds.Das.Web.Services
{
    public class SearchService : ISearchForStandards
    {
        public SearchResults Search(string keywords)
        {
            var searchHost = ConfigurationManager.AppSettings["SearchHost"];
            var node = new Uri(searchHost);

            var settings = new ConnectionSettings(
                node,
                defaultIndex: "cistandardindexesalias");

            settings.MapDefaultTypeNames(d => d.Add(typeof(SearchResultsItem), "standarddocument"));

            var client = new ElasticClient(settings);

            var results = client.Search<SearchResultsItem>(s => s
            .From(0)
            .Size(1000)
            .QueryString(keywords));

            return new SearchResults
            {
                TotalResults = results.Total,
                Results = results.Documents
            };
        }

        private QueryContainer BuildContainer(QueryContainer queryContainer, QueryContainer queryClause)
        {
            if (queryContainer == null)
            {
                queryContainer = queryClause;
            }
            else
            {
                queryContainer |= queryClause;
            }

            return queryContainer;
        }
    }
}