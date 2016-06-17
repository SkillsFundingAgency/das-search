using Nest;
using Sfa.Das.Sas.Indexer.Core.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    using System;
    using System.Collections.Generic;

    public sealed class FrameworkDocument : IApprenticeshipDocument, IIndexEntry
    {
        [String(Analyzer = "english")]
        public string Title { get; set; }

        public int Level { get; set; }

        public int FrameworkCode { get; set; }

        public string FrameworkId { get; set; }

        [String(Analyzer = "english")]
        public string FrameworkName { get; set; }

        public int PathwayCode { get; set; }

        [String(Analyzer = "english")]
        public string PathwayName { get; set; }

        public IEnumerable<JobRoleItem> JobRoleItems { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}
