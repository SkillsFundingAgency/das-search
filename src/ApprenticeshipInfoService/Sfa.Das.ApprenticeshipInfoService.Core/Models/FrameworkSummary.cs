namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    using System;
    using System.Collections.Generic;

    public sealed class FrameworkSummary
    {
        public int Id { get; set; }

        public string Uri { get; set; }

        public string Title { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public int Level { get; set; }

        public TypicalLength TypicalLength { get; set; }
    }
}
