using System;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Infrastructure
{
    public interface IRetryWebRequests
    {
        Task<T> RetryWeb<T>(Func<Task<T>> action, Action<Exception> onError);
    }
}