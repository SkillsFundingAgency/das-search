namespace Sfa.Das.ApprenticeshipInfoService.Health.Models
{
    using System.Collections.Generic;

    public class HealthModel
    {
        public Status Status { get; set; }

        public List<string> Errors { get; set; }

        public IEnumerable<ElasticsearchAlias> ElasticSearchAliases { get; set; }

        public ElasticsearchLog ElasticsearchLog { get; set; }

        public Status LarsZipFileStatus { get; set; }

        public Status CourseDirectoryStatus { get; set; }

        public long Took { get; set; }
    }
}
