namespace Sfa.Das.Sas.WebTest.Infrastructure.Services
{
    using System;
    using System.Threading.Tasks;

    using Polly;

    public sealed class WebRequestRetryService : IRetryWebRequests
    {
        public async Task<T> RetryWeb<T>(Func<Task<T>> action, Action<Exception> onError)
        {
            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    2,
                    retrytime => TimeSpan.FromSeconds(Math.Pow(2, retrytime)),
                    (exception, timespan) => { onError.Invoke(exception); });

            return await policy.ExecuteAsync(action);
        }

        public void RetryWeb(Action action, Action<Exception> onError)
        {
            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(
                    2,
                    retrytime => TimeSpan.FromSeconds(Math.Pow(2, retrytime)),
                    (exception, timespan) => { onError.Invoke(exception); });

            policy.Execute(action);
        }
    }
}