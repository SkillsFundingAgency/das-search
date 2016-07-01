using Microsoft.ApplicationInsights.DataContracts;

namespace Sfa.Das.Sas.Indexer.AzureWorkerRole
{
    public class ApplicationInsightsInitializer : Microsoft.ApplicationInsights.Extensibility.IContextInitializer
    {
        public void Initialize(TelemetryContext context)
        {
            context.Properties["Application"] = "Sfa.Das.Indexer";
        }
    }
}