using System;

namespace CircuitBreaker.Core
{
    using System.Net;

    using Polly;
    using Polly.Retry;

    public class PollyRetry : ICircuitBreaker
    {
        private RetryPolicy policy;

        public PollyRetry()
        {
            policy = Policy.Handle<WebException>(e => e.Status != WebExceptionStatus.Success)
                .WaitAndRetry(
                    2,
                    retrytime => TimeSpan.FromSeconds(Math.Pow(2, retrytime)),
                    (Exception, timespan) =>

                    {
                        Console.WriteLine("Retry Count: " + timespan.ToString());
                    });
        }

        public void Execute(Action action)
        {
            policy.Execute(action);
        }
    }
}
