namespace Sfa.Eds.Das.Core.Domain.Model
{
    public sealed class Standard
    {
        public int StandardId { get; set; }

        public string StandardTitle { get; set; }

        public int NotionalEntryLevel { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfUrl { get; set; }

        public string MinimumLength { get; set; }
    }
}
