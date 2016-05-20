using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfa.Das.Sas.Web.Models
{
    public class ShortlistedApprenticeship
    {
        public int ApprenticeshipId { get; set; }

        public List<int> ProvidersId { get; set; }
    }
}