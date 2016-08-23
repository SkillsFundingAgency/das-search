namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Logging
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Logging;

    public class DependencyLogEntry : ILogEntry
    {
        public string Identifier { get; set; }

        public double ResponseTime { get; set; }

        public int? ResponseCode { get; set; }

        public string Url { get; set; }
    }
}
