using System.Diagnostics;
using System.Reflection;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace Sfa.Das.Sas.Nlog.CustomLayouts
{
    [LayoutRenderer("assembly-file-version")]
    public class AssemblyVersionNlogLayout : LayoutRenderer
    {
        // As we only need this to be called once it is placed in a static variable
        private static readonly string AssemblyVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            logEvent.Properties.Add("assembly-version", AssemblyVersion);
            builder.Append(AssemblyVersion);
        }
    }
}
