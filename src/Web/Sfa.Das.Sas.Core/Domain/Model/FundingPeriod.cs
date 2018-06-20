using System;

namespace Sfa.Das.Sas.Core.Domain.Model
{
    public class FundingPeriod
    {
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int FundingCap { get; set; }
    }
}