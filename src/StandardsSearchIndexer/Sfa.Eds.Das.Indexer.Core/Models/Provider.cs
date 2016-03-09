using Sfa.Eds.Das.Indexer.Core.Models.ProviderImport;
namespace Sfa.Eds.Das.Indexer.Core.Models

{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Indexer.Common.Models;

    public class Provider : IIndexEntry
    {
        public int Id { get; set;}
        public string Email { get; set; }
        public double? EmployerSatisfaction { get; set; }
        public IEnumerable<Framework> Frameworks { get; set; }
        public double? LearnerSatisfaction { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public string MarketingInfo { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public IEnumerable<Standard> Standards { get; set; }
        public string UkPrn { get; set; }
        public string Website { get; set; }
    }
}
