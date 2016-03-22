namespace Sfa.Infrastructure.Elasticsearch
{
    public class FrameworkDocument
    {
        public string FrameworkId { get; set; }

        public string Title { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public string IssuingAuthorityTitle { get; set; }

        public int Level { get; set; }
    }
}
