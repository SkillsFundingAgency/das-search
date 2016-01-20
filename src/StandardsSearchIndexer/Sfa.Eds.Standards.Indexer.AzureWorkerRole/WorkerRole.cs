using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole
{
    using System.Diagnostics;
    using System.IO;

    using log4net;

    public class WorkerRole : RoleEntryPoint
    {
        // private ILog _log;
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private IStandardControlQueueConsumer _standardControlQueueConsumer;
        private IStandardIndexSettings _standardIndexSettings;

        public override void Run()
        {
            var container = IoC.Initialize();

            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole is running");

            _standardControlQueueConsumer = container.GetInstance<IStandardControlQueueConsumer>();
            _standardIndexSettings = container.GetInstance<IStandardIndexSettings>();

            _log.Info("Starting...");

            while (true)
            {
                try
                {
                    _standardControlQueueConsumer.CheckMessage(_standardIndexSettings.QueueName);
                }
                catch (Exception ex)
                {
                    var error = ex.Message;

                    // TODO: manage exceptions
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

            _log.Info("Started...");

            return result;
        }

        public override void OnStop()
        {
            _log.Info("Stopping...");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            _log.Info("Stopped...");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                _log.Info("Working");
                await Task.Delay(1000);
            }
        }

        private void Initialise()
        {
            var container = IoC.Initialize();

            // _log = container.GetInstance<ILog>();
            _standardControlQueueConsumer = container.GetInstance<IStandardControlQueueConsumer>();
            _standardIndexSettings = container.GetInstance<IStandardIndexSettings>();
        }
    }
}