using System.Collections.Generic;

namespace Sfa.Das.FatApi.Client.Model
{
    public class StandardOrganisationSummary
    {
        public string StandardCode { get; set; }

        public string Uri { get; set; }

        public IEnumerable<Period> Periods { get; set; }
    }
}