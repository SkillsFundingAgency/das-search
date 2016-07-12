using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Infrastructure.Logging
{
    public class ElasticSearchLogEntry : ILogEntry
    {
        public string Identifier { get; set; }
        public int? ReturnCode { get; set; }
        public bool? Successful { get; set; }
        public int SearchTime { get; set; }
        public double RequestTime { get; set; }
        public double MaxScore { get; set; }
        public int? HitCount { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
    }
}
