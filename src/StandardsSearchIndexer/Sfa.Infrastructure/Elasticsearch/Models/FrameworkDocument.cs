using Nest;
using Sfa.Eds.Das.Indexer.Core.Models;

namespace Sfa.Infrastructure.Elasticsearch.Models
{
    public sealed class FrameworkDocument : IApprenticeshipDocument, IIndexEntry
    {
        [String(Analyzer = "english")]
        public string Title { get; set; }
        public int Level { get; set; }
        public int FrameworkCode { get; set; }
        public string FrameworkId { get; set; }
        [String(Analyzer = "english")]
        public string FrameworkName { get; set; }
        public int PathwayCode { get; set; }
        [String(Analyzer = "english")]
        public string PathwayName { get; set; }
    }
}
