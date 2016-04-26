using System;
using System.Net;
using Polly;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Services
{
    public class RetryService : IRetryWebRequest
    {
        private readonly ILog _log;

        public RetryService(ILog log)
        {
            _log = log;
        }

        public T RetryWeb<T>(Func<T> action)
        {
            var policy = Policy.Handle<WebException>(e => e.Status != WebExceptionStatus.Success)
                .WaitAndRetry(
                    2,
                    retrytime => TimeSpan.FromSeconds(Math.Pow(2, retrytime)),
                    (exception, timespan) => { _log.Error("Failed to connect to site", exception); });
            return policy.Execute(action);
        }
    }
}