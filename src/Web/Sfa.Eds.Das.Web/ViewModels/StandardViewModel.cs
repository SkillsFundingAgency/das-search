namespace Sfa.Eds.Das.Web.ViewModels
{
    public sealed class StandardViewModel
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEntryLevel { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfUrl { get; set; }

        public string MinimumLength { get; set; }

        public LinkViewModel SearchResultLink { get; set; }
    }
}