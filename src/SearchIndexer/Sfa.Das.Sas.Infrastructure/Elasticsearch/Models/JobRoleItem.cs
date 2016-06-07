namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    using Nest;

    public class JobRoleItem
    {
        [String(Analyzer = "english")]
        public string Title { get; set; }

        public string Description { get; set; }

    }
}