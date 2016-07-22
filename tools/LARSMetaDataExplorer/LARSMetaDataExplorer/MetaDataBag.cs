using System.Collections.Generic;
using LARSMetaDataExplorer.Models;

namespace LARSMetaDataExplorer
{
    public class MetaDataBag
    {
        public ICollection<FrameworkMetaData> Frameworks { get; set; }
        public ICollection<LarsStandard> Standards { get; set; }
        public ICollection<FrameworkAimMetaData> FrameworkAims { get; set; }
        public ICollection<FrameworkComponentTypeMetaData> FrameworkComponentTypes { get; set; }
        public ICollection<LearningDeliveryMetaData> LearningDeliveries { get; set; }
        public ICollection<FundingMetaData> Fundings { get; set; }
    }
}
