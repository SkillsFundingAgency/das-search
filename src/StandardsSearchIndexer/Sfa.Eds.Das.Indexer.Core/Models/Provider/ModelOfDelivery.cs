namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    using System.ComponentModel;

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