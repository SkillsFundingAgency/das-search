using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models
{
    public class ProviderBulk
    {
        public int UkPrn { get; set; }

        public string Name { get; set; }

        public string MarketingInfo { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string Phone { get; set; }

        public double LearnerSatisfaction { get; set; }

        public double EmployerSatisfaction { get; set; }

        public IEnumerable<StandardProvider> Standards { get; set; }

        public IEnumerable<FrameworkProvider> Frameworks { get; set; }

        public IEnumerable<Location> Locations { get; set; }
    }
}
