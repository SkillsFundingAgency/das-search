using System;
using System.Reflection;
using System.Threading;
using log4net;

namespace Sfa.Eds.Indexer.Common.Helpers
{
    public class IndexerScheduler : IIndexerScheduler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Schedule(Action action, int intervalMinutes)
        {
            while (true)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(intervalMinutes));
            }
        }
    }
}