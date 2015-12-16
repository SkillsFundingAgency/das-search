using Nest;
using Sfa.Eds.Das.Web.Models;
using System;

namespace Sfa.Eds.Das.Web.Services
{
    public class SearchService : ISearchForStandards
    {
        public SearchResults Search(string keywords)
        {
            var node = new Uri("http://192.168.99.100:9200");

            var settings = new ConnectionSettings(
                node,
                defaultIndex: "trying-out-mapper-attachements"
            );

            settings.MapDefaultTypeNames(d => d.Add(typeof(SearchResultsItem), "search-results-item"));

            var client = new ElasticClient(settings);

            var results = client.Search<SearchResultsItem>(s => s
            .From(0)
            .Size(10)
            .QueryRaw(@"{""query_string"": {""query"": """ + keywords + @"""}}")
            );

            return new SearchResults
            {
                TotalResults = results.Total,
                Results = results.Documents
            };
        }
    }
}