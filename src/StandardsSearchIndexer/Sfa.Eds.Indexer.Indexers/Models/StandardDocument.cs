using Nest;

namespace Sfa.Eds.Indexer.Indexer.Infrastructure.Models
{
    public class StandardDocument
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfFileName { get; set; }

        public string PdfUrl { get; set; }

        [ElasticProperty(Type = FieldType.Attachment, TermVector = TermVectorOption.WithPositionsOffsets, Store = true)]
        public Attachment File { get; set; }
    }
}