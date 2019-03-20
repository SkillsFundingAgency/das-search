using System.Collections.Generic;

namespace test_site.Models
{
    public class SearchCriteria
    {
        public string Keywords { get; set; }
        public List<int> SelectedLevels { get; set; }
        public string SearchRouteName { get; set; }
    }
}