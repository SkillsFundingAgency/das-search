using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ShortlistedApprenticeship
    {
        public int ApprenticeshipId { get; set; }

        public List<ShortlistedProvider> ProvidersUkrpnAndLocation { get; set; }
    }
}