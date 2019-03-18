using System.Collections.Generic;
using test_site.Models;

namespace test_site
{
    public interface IGenerateSearchResults
    {
        IEnumerable<SearchResultItem> Generate();
    }
}