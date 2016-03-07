namespace Sfa.Eds.Das.Indexer.ApplicationServices.Http
{
    using System;

    public interface IRetryWebRequest
    {
        T RetryWeb<T>(Func<T> action);
    }
}