namespace Sfa.Das.Sas.Indexer.Core.Logging.Metrics
{
    public struct ExecutionTimerResult<T>
    {
        public T Result { get; set; }

        public double ElaspedMilliseconds { get; set; }
    }
}
