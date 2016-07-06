using Nest;
using Sfa.Das.Sas.Indexer.Core.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    using System;
    using System.Collections.Generic;

    using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration;

    public sealed class FrameworkDocument : ApprenticeshipDocument, IIndexEntry
    {
        public string FrameworkId { get; set; }

        public int FrameworkCode { get; set; }

        [String(Analyzer = ElasticsearchConfiguration.AnalyserEnglishCustom)]
        public string FrameworkName { get; set; }

        public int PathwayCode { get; set; }

        [String(Analyzer = ElasticsearchConfiguration.AnalyserEnglishCustom)]
        public string PathwayName { get; set; }

        public IEnumerable<JobRoleItem> JobRoleItems { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string EntryRequirements { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string CompletionQualifications { get; set; }
    }
}
