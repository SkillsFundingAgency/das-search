using System;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Http
{
    public interface IRetryWebRequest
    {
        T RetryWeb<T>(Func<T> action);
    }
}