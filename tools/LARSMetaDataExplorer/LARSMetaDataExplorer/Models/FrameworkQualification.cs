using System;

namespace LARSMetaDataToolBox.Models
{
    public class FrameworkQualification
    {
        public string LearnAimRef { get; set; }
        public string Title { get; set; }
        public int CompetenceType { get; set; }
        public string CompetenceDescription { get; set; }
        public DateTime? FundingEffectiveTo { get; set; }
        public int? FundingRateWeight { get; set; }
        public int ProgType { get; set; }
    }
}
