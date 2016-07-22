using System;

namespace LARSMetaDataToolBox.Models
{
    public class LearningDeliveryMetaData
    {
        public string LearnAimRef { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string LearnAimRefTitle { get; set; }
        public int LearnAimRefType { get; set; }
    }
}
