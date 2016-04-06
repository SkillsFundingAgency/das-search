namespace Sfa.Eds.Das.Web
{
    using Microsoft.ApplicationInsights.DataContracts;

    public class ApplicationInsightsInitializer : Microsoft.ApplicationInsights.Extensibility.IContextInitializer
    {
        public void Initialize(TelemetryContext context)
        {
            context.Properties["Application"] = "Sfa.Das.Web";
        }
    }
}