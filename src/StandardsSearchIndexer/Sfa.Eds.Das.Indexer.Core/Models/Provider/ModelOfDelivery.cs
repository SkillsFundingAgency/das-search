using System.ComponentModel;

namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
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
