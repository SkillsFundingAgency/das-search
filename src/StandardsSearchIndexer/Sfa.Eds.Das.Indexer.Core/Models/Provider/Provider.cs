using System.Collections.Generic;
using Sfa.Eds.Das.Indexer.Common.Models;

namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    public sealed class Provider : IIndexEntry
    {
        public long Id { get; set; }

        public int Ukprn { get; set; }

        public string Name { get; set; }

        public ContactInformation ContactDetails { get; set; }

        public string MarketingInfo { get; set; }

        public double? EmployerSatisfaction { get; set; }

        public double? LearnerSatisfaction { get; set; }

        public IEnumerable<FrameworkInformation> Frameworks { get; set; }

        public IEnumerable<Location> Locations { get; set; }

        public IEnumerable<StandardInformation> Standards { get; set; }
    }
}
