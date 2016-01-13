using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private StandardControlQueueConsumer _standardControlQueueConsumer;

        public override void Run()
        {
            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole is running");

            _standardControlQueueConsumer = new StandardControlQueueConsumer();

            while (true)
            {
                try
                {
                    _standardControlQueueConsumer.CheckMessage("indexerqueue");
                }
                catch (Exception ex)
                {
                    //TODO: manage exceptions
                }

                Thread.Sleep(TimeSpan.FromMinutes(10));
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
