namespace Sfa.Eds.Das.Indexer.Common.Models
{
    using System;

    public class JsonMetadataObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int NotionalEndLevel { get; set; }
        public string PdfFileName { get; set; }
        public string StandardPdf { get; set; }
        public Uri AssessmentPlanPdf { get; set; }
        public TypicalLength TypicalLength { get; set; }
    }
}