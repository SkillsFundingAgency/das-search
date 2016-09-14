namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    using System.Collections.Generic;

    public class StandardListResponse
    {
        public Page Page { get; set; }

        public List<Standard> Standards { get; set; }
    }
}
