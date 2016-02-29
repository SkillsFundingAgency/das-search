using Nest;

namespace Sfa.Eds.Das.Indexer.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class StandardDocument
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> JobRoles { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfFileName { get; set; }

        public string StandardPdf { get; set; }

        [ElasticProperty(Type = FieldType.Attachment, TermVector = TermVectorOption.WithPositionsOffsets, Store = true)]
        public Attachment File { get; set; }

        public string AssessmentPlanPdf { get; set; }

        public TypicalLength TypicalLength { get; set; }
    }
}