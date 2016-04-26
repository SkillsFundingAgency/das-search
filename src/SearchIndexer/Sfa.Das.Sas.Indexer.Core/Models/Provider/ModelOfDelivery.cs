using System.ComponentModel;

namespace Sfa.Das.Sas.Indexer.Core.Models.Provider
{
    public enum ModesOfDelivery
    {
        [Description("100PercentEmployer")]
        OneHundredPercentEmployer,

        [Description("BlockRelease")]
        BlockRelease,

        [Description("DayRelease")]
        DayRelease
    }
}