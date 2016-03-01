namespace Sfa.Eds.Das.Indexer.Common.Extensions
{
    using System;
    using System.Net;
    using System.Reflection;

    using log4net;

    using Polly;

    public static class FuncExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static T RetryWebRequest<T>(this Func<T> action)
        {
            var policy = Policy.Handle<WebException>(e => e.Status != WebExceptionStatus.Success)
                .WaitAndRetry(
                    2,
                    retrytime => TimeSpan.FromSeconds(Math.Pow(2, retrytime)),
                    (exception, timespan) =>

                        {
                            Log.Error("Failed to connect to site", exception);
                        });
            return policy.Execute<T>(action);
        }
    }
}