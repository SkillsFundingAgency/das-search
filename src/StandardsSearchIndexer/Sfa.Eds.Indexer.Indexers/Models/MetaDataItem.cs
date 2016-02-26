namespace Sfa.Eds.Das.Indexer.Common.Models
{

    public class MetaDataItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfFileName { get; set; }

        public string StandardPdfUrl { get; set; }

        public string AssessmentPlanPdfUrl { get; set; }

        public TypicalLength TypicalLength { get; set; }
    }
}