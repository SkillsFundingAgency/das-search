using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LARSMetaDataToolBox.Models;

namespace LARSMetaDataToolBox
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
