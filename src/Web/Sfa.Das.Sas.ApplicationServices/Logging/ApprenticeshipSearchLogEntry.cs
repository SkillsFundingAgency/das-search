using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Logging
{
    public class ApprenticeshipSearchLogEntry
    {
        public IEnumerable<string> Keywords { get; set; }

        public long TotalHits { get; set; }

        public string Postcode { get; set; }

        public double[] Coordinates { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
