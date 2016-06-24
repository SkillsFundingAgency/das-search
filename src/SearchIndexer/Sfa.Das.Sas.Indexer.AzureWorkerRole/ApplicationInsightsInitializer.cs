using Microsoft.ApplicationInsights.Channel;

namespace Sfa.Das.Sas.Indexer.AzureWorkerRole
{
    public class ApplicationInsightsInitializer : Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["Application"] = "Sfa.Das.Indexer";
        }
    }
}