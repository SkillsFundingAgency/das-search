using System;
using System.Configuration;
using System.Net;
using System.Threading;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.WindowsAzure.ServiceRuntime;
using Sfa.Das.Sas.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Das.Sas.Indexer.AzureWorkerRole.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Services;
using StructureMap;

namespace Sfa.Das.Sas.Indexer.AzureWorkerRole
{
    public class WorkerRole : RoleEntryPoint, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        private ILog _logger;

        private IWorkerRoleSettings _commonSettings;

        private IContainer _container;

        public override void Run()
        {
            _logger.Info("Starting indexer processing loop. ");

            while (true)
            {
                try
                {
                    _container.GetInstance<IIndexerJob>().Run();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Exception worker role");
                }

                Thread.Sleep(TimeSpan.FromSeconds(double.Parse(_commonSettings.WorkerRolePauseTime ?? "60")));
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

            _logger.Info("Indexer worker role started.");

            return result;
        }

        public override void OnStop()
        {
            _logger.Info("Stopping...");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            Dispose();

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

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new ApplicationInsightsInitializer());
        }
    }
}