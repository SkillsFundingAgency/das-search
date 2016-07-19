using Nest;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    public sealed class Address
    {
        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string Address1 { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string Address2 { get; set; }

        public string Town { get; set; }

        public string County { get; set; }

        public string PostCode { get; set; }
    }
}
