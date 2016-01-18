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
                defaultIndex: "standardIndexAlias");

            settings.MapDefaultTypeNames(d => d.Add(typeof(SearchResultsItem), "standarddocument"));

            var client = new ElasticClient(settings);

            var results = client.Search<SearchResultsItem>(s => s
            .From(0)
            .Size(1000)
            .QueryString(keywords));

            var queryTerm = Query<StandardDocument>.Term("file", keywords);

            var test = client.Search<StandardDocument>(s =>
            {
                s.Skip(10);
                s.Take(10);
                s.Query(q =>
                    q.Term("Title", keywords)
                    || q.Term("File.Content", keywords));
                    
                /*
                s.Query(q => 
                {
                    QueryContainer query = null;

                    var queryClause = q.Match(m =>
                    {
                        m.OnField(f => f.File.Content).Query(keywords);
                    });

                    query = BuildContainer(null, queryClause);


                    return query;
                });
                */
                return s;
            });

            var test2 = client.Search<StandardDocument>(s => s.Query(queryTerm));
                
            var a = test;

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