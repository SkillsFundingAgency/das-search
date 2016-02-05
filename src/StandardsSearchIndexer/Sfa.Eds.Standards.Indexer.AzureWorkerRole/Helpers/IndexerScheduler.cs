using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
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