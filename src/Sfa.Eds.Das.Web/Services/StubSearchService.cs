using System.Collections.Generic;
using Sfa.Eds.Das.Web.Models;

namespace Sfa.Eds.Das.Web.Services
{
    public class StubSearchService : ISearchForStandards
    {
        public SearchResults Search(string keywords)
        {
            return CreateStubSearchResults();
        }

        private static SearchResults CreateStubSearchResults()
        {
            var results = new SearchResults
            {
                TotalResults = 5,
                Results = CreateListOfResults()
            };

            return results;
        }

        private static IEnumerable<SearchResultsItem> CreateListOfResults()
        {
            var resultItems = new List<SearchResultsItem>(5)
            {
                new SearchResultsItem { Title = "DENTAL_HEALTH_Dental_Technician.ashx.pdf" },
                new SearchResultsItem { Title = "BUTCHERY_Butcher.ashx.pdf" },
                new SearchResultsItem { Title = "AUTOMOTIVE_Manufacturing_Engineer_standard.ashx.pdf" },
                new SearchResultsItem { Title = "ACTUARIAL_-_Actuarial_Technician.pdf" },
                new SearchResultsItem { Title = "AEROSPACE_-_Aerospace_Engineer.pdf" }
            };

            return resultItems;
        }
    }
}