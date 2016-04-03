using Nest;

namespace Sfa.Infrastructure.Elasticsearch.Models
{
    public class Attachment
    {
        [String(Name="_content")]
        public string Content { get; set; }

        [String(Name= "_content_type")]
        public string ContentType { get; set; }

        [String(Name= "_name")]
        public string Name { get; set; }
    }
}