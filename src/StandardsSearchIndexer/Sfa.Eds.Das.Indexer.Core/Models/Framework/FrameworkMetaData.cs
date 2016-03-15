namespace Sfa.Eds.Das.Indexer.Core.Models.Framework
{
    using Common.Models;

    public class FrameworkMetaData : IIndexEntry
    {
        public string FworkCode { get; set; }

        public string ProgType { get; set; }

        public string PwayCode { get; set; }

        public string PathwayName { get; set; }

        public string NASTitle { get; set; }

        public string IssuingAuthorityTitle { get; set; }
    }
}
