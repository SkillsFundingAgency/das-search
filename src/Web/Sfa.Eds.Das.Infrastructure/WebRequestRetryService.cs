using System;
using System.Threading.Tasks;
using Polly;
using Sfa.Eds.Das.Core.Logging;

namespace Sfa.Eds.Das.Infrastructure
{
    public sealed class WebRequestRetryService : IRetryWebRequests
    {
        private readonly ILog _logger;

        public WebRequestRetryService(ILog logger)
        {
            _logger = logger;
        }

        public async Task<T> RetryWeb<T>(Func<Task<T>> action, Action<Exception> onError)
        {
            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    2,
                    retrytime => TimeSpan.FromSeconds(Math.Pow(2, retrytime)),
                    (exception, timespan) => { onError.Invoke(exception); });

            return await policy.ExecuteAsync(action);
        }
    }
}
