using Microsoft.ApplicationInsights.Channel;

namespace Sfa.Das.Sas.Web
{
    public sealed class ApplicationInsightsInitializer : Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["Application"] = "Sfa.Das.Web";
        }
    }
}