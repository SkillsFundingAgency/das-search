using Sfa.Eds.Das.Web.Models;
using System.Collections.Generic;

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
                new SearchResultsItem {Name = "DENTAL_HEALTH_Dental_Technician.ashx.pdf", TextSnippet = CreateSnippetText()},
                new SearchResultsItem {Name = "BUTCHERY_Butcher.ashx.pdf", TextSnippet = CreateSnippetText()},
                new SearchResultsItem {Name = "AUTOMOTIVE_Manufacturing_Engineer_standard.ashx.pdf", TextSnippet = CreateSnippetText()},
                new SearchResultsItem {Name = "ACTUARIAL_-_Actuarial_Technician.pdf", TextSnippet = CreateSnippetText()},
                new SearchResultsItem {Name = "AEROSPACE_-_Aerospace_Engineer.pdf", TextSnippet = CreateSnippetText()}
            };

            return resultItems;
        }

        private static string CreateSnippetText()
        {
            return
                "Bacon ipsum dolor amet incididunt flank dolore beef chicken ham hock prosciutto ad sunt et fugiat aute dolore pork belly laborum. Flank in ut aliquip ground round eu. Pastrami ball tip sirloin spare ribs ut sed aliquip est turkey beef ribs drumstick boudin venison. Short ribs minim bacon labore veniam ea jerky kevin picanha turkey brisket adipisicing tri-tip.";
        }
    }
}