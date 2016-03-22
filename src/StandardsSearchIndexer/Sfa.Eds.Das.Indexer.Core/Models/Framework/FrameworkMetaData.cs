namespace Sfa.Eds.Das.Indexer.Core.Models.Framework
{
    using System;

    using Common.Models;

    public class FrameworkMetaData : IIndexEntry
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public string PathwayName { get; set; }

        public string NASTitle { get; set; }

        public string IssuingAuthorityTitle { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime EffectiveTo { get; set; }
    }
}
