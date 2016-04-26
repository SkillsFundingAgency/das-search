using System;

namespace Sfa.Das.Sas.Indexer.Core.Models.Framework
{
    public class FrameworkMetaData : IIndexEntry
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public string PathwayName { get; set; }

        public string NASTitle { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime EffectiveTo { get; set; }
    }
}
