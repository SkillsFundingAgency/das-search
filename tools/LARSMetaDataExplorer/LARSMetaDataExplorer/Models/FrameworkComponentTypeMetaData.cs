using System;

namespace LARSMetaDataExplorer.Models
{
    public class FrameworkComponentTypeMetaData
    {
        public int FrameworkComponentType { get; set; }
        public string FrameworkComponentTypeDesc { get; set; }
        public string FrameworkComponentTypeDesc2 { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
