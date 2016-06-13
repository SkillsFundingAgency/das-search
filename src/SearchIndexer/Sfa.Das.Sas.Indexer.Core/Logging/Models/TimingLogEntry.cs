namespace Sfa.Das.Sas.Indexer.Core.Logging.Models
{
    public class TimingLogEntry : ILogEntry
    {
        public string Name => "Timing";

        public double ElaspedMilliseconds { get; set; }
    }
}
