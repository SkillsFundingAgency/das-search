namespace Sfa.Eds.Das.Indexer.AzureWorkerRole
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Threading;
    using Core.Services;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.WindowsAzure.ServiceRuntime;

    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings;

    using StructureMap;

    public class WorkerRole : RoleEntryPoint, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        private ILog _logger;

        private IWorkerRoleSettings _commonSettings;

        private IContainer _container;

        public override void Run()
        {
            _logger.Info("Starting indexer... ");

            while (true)
            {
                try
                {
                    _container.GetInstance<IIndexerJob>().Run();
                }
                catch (Exception ex)
                {
                    _logger.Fatal("Exception from  " + ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(double.Parse(_commonSettings.WorkerRolePauseTime ?? "10")));
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            SetupApplicationInsights();
            _container = IoC.Initialize();
            _logger = _container.GetInstance<ILog>();
            _commonSettings = _container.GetInstance<IWorkerRoleSettings>();

            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            var result = base.OnStart();

            _logger.Info("Started...");

            return result;
        }

        public override void OnStop()
        {
            _logger.Info("Stopping...");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            _cancellationTokenSource.Dispose();
            _runCompleteEvent.Dispose();

            _logger.Info("Stopped...");
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _runCompleteEvent.Dispose();
        }

        private void SetupApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["iKey"];

            TelemetryConfiguration.Active.ContextInitializers.Add(new ApplicationInsightsInitializer());
        }
    }
}