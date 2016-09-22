namespace Sfa.Das.ApprenticeshipInfoService.Health.Models
{
    using System.Collections.Generic;

    public class ElasticsearchLog
    {
        public long  ErrorCount { get; set; }

        public IEnumerable<string> LogErrors { get; set; }
    }
}