using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.WindowsAzure.ServiceRuntime;
using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Eds.Das.Indexer.Common.Configuration;
using Sfa.Eds.Das.Indexer.Common.Services;
using Sfa.Eds.Das.ProviderIndexer.Services;
using Sfa.Eds.Das.StandardIndexer.Services;

namespace Sfa.Eds.Das.Indexer.AzureWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private IGenericControlQueueConsumer _controlQueueConsumer;

        public override void Run()
        {
            Log.Info("Starting indexer...");
            while (true)
            {
                try
                {
                    var tasks = new List<Task>
                    {
                        // _controlQueueConsumer.CheckMessage<IStandardIndexerService>(),
                        _controlQueueConsumer.CheckMessage<IProviderIndexerService>()
                    };

                    Task.WaitAll(tasks.ToArray());
                }
                catch (Exception ex)
                {
                    Log.Fatal("Exception from  " + ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(10));
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            Initialise();

            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            var result = base.OnStart();

            Log.Info("Started...");

            return result;
        }

        public override void OnStop()
        {
            Log.Info("Stopping...");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            Log.Info("Stopped...");
        }

        private void Initialise()
        {
            var container = IoC.Initialize();
            _controlQueueConsumer = container.GetInstance<IGenericControlQueueConsumer>();

            Log4NetSettings.Initialise();
        }
    }
}