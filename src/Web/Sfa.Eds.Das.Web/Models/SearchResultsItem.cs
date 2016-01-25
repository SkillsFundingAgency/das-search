using System;

namespace Sfa.Eds.Das.Web.Models
{
    public sealed class SearchResultsItem
    {
        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfUrl { get; set; }
    }
}