using System;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Infrastructure
{
    public interface IRetryWebRequests
    {
        Task<T> RetryWeb<T>(Func<Task<T>> action, Action<Exception> onError);
    }
}