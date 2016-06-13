using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Web.Logging
{
    public class ShortlistLogEntry : ILogEntry
    {
        public string Name => "Shortlist";

        public int StandardId { get; set; }
        public int FrameworkId { get; set; }

        public string ProviderId { get; set; }

        public int LocationId { get; set; }
    }
}
