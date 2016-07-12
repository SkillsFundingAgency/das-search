using System.Collections.Generic;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices.Logging
{
    using Sfa.Das.Sas.Core.Domain.Model;

    public class ApprenticeshipSearchLogEntry : ILogEntry
    {
        public IEnumerable<string> Keywords { get; set; }

        public long TotalHits { get; set; }

        public string Postcode { get; set; }

        public double[] Coordinates { get; set; }
    }
}
