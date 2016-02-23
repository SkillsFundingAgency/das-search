using System;
using System.Net;
using System.Reflection;
using System.Threading;
using log4net;
using Microsoft.WindowsAzure.ServiceRuntime;
using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Eds.Das.Indexer.Common.Configuration;
using Sfa.Eds.Das.Indexer.Common.Settings;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.AzureWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IContainer _container;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private ICommonSettings _commonSettings;

        public override void Run()
        {
            Log.Info("Starting indexer...");
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
            _commonSettings = _container.GetInstance<ICommonSettings>();

            Log4NetSettings.Initialise();

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