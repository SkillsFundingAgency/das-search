namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    using System.Collections.Generic;

    public class FrameworkListResponse
    {
        public Page Page { get; set; }

        public List<Framework> Frameworks { get; set; }
    }
}
