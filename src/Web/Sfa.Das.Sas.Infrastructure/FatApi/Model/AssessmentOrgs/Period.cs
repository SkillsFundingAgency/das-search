using System;

namespace Sfa.Das.FatApi.Client.Model
{
    public class Period
    {
        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}