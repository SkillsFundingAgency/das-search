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
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private IStandardControlQueueConsumer _standardControlQueueConsumer;
        private IStandardIndexSettings _standardIndexSettings;

        public override void Run()
        {
            var container = IoC.Initialize();

            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole is running");
            Trace.TraceInformation("PATATA");

            _standardControlQueueConsumer = container.GetInstance<IStandardControlQueueConsumer>();
            _standardIndexSettings = container.GetInstance<IStandardIndexSettings>();

            while (true)
            {
                try
                {
                    Trace.TraceInformation("LET'S GO TO CHECK MESSAGES into " + _standardIndexSettings.QueueName);

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

            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            var result = base.OnStart();

            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Sfa.Eds.Standards.Indexer.AzureWorkerRole is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

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

    sealed class SampleEventSourceWriter : EventSource
    {
        public static SampleEventSourceWriter Log = new SampleEventSourceWriter();
        public void SendEnums(MyColor color, MyFlags flags) { if (IsEnabled()) WriteEvent(1, (int)color, (int)flags); }// Cast enums to int for efficient logging.
        public void MessageMethod(string Message) { if (IsEnabled()) WriteEvent(2, Message); }
        public void SetOther(bool flag, int myInt) { if (IsEnabled()) WriteEvent(3, flag, myInt); }
        public void HighFreq(int value) { if (IsEnabled()) WriteEvent(4, value); }
    }
    enum MyColor
    {
        Red,
        Blue,
        Green
    }
    [Flags]
    enum MyFlags
    {
        Flag1 = 1,
        Flag2 = 2,
        Flag3 = 4
    }
}