using System.Collections.Generic;

namespace Sfa.Das.FatApi.Client.Model
{
    public class OrganisationSummary
    {
        public string Id { get; set; }

        public string Uri { get; set; }

        public string Name { get; set; }

        public List<Link> Links { get; set; }

        public long? Ukprn { get; set; }
    }
}