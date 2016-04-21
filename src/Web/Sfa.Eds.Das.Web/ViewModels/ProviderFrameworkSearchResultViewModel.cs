﻿namespace Sfa.Eds.Das.Web.ViewModels
{
    using System.Collections.Generic;

    public class ProviderFrameworkSearchResultViewModel
    {
        public string Title { get; set; }

        public long TotalResults { get; set; }

        public int FrameworkId { get; set; }

        public int FrameworkCode { get; set; }

        public int Level { get; set; }
        
        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<FrameworkProviderResultItemViewModel> Hits { get; set; }

        public bool HasError { get; set; }

        public bool PostCodeMissing { get; set; }

        public int FrameworkLevel { get; set; }
    }
}