using System;

namespace LARSMetaDataExplorer.Models
{
    public class FundingMetaData
    {
        public string LearnAimRef { get; set; }
        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int? RateWeighted { get; set; }
    }
}
