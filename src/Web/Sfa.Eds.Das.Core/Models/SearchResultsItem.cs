namespace Sfa.Eds.Das.Core.Models
{

    public sealed class SearchResultsItem
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEntryLevel { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfUrl { get; set; }

        public string MinimumLength { get; set; }

        public bool HasError { get; set; }
    }
}