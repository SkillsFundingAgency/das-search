using System.Collections.Generic;
using shared_lib.Models;

namespace shared_lib
{
    public interface IGenerateSearchResults
    {
        IEnumerable<SearchResultItem> Generate();
    }
}