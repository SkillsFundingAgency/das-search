namespace Sfa.Das.WebTest.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Threading;

    public static class Retry
    {
        public static void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            Do<object>(
                () =>
                    {
                        action();
                        return null;
                    },
                retryInterval,
                retryCount);
        }

        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (var retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }

        public static bool DoUntil(Func<bool> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (var retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    if (action())
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            return false;
        }
    }
}