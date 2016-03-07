namespace Sfa.Eds.Das.Indexer.AzureWorkerRole
{
    using System;
    using System.Net;
    using System.Threading;

    using Microsoft.WindowsAzure.ServiceRuntime;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings;

    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        private ILog Log;

        private IWorkerRoleSettings _commonSettings;

        private IContainer _container;

        public override void Run()
        {
            Log.Info("Starting indexer... ");

            while (true)
            {
                try
                {
                    _container.GetInstance<IIndexerJob>().Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal("Exception from  " + ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(double.Parse(_commonSettings.WorkerRolePauseTime ?? "10")));
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            _container = IoC.Initialize();
            Log = _container.GetInstance<ILog>();
            _commonSettings = _container.GetInstance<IWorkerRoleSettings>();

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
    }
}